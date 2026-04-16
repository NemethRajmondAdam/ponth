import React, { useState, useEffect, useRef } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import Layout from '../components/Layout';
import { useAuth } from '../context/AuthContext';
import api from '../api';

// We'll dynamically import Html5QrcodeScanner to avoid SSR issues
let Html5Qrcode = null;

export default function OrderDrink() {
    const { user, isLoggedIn, logout } = useAuth();
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();

    // States: 'scanning', 'ordering', 'success'
    const [step, setStep] = useState('scanning');
    const [boxId, setBoxId] = useState(null);
    const [boxNumber, setBoxNumber] = useState(null);
    const [drinks, setDrinks] = useState([]);
    const [customCocktails, setCustomCocktails] = useState([]);
    const [cart, setCart] = useState({});
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [submitting, setSubmitting] = useState(false);
    const [scannerReady, setScannerReady] = useState(false);
    // Cocktail customization modal
    const [customizeDrink, setCustomizeDrink] = useState(null);
    const [ingredientMods, setIngredientMods] = useState({});
    const scannerRef = useRef(null);
    const html5QrCodeRef = useRef(null);

    useEffect(() => {
        if (!isLoggedIn) {
            navigate('/login');
            return;
        }

        // If ?continue=true, skip scanning and load existing open bill
        if (searchParams.get('continue') === 'true') {
            loadExistingBill();
        }
    }, [isLoggedIn]);

    const loadExistingBill = async () => {
        setLoading(true);
        setError('');
        try {
            const [billRes, drinksRes, customRes] = await Promise.all([
                api.get('/my-open-bill'),
                api.get('/drinks'),
                api.get('/my-custom-cocktails'),
            ]);

            const bill = billRes.data.bill;
            if (!bill) {
                setError('No active bill found. Please scan a QR code first.');
                setLoading(false);
                return;
            }

            setBoxId(bill.box_id);
            setBoxNumber(bill.box?.number || bill.box_id);
            setDrinks(drinksRes.data.filter(d => d.name !== 'CostumeCocktail'));
            setCustomCocktails(customRes.data);
            setStep('ordering');
        } catch (err) {
            console.error(err);
            setError('Failed to load bill data.');
        } finally {
            setLoading(false);
        }
    };

    // Initialize QR scanner
    useEffect(() => {
        if (step !== 'scanning') return;

        let mounted = true;

        const initScanner = async () => {
            try {
                // Dynamic import
                const module = await import('html5-qrcode');
                Html5Qrcode = module.Html5Qrcode;

                if (!mounted) return;

                const scanner = new Html5Qrcode('qr-reader');
                html5QrCodeRef.current = scanner;

                await scanner.start(
                    { facingMode: 'environment' },
                    {
                        fps: 10,
                        qrbox: { width: 250, height: 250 },
                        aspectRatio: 1,
                    },
                    (decodedText) => {
                        handleScanSuccess(decodedText, scanner);
                    },
                    (errorMessage) => {
                        // Ignore scan errors (no QR found in frame)
                    }
                );

                if (mounted) setScannerReady(true);
            } catch (err) {
                console.error('Scanner init error:', err);
                if (mounted) {
                    setError('Could not access camera. Please allow camera permissions and try again.');
                }
            }
        };

        // Small delay to ensure DOM element exists
        const timeout = setTimeout(initScanner, 300);

        return () => {
            mounted = false;
            clearTimeout(timeout);
            if (html5QrCodeRef.current) {
                html5QrCodeRef.current.stop().catch(() => { });
                html5QrCodeRef.current = null;
            }
        };
    }, [step]);

    const handleScanSuccess = async (decodedText, scanner) => {
        // Parse PONTH_BOX_X format
        const match = decodedText.match(/^PONTH_BOX_(\d+)$/);
        if (!match) {
            setError('Invalid QR code. Please scan a valid Ponth box QR code.');
            return;
        }

        const scannedBoxId = parseInt(match[1]);

        // Stop scanner
        try {
            await scanner.stop();
            html5QrCodeRef.current = null;
        } catch (e) { }

        setBoxId(scannedBoxId);
        setError('');
        setLoading(true);

        try {
            // 1. Check/open bill for this box
            const billRes = await api.post(`/check-bill/${scannedBoxId}`);
            const bill = billRes.data.bill;
            setBoxNumber(bill.box?.number || scannedBoxId);

            // 2. Fetch drinks and custom cocktails
            const [drinksRes, customRes] = await Promise.all([
                api.get('/drinks'),
                api.get('/my-custom-cocktails'),
            ]);
            setDrinks(drinksRes.data.filter(d => d.name !== 'CostumeCocktail'));
            setCustomCocktails(customRes.data);
            setStep('ordering');
        } catch (err) {
            console.error(err);
            if (err.response?.status === 409) {
                setError(err.response.data.error);
            } else if (err.response?.status === 401) {
                setError('Session expired. Please log in again.');
                setTimeout(() => { logout(); navigate('/login'); }, 2000);
            } else {
                const errorMsg = err.response?.data?.error || err.message || 'Unknown error';
                setError(`Failed to load data: ${errorMsg}`);
            }
            setStep('scanning');
        } finally {
            setLoading(false);
        }
    };

    // --- Cart helpers (cart = { drinkId: { quantity, extras: {ingredientId: delta} } }) ---
    const updateCart = (drinkId, delta) => {
        setCart(prev => {
            const entry = prev[drinkId] || { quantity: 0, extras: {} };
            const next = Math.max(0, entry.quantity + delta);
            const newCart = { ...prev };
            if (next === 0) {
                delete newCart[drinkId];
            } else {
                newCart[drinkId] = { ...entry, quantity: next };
            }
            return newCart;
        });
    };

    const addCocktailToCart = (drinkId, extras) => {
        setCart(prev => {
            const entry = prev[drinkId] || { quantity: 0, extras: {} };
            return {
                ...prev,
                [drinkId]: { quantity: entry.quantity + 1, extras: { ...extras } },
            };
        });
    };

    const getItemExtrasTotal = (drinkId, entry) => {
        const drink = drinks.find(d => d.id === parseInt(drinkId));
        if (!drink || drink.type !== 'Cocktail' || !drink.cocktail) return 0;
        const cocktailDrinks = drink.cocktail.drinks || [];
        const ingredients = drink.cocktail.ingredients || [];
        let extra = 0;
        for (const [key, delta] of Object.entries(entry.extras || {})) {
            if (delta > 0) {
                if (key.startsWith('d_')) {
                    const dId = parseInt(key.slice(2));
                    const d = cocktailDrinks.find(x => x.id === dId);
                    if (d) extra += d.mixing_price * delta;
                } else if (key.startsWith('i_')) {
                    const iId = parseInt(key.slice(2));
                    const ing = ingredients.find(x => x.id === iId);
                    if (ing) extra += ing.price * delta;
                }
            }
        }
        return extra;
    };

    const getCartTotal = () => {
        return Object.entries(cart).reduce((sum, [key, entry]) => {
            // Custom cocktail entries are keyed as 'custom_X'
            if (key.startsWith('custom_')) {
                const ccId = parseInt(key.slice(7));
                const cc = customCocktails.find(c => c.id === ccId);
                return sum + (cc ? cc.price * entry.quantity : 0);
            }
            const drink = drinks.find(d => d.id === parseInt(key));
            if (!drink) return sum;
            const baseTotal = drink.price * entry.quantity;
            const extrasTotal = getItemExtrasTotal(key, entry) * entry.quantity;
            return sum + baseTotal + extrasTotal;
        }, 0);
    };

    const getCartItemCount = () => {
        return Object.values(cart).reduce((sum, entry) => sum + entry.quantity, 0);
    };

    // --- Cocktail customization modal ---
    const openCustomize = (drink) => {
        const mods = {};
        // Initialize drink component mods
        if (drink.cocktail?.drinks) {
            drink.cocktail.drinks.forEach(d => {
                mods[`d_${d.id}`] = 0;
            });
        }
        // Initialize ingredient mods
        if (drink.cocktail?.ingredients) {
            drink.cocktail.ingredients.forEach(ing => {
                mods[`i_${ing.id}`] = 0;
            });
        }
        setIngredientMods(mods);
        setCustomizeDrink(drink);
    };

    const updateMod = (key, delta, defaultQty) => {
        setIngredientMods(prev => ({
            ...prev,
            [key]: Math.max(-defaultQty, (prev[key] || 0) + delta),
        }));
    };

    const getExtrasPrice = () => {
        if (!customizeDrink?.cocktail) return 0;
        let total = 0;
        const cocktailDrinks = customizeDrink.cocktail.drinks || [];
        const ingredients = customizeDrink.cocktail.ingredients || [];
        for (const [key, delta] of Object.entries(ingredientMods)) {
            if (delta > 0) {
                if (key.startsWith('d_')) {
                    const d = cocktailDrinks.find(x => x.id === parseInt(key.slice(2)));
                    if (d) total += d.mixing_price * delta;
                } else if (key.startsWith('i_')) {
                    const ing = ingredients.find(x => x.id === parseInt(key.slice(2)));
                    if (ing) total += ing.price * delta;
                }
            }
        }
        return total;
    };

    const confirmCustomize = () => {
        const extras = {};
        for (const [key, delta] of Object.entries(ingredientMods)) {
            if (delta !== 0) extras[key] = delta;
        }
        addCocktailToCart(customizeDrink.id, extras);
        setCustomizeDrink(null);
        setIngredientMods({});
    };

    // --- Order submission ---
    const handleOrder = async () => {
        if (Object.keys(cart).length === 0) {
            setError('Please select at least one drink.');
            return;
        }

        setSubmitting(true);
        setError('');

        const items = Object.entries(cart).map(([key, entry]) => {
            // Custom cocktail
            if (key.startsWith('custom_')) {
                return {
                    custom_cocktail_id: parseInt(key.slice(7)),
                    quantity: entry.quantity,
                };
            }
            const item = {
                drink_id: parseInt(key),
                quantity: entry.quantity,
            };
            // Include extras if any (typed: d_ for drinks, i_ for ingredients)
            const extras = Object.entries(entry.extras || {}).filter(([, d]) => d !== 0);
            if (extras.length > 0) {
                item.extras = extras.map(([eKey, delta]) => {
                    if (eKey.startsWith('d_')) {
                        return { type: 'Drink', item_id: parseInt(eKey.slice(2)), delta };
                    } else {
                        return { type: 'Ingredient', item_id: parseInt(eKey.slice(2)), delta };
                    }
                });
            }
            return item;
        });

        try {
            await api.post('/mobile-order', { items });
            setStep('success');
        } catch (err) {
            console.error(err);
            if (err.response?.status === 401) {
                setError('Session expired. Please log in again. Redirecting...');
                setTimeout(() => {
                    logout();
                    navigate('/login');
                }, 2000);
            } else if (err.response?.data?.message) {
                setError(err.response.data.message);
            } else {
                setError('Failed to place order. Please try again.');
            }
        } finally {
            setSubmitting(false);
        }
    };

    const resetOrder = () => {
        setStep('scanning');
        setBoxId(null);
        setBoxNumber(null);
        setCart({});
        setError('');
        setCustomizeDrink(null);
        setIngredientMods({});
        setCustomCocktails([]);
    };

    // Group drinks by type
    const groupedDrinks = drinks.reduce((acc, drink) => {
        const type = drink.type || 'Other';
        if (!acc[type]) acc[type] = [];
        acc[type].push(drink);
        return acc;
    }, {});

    if (!isLoggedIn) return null;

    return (
        <Layout>
            <div className="min-h-[80vh] px-4 py-8 max-w-lg mx-auto">
                {/* Header */}
                <div className="mb-8 text-center">
                    <div className="inline-flex items-center justify-center w-14 h-14 rounded-2xl bg-gradient-to-br from-sky-700 to-sky-950 shadow-lg shadow-sky-700/10 mb-4">
                        <svg className="w-7 h-7 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v1m6 11h2m-6 0h-2v4m0-11v3m0 0h.01M12 12h4.01M16 20h4M4 12h4m12 0h.01M5 8h2a1 1 0 001-1V5a1 1 0 00-1-1H5a1 1 0 00-1 1v2a1 1 0 001 1zm12 0h2a1 1 0 001-1V5a1 1 0 00-1-1h-2a1 1 0 00-1 1v2a1 1 0 001 1zM5 20h2a1 1 0 001-1v-2a1 1 0 00-1-1H5a1 1 0 00-1 1v2a1 1 0 001 1z" />
                        </svg>
                    </div>
                    <h1 className="text-2xl font-black tracking-tight text-gray-900 dark:text-white">
                        Order Drink
                    </h1>
                    <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
                        {step === 'scanning' && 'Scan the QR code on your table'}
                        {step === 'ordering' && `Ordering for Box ${boxNumber}`}
                        {step === 'success' && 'Order placed!'}
                    </p>
                </div>

                {/* Error */}
                {error && (
                    <div className="mb-6 bg-red-50 dark:bg-red-500/10 text-red-500 text-sm p-4 rounded-xl text-center font-medium">
                        {error}
                    </div>
                )}

                {/* Loading */}
                {loading && (
                    <div className="flex justify-center py-20">
                        <div className="w-10 h-10 border-4 border-sky-700 border-t-transparent rounded-full animate-spin"></div>
                    </div>
                )}

                {/* Step 1: QR Scanner */}
                {step === 'scanning' && !loading && (
                    <div className="bg-white dark:bg-[#0c0d12] rounded-2xl shadow-xl border border-gray-100 dark:border-white/5 overflow-hidden">
                        <div className="p-6">
                            <div className="flex items-center gap-3 mb-4">
                                <div className="w-8 h-8 rounded-lg bg-sky-600 dark:bg-sky-700/10 flex items-center justify-center text-sky-700 font-bold text-sm">1</div>
                                <h2 className="text-lg font-bold text-gray-900 dark:text-white">Scan Table QR Code</h2>
                            </div>
                            <p className="text-sm text-gray-500 dark:text-gray-400 mb-4">
                                Point your camera at the QR code on the table to start ordering.
                            </p>
                            <div id="qr-reader" className="rounded-xl overflow-hidden border border-gray-200 dark:border-white/10"></div>
                            {!scannerReady && !error && (
                                <div className="flex items-center justify-center py-8 gap-3">
                                    <div className="w-5 h-5 border-2 border-sky-700 border-t-transparent rounded-full animate-spin"></div>
                                    <span className="text-sm text-gray-500">Starting camera...</span>
                                </div>
                            )}
                        </div>
                    </div>
                )}

                {/* Step 2: Drink Selection */}
                {step === 'ordering' && (
                    <>
                        <div className="bg-white dark:bg-[#0c0d12] rounded-2xl shadow-xl border border-gray-100 dark:border-white/5 overflow-hidden mb-4">
                            <div className="p-6">
                                <div className="flex items-center justify-between mb-4">
                                    <div className="flex items-center gap-3">
                                        <div className="w-8 h-8 rounded-lg bg-sky-600 dark:bg-sky-700/10 flex items-center justify-center text-sky-700 font-bold text-sm">2</div>
                                        <h2 className="text-lg font-bold text-gray-900 dark:text-white">Select Drinks</h2>
                                    </div>
                                    <button
                                        onClick={resetOrder}
                                        className="text-xs font-medium text-gray-400 hover:text-gray-600 dark:hover:text-gray-300 transition-colors"
                                    >
                                        ← Scan again
                                    </button>
                                </div>

                                {/* Box info badge */}
                                <div className="inline-flex items-center gap-2 px-3 py-1.5 bg-sky-70 dark:bg-sky-700/10 rounded-lg mb-6">
                                    <div className="w-5 h-5 rounded bg-gradient-to-br from-sky-700 to-sky-950 flex items-center justify-center text-white text-xs font-bold">{boxNumber}</div>
                                    <span className="text-sm font-semibold text-sky-700 dark:text-sky-600">Box {boxNumber}</span>
                                </div>

                                {/* Drink list by type */}
                                <div className="space-y-6">
                                    {/* My Custom Cocktails */}
                                    {customCocktails.length > 0 && (
                                        <div>
                                            <div className="flex items-center gap-3 mb-3">
                                                <h3 className="text-sm font-bold text-sky-700 uppercase tracking-wider">🧪 My Cocktails</h3>
                                                <div className="flex-1 h-px bg-sky-600 dark:bg-sky-700/10"></div>
                                            </div>
                                            <div className="space-y-2">
                                                {customCocktails.map(cc => {
                                                    const cartKey = `custom_${cc.id}`;
                                                    const entry = cart[cartKey];
                                                    const qty = entry?.quantity || 0;
                                                    return (
                                                        <div key={cartKey}
                                                            className={`flex items-center justify-between p-3 rounded-xl transition-all ${qty > 0
                                                                    ? 'bg-sky-70 dark:bg-sky-700/10 ring-1 ring-sky-700 dark:ring-sky-700/10'
                                                                    : 'bg-gray-50 dark:bg-white/[0.03] hover:bg-gray-100 dark:hover:bg-white/[0.05]'
                                                                }`}
                                                        >
                                                            <div className="flex-1 min-w-0 mr-3">
                                                                <p className="text-sm font-semibold text-gray-900 dark:text-white truncate">🍹 {cc.cocktail_name}</p>
                                                                <div className="flex items-center gap-1.5">
                                                                    <p className="text-xs font-bold text-sky-700">{cc.price} Ft</p>
                                                                    <span className="text-[10px] px-1.5 py-0.5 rounded bg-sky-600 dark:bg-sky-700/10 text-sky-700 font-bold">Custom</span>
                                                                </div>
                                                            </div>
                                                            <div className="flex items-center gap-2">
                                                                {qty > 0 && (
                                                                    <button
                                                                        onClick={() => updateCart(cartKey, -1)}
                                                                        className="w-8 h-8 rounded-lg bg-white dark:bg-[#1a1c23] border border-gray-200 dark:border-white/10 flex items-center justify-center text-gray-600 dark:text-gray-300 hover:bg-red-50 dark:hover:bg-red-500/10 hover:text-red-500 hover:border-red-200 dark:hover:border-red-500/20 transition-all"
                                                                    >
                                                                        <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M20 12H4" /></svg>
                                                                    </button>
                                                                )}
                                                                {qty > 0 && (
                                                                    <span className="w-6 text-center text-sm font-bold text-gray-900 dark:text-white">{qty}</span>
                                                                )}
                                                                <button
                                                                    onClick={() => updateCart(cartKey, 1)}
                                                                    className="w-8 h-8 rounded-lg bg-gradient-to-br from-sky-700 to-sky-950 flex items-center justify-center text-white shadow-sm hover:shadow-md hover:scale-105 transition-all"
                                                                >
                                                                    <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" /></svg>
                                                                </button>
                                                            </div>
                                                        </div>
                                                    );
                                                })}
                                            </div>
                                        </div>
                                    )}

                                    {/* Standard drinks */}
                                    {Object.entries(groupedDrinks).map(([type, items]) => (
                                        <div key={type}>
                                            <div className="flex items-center gap-3 mb-3">
                                                <h3 className="text-sm font-bold text-gray-500 dark:text-gray-400 uppercase tracking-wider">{type}s</h3>
                                                <div className="flex-1 h-px bg-gray-100 dark:bg-white/5"></div>
                                            </div>
                                            <div className="space-y-2">
                                                {items.map(drink => {
                                                    const entry = cart[drink.id];
                                                    const qty = entry?.quantity || 0;
                                                    const isCocktail = drink.type === 'Cocktail';
                                                    const hasExtras = entry && Object.keys(entry.extras || {}).length > 0;
                                                    const extrasTotal = entry ? getItemExtrasTotal(drink.id, entry) : 0;
                                                    return (
                                                        <div key={drink.id}
                                                            className={`flex items-center justify-between p-3 rounded-xl transition-all ${qty > 0
                                                                    ? 'bg-sky-70 dark:bg-sky-700/10 ring-1 ring-sky-700 dark:ring-sky-700/20'
                                                                    : 'bg-gray-50 dark:bg-white/[0.03] hover:bg-gray-100 dark:hover:bg-white/[0.05]'
                                                                }`}
                                                        >
                                                            <div className="flex-1 min-w-0 mr-3">
                                                                <p className="text-sm font-semibold text-gray-900 dark:text-white truncate">{drink.name}</p>
                                                                <div className="flex items-center gap-1.5">
                                                                    <p className="text-xs font-bold text-sky-700">{drink.price} Ft</p>
                                                                    {hasExtras && extrasTotal > 0 && (
                                                                        <span className="text-[10px] font-bold text-sky-700">+{extrasTotal} Ft</span>
                                                                    )}
                                                                    {isCocktail && (
                                                                        <span className="text-[10px] px-1.5 py-0.5 rounded bg-sky-600 dark:bg-sky-700/10 text-sky-700 font-bold">🍹</span>
                                                                    )}
                                                                </div>
                                                            </div>
                                                            <div className="flex items-center gap-2">
                                                                {qty > 0 && (
                                                                    <button
                                                                        onClick={() => updateCart(drink.id, -1)}
                                                                        className="w-8 h-8 rounded-lg bg-white dark:bg-[#1a1c23] border border-gray-200 dark:border-white/10 flex items-center justify-center text-gray-600 dark:text-gray-300 hover:bg-red-50 dark:hover:bg-red-500/10 hover:text-red-500 hover:border-red-200 dark:hover:border-red-500/20 transition-all"
                                                                    >
                                                                        <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M20 12H4" /></svg>
                                                                    </button>
                                                                )}
                                                                {qty > 0 && (
                                                                    <span className="w-6 text-center text-sm font-bold text-gray-900 dark:text-white">{qty}</span>
                                                                )}
                                                                <button
                                                                    onClick={() => isCocktail ? openCustomize(drink) : updateCart(drink.id, 1)}
                                                                    className="w-8 h-8 rounded-lg bg-gradient-to-br from-sky-700 to-sky-950 flex items-center justify-center text-white shadow-sm hover:shadow-md hover:scale-105 transition-all"
                                                                >
                                                                    <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" /></svg>
                                                                </button>
                                                            </div>
                                                        </div>
                                                    );
                                                })}
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            </div>
                        </div>

                        {/* Cart summary & order button */}
                        {getCartItemCount() > 0 && (
                            <div className="sticky bottom-4 bg-white dark:bg-[#0c0d12] rounded-2xl shadow-2xl border border-gray-200 dark:border-white/10 p-4">
                                <div className="flex items-center justify-between mb-3">
                                    <div>
                                        <p className="text-xs text-gray-500 dark:text-gray-400">{getCartItemCount()} items</p>
                                        <p className="text-xl font-black text-gray-900 dark:text-white">{getCartTotal().toLocaleString()} Ft</p>
                                    </div>
                                </div>
                                <button
                                    onClick={handleOrder}
                                    disabled={submitting}
                                    className="w-full py-3 rounded-xl bg-gradient-to-r from-sky-700 to-sky-950 text-white font-bold text-sm shadow-lg shadow-sky-700/10 hover:from-sky-700 hover:to-sky-950 transition-all hover:-translate-y-0.5 active:translate-y-0 disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:translate-y-0"
                                >
                                    {submitting ? 'Placing Order...' : 'Place Order'}
                                </button>
                            </div>
                        )}
                    </>
                )}

                {/* Step 3: Success */}
                {step === 'success' && (
                    <div className="bg-white dark:bg-[#0c0d12] rounded-2xl shadow-xl border border-gray-100 dark:border-white/5 p-8 text-center">
                        <div className="inline-flex items-center justify-center w-16 h-16 rounded-full bg-sky-600 dark:bg-sky-700/10 mb-5">
                            <svg className="w-8 h-8 text-sky-700" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
                            </svg>
                        </div>
                        <h2 className="text-2xl font-black text-gray-900 dark:text-white mb-2">Order Placed!</h2>
                        <p className="text-sm text-gray-500 dark:text-gray-400 mb-6">
                            Your drinks for Box {boxNumber} are being prepared.
                        </p>
                        <div className="space-y-3">
                            <button
                                onClick={resetOrder}
                                className="w-full py-3 rounded-xl bg-gradient-to-r from-sky-700 to-sky-950 text-white font-bold text-sm shadow-lg shadow-sky-700/10 hover:from-sky-700 hover:to-sky-950 transition-all hover:-translate-y-0.5 active:translate-y-0"
                            >
                                Order More
                            </button>
                            <button
                                onClick={() => navigate('/profile')}
                                className="w-full py-3 rounded-xl bg-sky-700/10 text-sky-700 dark:text-sky-700 font-bold text-sm hover:bg-sky-700/10 transition-all"
                            >
                                View Active Bill
                            </button>
                            <button
                                onClick={() => navigate('/')}
                                className="w-full py-3 rounded-xl bg-gray-100 dark:bg-white/5 text-gray-700 dark:text-gray-300 font-bold text-sm hover:bg-gray-200 dark:hover:bg-white/10 transition-all"
                            >
                                Back to Home
                            </button>
                        </div>
                    </div>
                )}

                {/* Cocktail Customize Modal */}
                {customizeDrink && (
                    <div className="fixed inset-0 z-50 flex items-end sm:items-center justify-center p-4" onClick={() => setCustomizeDrink(null)}>
                        <div className="fixed inset-0 bg-black/50 backdrop-blur-sm" />
                        <div
                            className="relative w-full max-w-md bg-white dark:bg-[#0c0d12] rounded-2xl shadow-2xl border border-gray-100 dark:border-white/10 overflow-hidden max-h-[80vh] flex flex-col"
                            onClick={e => e.stopPropagation()}
                        >
                            {/* Modal Header */}
                            <div className="p-5 border-b border-gray-100 dark:border-white/5">
                                <div className="flex items-center justify-between">
                                    <div>
                                        <h3 className="text-lg font-black text-gray-900 dark:text-white">🍹 {customizeDrink.name}</h3>
                                        <p className="text-xs text-gray-400 mt-0.5">Customize your cocktail</p>
                                    </div>
                                    <button
                                        onClick={() => setCustomizeDrink(null)}
                                        className="w-8 h-8 rounded-lg bg-gray-100 dark:bg-white/5 flex items-center justify-center text-gray-400 hover:text-gray-600 transition-colors"
                                    >
                                        ✕
                                    </button>
                                </div>
                            </div>

                            {/* Components List */}
                            <div className="flex-1 overflow-y-auto p-5 space-y-4">
                                {/* Drinks (Alcohols + Softdrinks) */}
                                {customizeDrink.cocktail?.drinks?.length > 0 && (
                                    <div>
                                        <p className="text-xs font-bold text-gray-400 uppercase tracking-wider mb-2">🥃 Drinks</p>
                                        <div className="space-y-2">
                                            {customizeDrink.cocktail.drinks.map(d => {
                                                const key = `d_${d.id}`;
                                                const defaultQty = d.pivot?.quantity || 0;
                                                const mod = ingredientMods[key] || 0;
                                                const currentQty = defaultQty + mod;
                                                return (
                                                    <div key={key} className={`flex items-center justify-between p-3 rounded-xl transition-all ${
                                                        mod !== 0
                                                            ? mod > 0
                                                                ? 'bg-sky-70 dark:bg-sky-700/10 ring-1 ring-sky-700 dark:ring-sky-700/10'
                                                                : 'bg-red-50 dark:bg-red-500/10 ring-1 ring-red-200 dark:ring-red-500/20'
                                                            : 'bg-gray-50 dark:bg-white/[0.03]'
                                                    }`}>
                                                        <div className="flex-1 min-w-0 mr-3">
                                                            <p className="text-sm font-semibold text-gray-900 dark:text-white">{d.name}</p>
                                                            <div className="flex items-center gap-2">
                                                                <span className="text-[10px] text-gray-400">Default: {defaultQty} {d.mixing_quantity?.quantity || 'ml'}</span>
                                                                {mod > 0 && (
                                                                    <span className="text-[10px] font-bold text-sky-700">+{d.mixing_price * mod} Ft</span>
                                                                )}
                                                            </div>
                                                        </div>
                                                        <div className="flex items-center gap-2">
                                                            <button
                                                                onClick={() => updateMod(key, -1, defaultQty)}
                                                                disabled={currentQty <= 0}
                                                                className="w-8 h-8 rounded-lg bg-white dark:bg-[#1a1c23] border border-gray-200 dark:border-white/10 flex items-center justify-center text-gray-600 dark:text-gray-300 hover:bg-red-50 hover:text-red-500 hover:border-red-200 transition-all disabled:opacity-30 disabled:cursor-not-allowed"
                                                            >
                                                                <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M20 12H4" /></svg>
                                                            </button>
                                                            <span className={`w-6 text-center text-sm font-bold ${mod > 0 ? 'text-emerald-500' : mod < 0 ? 'text-red-500' : 'text-gray-900 dark:text-white'}`}>
                                                                {currentQty}
                                                            </span>
                                                            <button
                                                                onClick={() => updateMod(key, 1, defaultQty)}
                                                                className="w-8 h-8 rounded-lg bg-gradient-to-br from-sky-700 to-sky-950 flex items-center justify-center text-white shadow-sm hover:shadow-md hover:scale-105 transition-all"
                                                            >
                                                                <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" /></svg>
                                                            </button>
                                                        </div>
                                                    </div>
                                                );
                                            })}
                                        </div>
                                    </div>
                                )}

                                {/* Ingredients */}
                                {customizeDrink.cocktail?.ingredients?.length > 0 && (
                                    <div>
                                        <p className="text-xs font-bold text-gray-400 uppercase tracking-wider mb-2">🍋 Ingredients</p>
                                        <div className="space-y-2">
                                            {customizeDrink.cocktail.ingredients.map(ing => {
                                                const key = `i_${ing.id}`;
                                                const defaultQty = ing.pivot?.quantity || 0;
                                                const mod = ingredientMods[key] || 0;
                                                const currentQty = defaultQty + mod;
                                                return (
                                                    <div key={key} className={`flex items-center justify-between p-3 rounded-xl transition-all ${
                                                        mod !== 0
                                                            ? mod > 0
                                                                ? 'bg-sky-70 dark:bg-sky-700/10 ring-1 ring-sky-700 dark:ring-sky-700/10'
                                                                : 'bg-red-50 dark:bg-red-500/10 ring-1 ring-red-200 dark:ring-red-500/20'
                                                            : 'bg-gray-50 dark:bg-white/[0.03]'
                                                    }`}>
                                                        <div className="flex-1 min-w-0 mr-3">
                                                            <p className="text-sm font-semibold text-gray-900 dark:text-white">{ing.name}</p>
                                                            <div className="flex items-center gap-2">
                                                                <span className="text-[10px] text-gray-400">Default: {defaultQty} {ing.quantity_unit?.quantity || ''}</span>
                                                                {mod > 0 && (
                                                                    <span className="text-[10px] font-bold text-sky-700">+{ing.price * mod} Ft</span>
                                                                )}
                                                            </div>
                                                        </div>
                                                        <div className="flex items-center gap-2">
                                                            <button
                                                                onClick={() => updateMod(key, -1, defaultQty)}
                                                                disabled={currentQty <= 0}
                                                                className="w-8 h-8 rounded-lg bg-white dark:bg-[#1a1c23] border border-gray-200 dark:border-white/10 flex items-center justify-center text-gray-600 dark:text-gray-300 hover:bg-red-50 hover:text-red-500 hover:border-red-200 transition-all disabled:opacity-30 disabled:cursor-not-allowed"
                                                            >
                                                                <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M20 12H4" /></svg>
                                                            </button>
                                                            <span className={`w-6 text-center text-sm font-bold ${mod > 0 ? 'text-sky-700' : mod < 0 ? 'text-red-500' : 'text-gray-900 dark:text-white'}`}>
                                                                {currentQty}
                                                            </span>
                                                            <button
                                                                onClick={() => updateMod(key, 1, defaultQty)}
                                                                className="w-8 h-8 rounded-lg bg-gradient-to-br from-sky-700 to-sky-950 flex items-center justify-center text-white shadow-sm hover:shadow-md hover:scale-105 transition-all"
                                                            >
                                                                <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" /></svg>
                                                            </button>
                                                        </div>
                                                    </div>
                                                );
                                            })}
                                        </div>
                                    </div>
                                )}
                            </div>

                            {/* Modal Footer */}
                            <div className="p-5 border-t border-gray-100 dark:border-white/5">
                                <div className="flex items-center justify-between mb-3">
                                    <span className="text-sm text-gray-500 dark:text-gray-400">Total per cocktail</span>
                                    <div className="text-right">
                                        <span className="text-lg font-black text-gray-900 dark:text-white">{(customizeDrink.price + getExtrasPrice()).toLocaleString()} Ft</span>
                                        {getExtrasPrice() > 0 && (
                                            <span className="ml-1 text-xs font-bold text-sky-700">(+{getExtrasPrice()} Ft)</span>
                                        )}
                                    </div>
                                </div>
                                <button
                                    onClick={confirmCustomize}
                                    className="w-full py-3 rounded-xl bg-gradient-to-r from-sky-700 to-sky-950 text-white font-bold text-sm shadow-lg shadow-sky-700/10 hover:from-sky-700 hover:to-sky-950 transition-all hover:-translate-y-0.5 active:translate-y-0"
                                >
                                    Add to Cart
                                </button>
                            </div>
                        </div>
                    </div>
                )}
            </div>
        </Layout>
    );
}
