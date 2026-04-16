import React, { useEffect, useState } from 'react';
import Layout from '../components/Layout';
import api from '../api';

export default function Drinks() {
    const [drinks, setDrinks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [selectedDrink, setSelectedDrink] = useState(null);

    useEffect(() => {
        const fetchDrinks = async () => {
            try {
                const response = await api.get('/drinks');
                setDrinks(response.data);
                setLoading(false);
            } catch (err) {
                console.error("Failed to fetch drinks", err);
                setError("Failed to load drinks. Is the API running?");
                setLoading(false);
            }
        };

        fetchDrinks();
    }, []);

    // Group drinks by type, filtering out CostumeCocktail
    const groupedDrinks = drinks.filter(drink => drink.name !== 'CostumeCocktail').reduce((acc, drink) => {
        const type = drink.type || 'Other';
        if (!acc[type]) acc[type] = [];
        acc[type].push(drink);
        return acc;
    }, {});

    // Helper to get image based on type
    const getImageForType = (type) => {
        switch (type?.toLowerCase()) {
            case 'cocktail':
                return 'https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?auto=format&fit=crop&q=80&w=800';
            case 'alcohol':
                return 'https://images.unsplash.com/photo-1569529465841-dfecdab7503b?auto=format&fit=crop&q=80&w=800';
            case 'softdrink':
                return 'https://images.unsplash.com/photo-1622483767028-3f66f32aef97?auto=format&fit=crop&q=80&w=800';
            default:
                return 'https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?auto=format&fit=crop&q=80&w=800';
        }
    };

    return (
        <Layout>
            {/* Modal for Drink Details */}
            {selectedDrink && (
                <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/50 backdrop-blur-sm" onClick={() => setSelectedDrink(null)}>
                    <div
                        className="bg-white dark:bg-[#0c0d12] rounded-3xl overflow-hidden max-w-md w-full shadow-2xl transform transition-all border border-gray-200 dark:border-white/10"
                        onClick={e => e.stopPropagation()}
                    >
                        <div className="relative h-48 w-full bg-gray-200 dark:bg-gray-800">
                            <img
                                src={getImageForType(selectedDrink.type)}
                                alt={selectedDrink.name}
                                className="w-full h-full object-cover"
                            />
                            <button
                                onClick={() => setSelectedDrink(null)}
                                className="absolute top-4 right-4 bg-black/40 hover:bg-black/60 text-white rounded-full p-2 backdrop-blur-md transition-colors"
                            >
                                <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                                </svg>
                            </button>
                        </div>
                        <div className="p-6">
                            <div className="flex justify-between items-start mb-4">
                                <div>
                                    <h3 className="text-2xl font-bold text-gray-900 dark:text-white">{selectedDrink.name}</h3>
                                    <span className="inline-block mt-1 px-3 py-1 text-xs font-semibold bg-sky-100 text-sky-700 dark:bg-sky-700/10 dark:text-sky-700 rounded-full">
                                        {selectedDrink.type}
                                    </span>
                                </div>
                                <div className="text-xl font-black text-sky-700">{selectedDrink.price} Ft</div>
                            </div>

                            <div className="space-y-3 mt-6">

                                {selectedDrink.type?.toLowerCase() === 'cocktail' && selectedDrink.cocktail && (
                                    <div className="mt-4 pt-4 border-t border-gray-100 dark:border-white/5">
                                        <h4 className="text-lg font-bold text-gray-900 dark:text-white mb-3">Ingredients</h4>
                                        <ul className="space-y-2">
                                            {selectedDrink.cocktail.drinks?.map((d, i) => (
                                                <li key={`drink-${i}`} className="flex justify-between text-sm py-1">
                                                    <span className="text-gray-700 dark:text-gray-300">{d.name}</span>
                                                    <span className="text-gray-500 font-medium">{d.pivot?.quantity} {d.mixing_quantity?.quantity || d.mixingQuantity?.quantity || d.quantity?.quantity || ''}</span>
                                                </li>
                                            ))}
                                            {selectedDrink.cocktail.ingredients?.map((ing, i) => (
                                                <li key={`ing-${i}`} className="flex justify-between text-sm py-1">
                                                    <span className="text-gray-700 dark:text-gray-300">{ing.name}</span>
                                                    <span className="text-gray-500 font-medium">{ing.pivot?.quantity} {ing.quantity_unit?.quantity || ing.quantityUnit?.quantity || ''}</span>
                                                </li>
                                            ))}
                                        </ul>
                                        {(!selectedDrink.cocktail.drinks?.length && !selectedDrink.cocktail.ingredients?.length) && (
                                            <p className="text-sm text-gray-500 italic">No ingredients listed.</p>
                                        )}
                                    </div>
                                )}

                                {selectedDrink.type?.toLowerCase() !== 'alcohol' && selectedDrink.type?.toLowerCase() !== 'softdrink' && selectedDrink.mixing_price > 0 && (
                                    <div className="flex justify-between py-2 border-b border-gray-100 dark:border-white/5">
                                        <span className="text-gray-500 dark:text-gray-400">Mixing Price</span>
                                        <span className="font-medium text-gray-900 dark:text-white">
                                            {selectedDrink.mixing_price} Ft
                                        </span>
                                    </div>
                                )}

                                {selectedDrink.type?.toLowerCase() !== 'alcohol' && selectedDrink.type?.toLowerCase() !== 'softdrink' && selectedDrink.mixingQuantity && (
                                    <div className="flex justify-between py-2 border-b border-gray-100 dark:border-white/5">
                                        <span className="text-gray-500 dark:text-gray-400">Mixing Quantity</span>
                                        <span className="font-medium text-gray-900 dark:text-white">
                                            {selectedDrink.mixingQuantity.quantity}
                                        </span>
                                    </div>
                                )}


                            </div>
                        </div>
                    </div>
                </div>
            )}

            <div className="px-4 py-8 sm:px-6 lg:px-8 max-w-7xl mx-auto">
                <div className="mb-10 text-center sm:text-left">
                    <h1 className="text-4xl md:text-5xl font-black text-gray-900 dark:text-white tracking-tight">Our <span className="text-transparent bg-clip-text bg-gradient-to-r from-sky-700 to-sky-950">Menu</span></h1>
                    <p className="mt-3 text-lg text-gray-600 dark:text-gray-400 max-w-2xl">Explore our hand-crafted selection of fine beverages.</p>
                </div>

                {loading && (
                    <div className="flex justify-center items-center h-64">
                        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-sky-700"></div>
                    </div>
                )}

                {error && (
                    <div className="bg-red-50 dark:bg-red-500/10 border-l-4 border-red-500 p-4 rounded-xl shadow-sm">
                        <div className="flex">
                            <div className="flex-shrink-0">
                                <svg className="h-5 w-5 text-red-400" viewBox="0 0 20 20" fill="currentColor">
                                    <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
                                </svg>
                            </div>
                            <div className="ml-3">
                                <p className="text-sm font-medium text-red-800 dark:text-red-300">{error}</p>
                            </div>
                        </div>
                    </div>
                )}

                {!loading && !error && drinks.length === 0 && (
                    <div className="text-center py-16 bg-white dark:bg-[#0c0d12] rounded-3xl shadow-sm border border-gray-100 dark:border-white/5">
                        <svg className="mx-auto h-16 w-16 text-gray-300 dark:text-gray-700" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4" />
                        </svg>
                        <h3 className="mt-4 text-lg font-medium text-gray-900 dark:text-white">No drinks found</h3>
                        <p className="mt-2 text-gray-500 dark:text-gray-400">Check back later for our updated menu.</p>
                    </div>
                )}

                {!loading && !error && drinks.length > 0 && (
                    <div className="space-y-12">
                        {Object.entries(groupedDrinks).map(([type, items]) => (
                            <div key={type} className="scroll-mt-6">
                                <div className="flex items-center space-x-4 mb-6">
                                    <h2 className="text-2xl font-bold text-gray-900 dark:text-white capitalize">{type}s</h2>
                                    <div className="flex-1 h-px bg-gray-200 dark:bg-gray-800"></div>
                                </div>
                                <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
                                    {items.map((drink) => (
                                        <div
                                            key={drink.id || drink.name}
                                            onClick={() => setSelectedDrink(drink)}
                                            className="group cursor-pointer bg-white dark:bg-[#0c0d12] rounded-2xl overflow-hidden shadow-sm hover:shadow-xl transition-all duration-300 border border-gray-100 dark:border-white/5 transform hover:-translate-y-1 flex flex-col"
                                        >
                                            <div className="relative h-48 overflow-hidden bg-gray-100 dark:bg-gray-800">
                                                <img
                                                    src={getImageForType(drink.type)}
                                                    alt={drink.name}
                                                    className="w-full h-full object-cover group-hover:scale-110 transition-transform duration-500"
                                                />
                                            </div>
                                            <div className="p-5 flex-1 flex flex-col justify-between">
                                                <h3 className="text-lg font-bold text-gray-900 dark:text-white truncate group-hover:text-sky-700 transition-colors">
                                                    {drink.name}
                                                </h3>
                                                <div className="mt-4 flex items-center justify-between">
                                                    <span className="text-lg font-black text-sky-700">{drink.price} Ft</span>
                                                    <span className="text-xs font-semibold text-gray-500 dark:text-gray-400 uppercase tracking-wider bg-gray-100 dark:bg-gray-800 px-2 py-1 rounded">
                                                        Details
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </Layout>
    );
}
