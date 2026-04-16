import React, { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import Layout from '../components/Layout';
import { useAuth } from '../context/AuthContext';
import api from '../api';

export default function CocktailLab() {
    const { isLoggedIn } = useAuth();
    const navigate = useNavigate();

    // Data
    const [myRecipes, setMyRecipes] = useState([]);
    const [allDrinks, setAllDrinks] = useState([]);
    const [allIngredients, setAllIngredients] = useState([]);
    const [loading, setLoading] = useState(true);

    // Creator state
    const [showCreator, setShowCreator] = useState(false);
    const [editingId, setEditingId] = useState(null);
    const [cocktailName, setCocktailName] = useState('');
    const [selectedDrinks, setSelectedDrinks] = useState({}); // { drinkId: quantity }
    const [selectedIngredients, setSelectedIngredients] = useState({}); // { ingredientId: quantity }
    const [search, setSearch] = useState('');
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState('');
    const [deleteConfirm, setDeleteConfirm] = useState(null);
    // Inline quantity editing state: { type: 'drink'|'ingredient', id: number }
    const [editingQty, setEditingQty] = useState(null);
    const editInputRef = useRef(null);

    useEffect(() => {
        if (!isLoggedIn) { navigate('/login'); return; }
        fetchAll();
    }, [isLoggedIn]);

    const fetchAll = async () => {
        setLoading(true);
        try {
            const [recipesRes, drinksRes, ingredientsRes] = await Promise.all([
                api.get('/my-custom-cocktails'),
                api.get('/drinks'),
                api.get('/ingredients'),
            ]);
            setMyRecipes(recipesRes.data);
            // Only non-cocktail drinks for mixing
            setAllDrinks(drinksRes.data.filter(d => d.type !== 'Cocktail'));
            setAllIngredients(ingredientsRes.data);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    // --- Creator helpers ---
    const resetCreator = () => {
        setCocktailName('');
        setSelectedDrinks({});
        setSelectedIngredients({});
        setSearch('');
        setEditingId(null);
        setError('');
    };

    const openCreator = () => {
        resetCreator();
        setShowCreator(true);
    };

    const openEditor = (recipe) => {
        setCocktailName(recipe.cocktail_name);
        const drinks = {};
        recipe.drinks?.forEach(d => { drinks[d.id] = d.pivot.quantity; });
        setSelectedDrinks(drinks);
        const ings = {};
        recipe.ingredients?.forEach(i => { ings[i.id] = i.pivot.quantity; });
        setSelectedIngredients(ings);
        setEditingId(recipe.id);
        setShowCreator(true);
    };

    const updateDrinkQty = (drinkId, delta) => {
        setSelectedDrinks(prev => {
            const next = (prev[drinkId] || 0) + delta;
            const copy = { ...prev };
            if (next <= 0) delete copy[drinkId];
            else copy[drinkId] = next;
            return copy;
        });
    };

    const setDrinkQty = (drinkId, value) => {
        const qty = Math.max(0, parseInt(value) || 0);
        setSelectedDrinks(prev => {
            const copy = { ...prev };
            if (qty <= 0) delete copy[drinkId];
            else copy[drinkId] = qty;
            return copy;
        });
    };

    const updateIngredientQty = (ingredientId, delta) => {
        setSelectedIngredients(prev => {
            const next = (prev[ingredientId] || 0) + delta;
            const copy = { ...prev };
            if (next <= 0) delete copy[ingredientId];
            else copy[ingredientId] = next;
            return copy;
        });
    };

    const setIngredientQty = (ingredientId, value) => {
        const qty = Math.max(0, parseInt(value) || 0);
        setSelectedIngredients(prev => {
            const copy = { ...prev };
            if (qty <= 0) delete copy[ingredientId];
            else copy[ingredientId] = qty;
            return copy;
        });
    };

    const startEditQty = (type, id) => {
        setEditingQty({ type, id });
        setTimeout(() => editInputRef.current?.select(), 0);
    };

    const commitEditQty = (type, id, value) => {
        if (type === 'drink') setDrinkQty(id, value);
        else setIngredientQty(id, value);
        setEditingQty(null);
    };

    const calcPrice = () => {
        let total = 0;
        for (const [id, qty] of Object.entries(selectedDrinks)) {
            const d = allDrinks.find(x => x.id === parseInt(id));
            if (d) total += d.mixing_price * qty;
        }
        for (const [id, qty] of Object.entries(selectedIngredients)) {
            const i = allIngredients.find(x => x.id === parseInt(id));
            if (i) total += i.price * qty;
        }
        return total;
    };

    const handleSave = async () => {
        if (!cocktailName.trim()) { setError('Give your cocktail a name!'); return; }
        if (Object.keys(selectedDrinks).length === 0 && Object.keys(selectedIngredients).length === 0) {
            setError('Add at least one drink or ingredient!');
            return;
        }

        setSaving(true);
        setError('');

        const payload = {
            cocktail_name: cocktailName.trim(),
            drinks: Object.entries(selectedDrinks).map(([id, quantity]) => ({ id: parseInt(id), quantity })),
            ingredients: Object.entries(selectedIngredients).map(([id, quantity]) => ({ id: parseInt(id), quantity })),
        };

        try {
            if (editingId) {
                await api.put(`/custom-cocktails/${editingId}`, payload);
            } else {
                await api.post('/custom-cocktails', payload);
            }
            setShowCreator(false);
            resetCreator();
            fetchAll();
        } catch (err) {
            if (err.response?.data?.errors?.cocktail_name) {
                setError('You already have a cocktail with this name!');
            } else if (err.response?.data?.message) {
                setError(err.response.data.message);
            } else {
                setError('Failed to save. Try again.');
            }
        } finally {
            setSaving(false);
        }
    };

    const handleDelete = async (id) => {
        try {
            await api.delete(`/custom-cocktails/${id}`);
            setDeleteConfirm(null);
            fetchAll();
        } catch (err) {
            console.error(err);
        }
    };

    // Filtered & categorized lists
    const searchLower = search.toLowerCase();
    const filteredAlcohols = allDrinks.filter(d =>
        d.type === 'Alcohol' && d.name.toLowerCase().includes(searchLower)
    );
    const filteredSoftdrinks = allDrinks.filter(d =>
        d.type === 'Softdrink' && d.name.toLowerCase().includes(searchLower)
    );
    const filteredIngredients = allIngredients.filter(i =>
        i.name.toLowerCase().includes(searchLower)
    );

    if (loading) {
        return (
            <Layout>
                <div className="flex items-center justify-center min-h-[60vh]">
                    <div className="animate-spin rounded-full h-10 w-10 border-2 border-sky-700 border-t-transparent" />
                </div>
            </Layout>
        );
    }

    return (
        <Layout>
            <div className="max-w-2xl mx-auto p-4 space-y-6">
                {/* Header */}
                <div className="flex items-center justify-between">
                    <div className="flex items-center gap-3">
                        <button
                            onClick={() => navigate('/profile')}
                            className="w-10 h-10 rounded-xl bg-white dark:bg-white/5 border border-gray-200 dark:border-white/10 flex items-center justify-center text-gray-500 dark:text-gray-400 hover:bg-sky-70 dark:hover:bg-sky-700/10 hover:text-sky-600 hover:border-sky-700 dark:hover:border-sky-700/10 transition-all shadow-sm"
                            title="Back to Profile"
                        >
                            <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" />
                            </svg>
                        </button>
                        <div>
                            <h1 className="text-2xl font-black text-gray-900 dark:text-white">🧪 Cocktail Lab</h1>
                            <p className="text-sm text-gray-400 mt-0.5">Create your own cocktail recipes</p>
                        </div>
                    </div>
                    {!showCreator && (
                        <button
                            onClick={openCreator}
                            className="px-4 py-2.5 rounded-xl bg-gradient-to-r from-sky-700 to-sky-950 text-white font-bold text-sm shadow-lg shadow-sky-700/10 hover:from-sky-700 hover:to-sky-950 transition-all hover:-translate-y-0.5 active:translate-y-0"
                        >
                            + New Recipe
                        </button>
                    )}
                </div>

                {/* Creator / Editor */}
                {showCreator && (
                    <div className="bg-white dark:bg-[#0c0d12] rounded-2xl shadow-xl border border-gray-100 dark:border-white/5 overflow-hidden">
                        <div className="p-5 border-b border-gray-100 dark:border-white/5 flex items-center justify-between">
                            <h2 className="text-lg font-black text-gray-900 dark:text-white">
                                {editingId ? '✏️ Edit Recipe' : '🧪 New Recipe'}
                            </h2>
                            <button
                                onClick={() => { setShowCreator(false); resetCreator(); }}
                                className="w-8 h-8 rounded-lg bg-gray-100 dark:bg-white/5 flex items-center justify-center text-gray-400 hover:text-gray-600 transition-colors"
                            >✕</button>
                        </div>

                        <div className="p-5 space-y-5">
                            {/* Name */}
                            <div>
                                <label className="block text-xs font-bold text-gray-400 uppercase tracking-wider mb-2">Cocktail Name</label>
                                <input
                                    type="text"
                                    value={cocktailName}
                                    onChange={e => setCocktailName(e.target.value)}
                                    placeholder="e.g. Sunset Paradise"
                                    className="w-full px-4 py-3 rounded-xl bg-gray-50 dark:bg-white/[0.03] border border-gray-200 dark:border-white/10 text-gray-900 dark:text-white text-sm font-medium placeholder-gray-400 focus:ring-2 focus:ring-sky-700 focus:border-transparent outline-none transition-all"
                                />
                            </div>

                            {/* Unified search bar */}
                            <div>
                                <input
                                    type="text"
                                    value={search}
                                    onChange={e => setSearch(e.target.value)}
                                    placeholder="Search all ingredients..."
                                    className="w-full px-3 py-2 rounded-lg bg-gray-50 dark:bg-white/[0.03] border border-gray-200 dark:border-white/10 text-gray-900 dark:text-white text-xs placeholder-gray-400 focus:ring-1 focus:ring-sky-700 outline-none transition-all mb-2"
                                />
                            </div>

                            {/* Spirits / Alcohols section */}
                            {filteredAlcohols.length > 0 && (
                                <div>
                                    <div className="flex items-center gap-2 mb-2">
                                        <label className="text-xs font-bold text-sky-700 uppercase tracking-wider">🥃 Spirits / Alcohols</label>
                                        <div className="flex-1 h-px bg-sky-600 dark:bg-sky-700/10"></div>
                                    </div>
                                    <div className="max-h-48 overflow-y-auto space-y-1.5 rounded-xl">
                                        {filteredAlcohols.map(drink => {
                                            const qty = selectedDrinks[drink.id] || 0;
                                            const isEditing = editingQty?.type === 'drink' && editingQty?.id === drink.id;
                                            return (
                                                <div key={drink.id} className={`flex items-center justify-between p-2.5 rounded-xl transition-all ${
                                                    qty > 0
                                                        ? 'bg-sky-70 dark:bg-sky-700/10 ring-1 ring-sky-700 dark:ring-sky-700/10'
                                                        : 'bg-gray-50 dark:bg-white/[0.03] hover:bg-gray-100 dark:hover:bg-white/[0.05]'
                                                }`}>
                                                    <div className="flex-1 min-w-0 mr-2">
                                                        <p className="text-xs font-semibold text-gray-900 dark:text-white truncate">{drink.name}</p>
                                                        <p className="text-[10px] text-gray-400">{drink.mixing_price} Ft / {drink.mixing_quantity?.quantity || 'unit'}</p>
                                                    </div>
                                                    <div className="flex items-center gap-1.5">
                                                        {qty > 0 && (
                                                            <button onClick={() => updateDrinkQty(drink.id, -1)}
                                                                className="w-7 h-7 rounded-lg bg-white dark:bg-[#1a1c23] border border-gray-200 dark:border-white/10 flex items-center justify-center text-gray-500 hover:bg-red-50 hover:text-red-500 transition-all text-xs">−</button>
                                                        )}
                                                        {qty > 0 && (
                                                            isEditing ? (
                                                                <input
                                                                    ref={editInputRef}
                                                                    type="number"
                                                                    min="0"
                                                                    defaultValue={qty}
                                                                    className="w-10 text-center text-xs font-bold text-gray-900 dark:text-white bg-white dark:bg-[#1a1c23] border border-sky-700 dark:border-sky-700/10 rounded-lg outline-none focus:ring-1 focus:ring-sky-700 py-0.5 [appearance:textfield] [&::-webkit-outer-spin-button]:appearance-none [&::-webkit-inner-spin-button]:appearance-none"
                                                                    onBlur={e => commitEditQty('drink', drink.id, e.target.value)}
                                                                    onKeyDown={e => {
                                                                        if (e.key === 'Enter') commitEditQty('drink', drink.id, e.target.value);
                                                                        if (e.key === 'Escape') setEditingQty(null);
                                                                    }}
                                                                />
                                                            ) : (
                                                                <span
                                                                    onClick={() => startEditQty('drink', drink.id)}
                                                                    className="w-7 text-center text-xs font-bold text-gray-900 dark:text-white cursor-pointer hover:bg-sky-600 dark:hover:bg-sky-700/10 rounded-lg py-0.5 transition-colors"
                                                                    title="Click to edit"
                                                                >{qty}</span>
                                                            )
                                                        )}
                                                        <button onClick={() => updateDrinkQty(drink.id, 1)}
                                                            className="w-7 h-7 rounded-lg bg-gradient-to-br from-sky-700 to-sky-950 flex items-center justify-center text-white text-xs shadow-sm hover:scale-105 transition-all">+</button>
                                                    </div>
                                                </div>
                                            );
                                        })}
                                    </div>
                                </div>
                            )}

                            {/* Mixers / Soft Drinks section */}
                            {filteredSoftdrinks.length > 0 && (
                                <div>
                                    <div className="flex items-center gap-2 mb-2">
                                        <label className="text-xs font-bold text-sky-700 uppercase tracking-wider">🥤 Mixers / Soft Drinks</label>
                                        <div className="flex-1 h-px bg-sky-600 dark:bg-sky-700/10"></div>
                                    </div>
                                    <div className="max-h-48 overflow-y-auto space-y-1.5 rounded-xl">
                                        {filteredSoftdrinks.map(drink => {
                                            const qty = selectedDrinks[drink.id] || 0;
                                            const isEditing = editingQty?.type === 'drink' && editingQty?.id === drink.id;
                                            return (
                                                <div key={drink.id} className={`flex items-center justify-between p-2.5 rounded-xl transition-all ${
                                                    qty > 0
                                                        ? 'bg-sky-70 dark:bg-sky-700/10 ring-1 ring-sky-700 dark:ring-sky-700/10'
                                                        : 'bg-gray-50 dark:bg-white/[0.03] hover:bg-gray-100 dark:hover:bg-white/[0.05]'
                                                }`}>
                                                    <div className="flex-1 min-w-0 mr-2">
                                                        <p className="text-xs font-semibold text-gray-900 dark:text-white truncate">{drink.name}</p>
                                                        <p className="text-[10px] text-gray-400">{drink.mixing_price} Ft / {drink.mixing_quantity?.quantity || 'unit'}</p>
                                                    </div>
                                                    <div className="flex items-center gap-1.5">
                                                        {qty > 0 && (
                                                            <button onClick={() => updateDrinkQty(drink.id, -1)}
                                                                className="w-7 h-7 rounded-lg bg-white dark:bg-[#1a1c23] border border-gray-200 dark:border-white/10 flex items-center justify-center text-gray-500 hover:bg-red-50 hover:text-red-500 transition-all text-xs">−</button>
                                                        )}
                                                        {qty > 0 && (
                                                            isEditing ? (
                                                                <input
                                                                    ref={editInputRef}
                                                                    type="number"
                                                                    min="0"
                                                                    defaultValue={qty}
                                                                    className="w-10 text-center text-xs font-bold text-gray-900 dark:text-white bg-white dark:bg-[#1a1c23] border border-sky-700 dark:border-sky-700/10 rounded-lg outline-none focus:ring-1 focus:ring-sky-700 py-0.5 [appearance:textfield] [&::-webkit-outer-spin-button]:appearance-none [&::-webkit-inner-spin-button]:appearance-none"
                                                                    onBlur={e => commitEditQty('drink', drink.id, e.target.value)}
                                                                    onKeyDown={e => {
                                                                        if (e.key === 'Enter') commitEditQty('drink', drink.id, e.target.value);
                                                                        if (e.key === 'Escape') setEditingQty(null);
                                                                    }}
                                                                />
                                                            ) : (
                                                                <span
                                                                    onClick={() => startEditQty('drink', drink.id)}
                                                                    className="w-7 text-center text-xs font-bold text-gray-900 dark:text-white cursor-pointer hover:bg-blue-100 dark:hover:bg-blue-500/20 rounded-lg py-0.5 transition-colors"
                                                                    title="Click to edit"
                                                                >{qty}</span>
                                                            )
                                                        )}
                                                        <button onClick={() => updateDrinkQty(drink.id, 1)}
                                                            className="w-7 h-7 rounded-lg bg-gradient-to-br from-sky-700 to-sky-950 flex items-center justify-center text-white text-xs shadow-sm hover:scale-105 transition-all">+</button>
                                                    </div>
                                                </div>
                                            );
                                        })}
                                    </div>
                                </div>
                            )}

                            {/* Additives / Garnishments section */}
                            {filteredIngredients.length > 0 && (
                            <div>
                                <div className="flex items-center gap-2 mb-2">
                                    <label className="text-xs font-bold text-sky-700 uppercase tracking-wider">🍋 Additives / Garnishments</label>
                                    <div className="flex-1 h-px bg-sky-700 dark:bg-sky-700/10"></div>
                                </div>
                                <div className="max-h-48 overflow-y-auto space-y-1.5 rounded-xl">
                                    {filteredIngredients.map(ing => {
                                        const qty = selectedIngredients[ing.id] || 0;
                                        const isEditing = editingQty?.type === 'ingredient' && editingQty?.id === ing.id;
                                        return (
                                            <div key={ing.id} className={`flex items-center justify-between p-2.5 rounded-xl transition-all ${
                                                qty > 0
                                                    ? 'bg-sky-70 dark:bg-sky-700/10 ring-1 ring-sky-700 dark:ring-sky-700/20'
                                                    : 'bg-gray-50 dark:bg-white/[0.03] hover:bg-gray-100 dark:hover:bg-white/[0.05]'
                                            }`}>
                                                <div className="flex-1 min-w-0 mr-2">
                                                    <p className="text-xs font-semibold text-gray-900 dark:text-white truncate">{ing.name}</p>
                                                    <p className="text-[10px] text-gray-400">{ing.price} Ft / {ing.quantity_unit?.quantity || 'unit'}</p>
                                                </div>
                                                <div className="flex items-center gap-1.5">
                                                    {qty > 0 && (
                                                        <button onClick={() => updateIngredientQty(ing.id, -1)}
                                                            className="w-7 h-7 rounded-lg bg-white dark:bg-[#1a1c23] border border-gray-200 dark:border-white/10 flex items-center justify-center text-gray-500 hover:bg-red-50 hover:text-red-500 transition-all text-xs">−</button>
                                                    )}
                                                    {qty > 0 && (
                                                        isEditing ? (
                                                            <input
                                                                ref={editInputRef}
                                                                type="number"
                                                                min="0"
                                                                defaultValue={qty}
                                                                className="w-10 text-center text-xs font-bold text-gray-900 dark:text-white bg-white dark:bg-[#1a1c23] border border-sky-600 dark:border-sky-700/10 rounded-lg outline-none focus:ring-1 focus:ring-sky-700 py-0.5 [appearance:textfield] [&::-webkit-outer-spin-button]:appearance-none [&::-webkit-inner-spin-button]:appearance-none"
                                                                onBlur={e => commitEditQty('ingredient', ing.id, e.target.value)}
                                                                onKeyDown={e => {
                                                                    if (e.key === 'Enter') commitEditQty('ingredient', ing.id, e.target.value);
                                                                    if (e.key === 'Escape') setEditingQty(null);
                                                                }}
                                                            />
                                                        ) : (
                                                            <span
                                                                onClick={() => startEditQty('ingredient', ing.id)}
                                                                className="w-7 text-center text-xs font-bold text-gray-900 dark:text-white cursor-pointer hover:bg-sky-600 dark:hover:bg-sky-700/10 rounded-lg py-0.5 transition-colors"
                                                                title="Click to edit"
                                                            >{qty}</span>
                                                        )
                                                    )}
                                                    <button onClick={() => updateIngredientQty(ing.id, 1)}
                                                        className="w-7 h-7 rounded-lg bg-gradient-to-br from-sky-700 to-sky-950 flex items-center justify-center text-white text-xs shadow-sm hover:scale-105 transition-all">+</button>
                                                </div>
                                            </div>
                                        );
                                    })}
                                </div>
                            </div>
                            )}
                            {/* Error */}
                            {error && (
                                <p className="text-xs text-red-500 font-medium bg-red-50 dark:bg-red-500/10 px-3 py-2 rounded-lg">{error}</p>
                            )}
                        </div>

                        {/* Footer with price */}
                        <div className="p-5 border-t border-gray-100 dark:border-white/5">
                            <div className="flex items-center justify-between mb-3">
                                <span className="text-sm text-gray-500 dark:text-gray-400">Estimated price</span>
                                <span className="text-xl font-black text-gray-900 dark:text-white">{calcPrice().toLocaleString()} Ft</span>
                            </div>
                            <button
                                onClick={handleSave}
                                disabled={saving}
                                className="w-full py-3 rounded-xl bg-gradient-to-r from-sky-700 to-sky-950 text-white font-bold text-sm shadow-lg shadow-sky-700/10 hover:from-sky-700 hover:to-sky-950 transition-all hover:-translate-y-0.5 active:translate-y-0 disabled:opacity-50 disabled:cursor-not-allowed"
                            >
                                {saving ? 'Saving...' : editingId ? 'Update Recipe' : 'Save Recipe'}
                            </button>
                        </div>
                    </div>
                )}

                {/* My Recipes */}
                {!showCreator && (
                    <div className="space-y-3">
                        {myRecipes.length === 0 ? (
                            <div className="bg-white dark:bg-[#0c0d12] rounded-2xl shadow-xl border border-gray-100 dark:border-white/5 p-8 text-center">
                                <div className="inline-flex items-center justify-center w-14 h-14 rounded-full bg-sky-600 dark:bg-sky-700/10 mb-4">
                                    <span className="text-2xl">🧪</span>
                                </div>
                                <h3 className="text-base font-bold text-gray-900 dark:text-white mb-1">No recipes yet</h3>
                                <p className="text-xs text-gray-400 mb-4">Create your first custom cocktail!</p>
                                <button
                                    onClick={openCreator}
                                    className="px-5 py-2.5 rounded-xl bg-gradient-to-r from-sky-700 to-sky-950 text-white font-bold text-sm shadow-lg shadow-sky-700/10 hover:from-sky-700 hover:to-sky-950 transition-all"
                                >
                                    + New Recipe
                                </button>
                            </div>
                        ) : (
                            myRecipes.map(recipe => (
                                <div key={recipe.id} className="bg-white dark:bg-[#0c0d12] rounded-2xl shadow-xl border border-gray-100 dark:border-white/5 p-4">
                                    <div className="flex items-start justify-between mb-3">
                                        <div>
                                            <h3 className="text-base font-black text-gray-900 dark:text-white">🍹 {recipe.cocktail_name}</h3>
                                            <p className="text-sm font-bold text-sky-700">{recipe.price?.toLocaleString()} Ft</p>
                                        </div>
                                        <div className="flex gap-2">
                                            <button
                                                onClick={() => openEditor(recipe)}
                                                className="w-8 h-8 rounded-lg bg-sky-600 dark:bg-sky-700/10 flex items-center justify-center text-sky-700 hover:bg-sky-700 dark:hover:bg-sky-700/10 transition-all text-xs"
                                            >✏️</button>
                                            {deleteConfirm === recipe.id ? (
                                                <div className="flex gap-1">
                                                    <button onClick={() => handleDelete(recipe.id)}
                                                        className="px-2 h-8 rounded-lg bg-red-500 text-white text-xs font-bold hover:bg-red-600 transition-all">Yes</button>
                                                    <button onClick={() => setDeleteConfirm(null)}
                                                        className="px-2 h-8 rounded-lg bg-gray-100 dark:bg-white/5 text-gray-500 text-xs font-bold hover:bg-gray-200 transition-all">No</button>
                                                </div>
                                            ) : (
                                                <button
                                                    onClick={() => setDeleteConfirm(recipe.id)}
                                                    className="w-8 h-8 rounded-lg bg-red-100 dark:bg-red-500/10 flex items-center justify-center text-red-500 hover:bg-red-200 dark:hover:bg-red-500/20 transition-all text-xs"
                                                >🗑️</button>
                                            )}
                                        </div>
                                    </div>
                                    {/* Recipe details */}
                                    <div className="space-y-1.5">
                                        {recipe.drinks?.map(d => (
                                            <div key={`d-${d.id}`} className="flex items-center justify-between px-3 py-1.5 rounded-lg bg-sky-70 dark:bg-sky-700/10">
                                                <span className="text-xs font-medium text-gray-700 dark:text-gray-300">🥃 {d.name}</span>
                                                <span className="text-[10px] text-gray-400">×{d.pivot.quantity} {d.mixing_quantity?.quantity || ''}</span>
                                            </div>
                                        ))}
                                        {recipe.ingredients?.map(i => (
                                            <div key={`i-${i.id}`} className="flex items-center justify-between px-3 py-1.5 rounded-lg bg-sky-70 dark:bg-sky-700/10">
                                                <span className="text-xs font-medium text-gray-700 dark:text-gray-300">🍋 {i.name}</span>
                                                <span className="text-[10px] text-gray-400">×{i.pivot.quantity} {i.quantity_unit?.quantity || ''}</span>
                                            </div>
                                        ))}
                                    </div>
                                </div>
                            ))
                        )}
                    </div>
                )}
            </div>
        </Layout>
    );
}
