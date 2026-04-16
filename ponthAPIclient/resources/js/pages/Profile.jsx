import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Layout from '../components/Layout';
import { useAuth } from '../context/AuthContext';
import api from '../api';

export default function Profile() {
    const { user, isLoggedIn } = useAuth();
    const navigate = useNavigate();

    const [openBill, setOpenBill] = useState(null);
    const [paidBills, setPaidBills] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [payingWith, setPayingWith] = useState(null);
    const [checkoutLoading, setCheckoutLoading] = useState(false);

    useEffect(() => {
        if (!isLoggedIn) {
            navigate('/login');
            return;
        }
        fetchData();
    }, [isLoggedIn]);

    const fetchData = async () => {
        setLoading(true);
        try {
            const [openRes, historyRes] = await Promise.all([
                api.get('/my-open-bill'),
                api.get('/my-bills'),
            ]);
            setOpenBill(openRes.data.bill);
            setPaidBills(historyRes.data);
        } catch (err) {
            console.error(err);
            setError('Failed to load profile data.');
        } finally {
            setLoading(false);
        }
    };

    const handleCheckout = async (method) => {
        setCheckoutLoading(true);
        setError('');
        try {
            await api.post('/checkout', { paid_with: method });
            setPayingWith(null);
            await fetchData();
        } catch (err) {
            console.error(err);
            setError(err.response?.data?.error || 'Checkout failed.');
        } finally {
            setCheckoutLoading(false);
        }
    };

    if (!isLoggedIn) return null;

    return (
        <Layout>
            <div className="min-h-[80vh] px-4 py-8 max-w-2xl mx-auto">
                {/* Profile Header */}
                <div className="bg-white dark:bg-[#0c0d12] rounded-2xl shadow-xl border border-gray-100 dark:border-white/5 p-6 mb-6">
                    <div className="flex items-center gap-4">
                        <div className="w-16 h-16 rounded-full bg-gradient-to-r from-sky-700 to-sky-950 flex items-center justify-center text-white text-2xl font-bold shadow-lg uppercase">
                            {user?.name ? user.name.charAt(0) : 'U'}
                        </div>
                        <div>
                            <h1 className="text-2xl font-black text-gray-900 dark:text-white">{user?.name || 'User'}</h1>
                            <p className="text-sm text-gray-500 dark:text-gray-400">{user?.email || ''}</p>
                        </div>
                    </div>
                </div>

                {/* Quick Actions */}
                <div className="flex gap-3 mb-6">
                    <button
                        onClick={() => navigate('/cocktail-lab')}
                        className="flex-1 py-3 rounded-xl bg-gradient-to-r from-sky-300 to-sky-500 text-white font-bold text-sm shadow-lg shadow-sky-200/10 hover:from-sky-200 hover:to-sky-400 transition-all hover:-translate-y-0.5 active:translate-y-0"
                    >
                        🧪 My Lab
                    </button>
                    <button
                        onClick={() => navigate('/order-drink?continue=true')}
                        className="flex-1 py-3 rounded-xl bg-gradient-to-r from-sky-300 to-sky-500 text-white font-bold text-sm shadow-lg shadow-sky-200/10 hover:from-sky-200 hover:to-sky-400 transition-all hover:-translate-y-0.5 active:translate-y-0"
                    >
                        + Order More
                    </button>
                </div>
                {error && (
                    <div className="mb-4 bg-red-50 dark:bg-red-500/10 text-red-500 text-sm p-4 rounded-xl text-center font-medium">
                        {error}
                    </div>
                )}

                {loading && (
                    <div className="flex justify-center py-12">
                        <div className="w-8 h-8 border-4 border-sky-700 border-t-transparent rounded-full animate-spin"></div>
                    </div>
                )}

                {/* =================== ACTIVE BILL =================== */}
                {!loading && openBill && (
                    <div className="mb-6">
                        <div className="flex items-center gap-3 mb-4">
                            <div className="w-8 h-8 rounded-lg bg-sky-600 dark:bg-sky-700/10 flex items-center justify-center">
                                <svg className="w-4 h-4 text-sky-700" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
                                </svg>
                            </div>
                            <h2 className="text-xl font-bold text-gray-900 dark:text-white">Active Bill</h2>
                            <span className="ml-auto px-3 py-1 rounded-lg bg-sky-600 dark:bg-sky-700/10 text-sky-700 dark:text-sky-700 text-xs font-bold">
                                📍 Box {openBill.box?.number}
                            </span>
                        </div>

                        <div className="bg-white dark:bg-[#0c0d12] rounded-2xl shadow-xl border border-gray-100 dark:border-white/5 overflow-hidden">
                            {/* Order items */}
                            {openBill.orders && openBill.orders.length > 0 ? (
                                <div className="p-4 space-y-2">
                                    {openBill.orders.map(order => (
                                        <div key={order.id} className="flex items-center justify-between py-2">
                                            <div className="flex items-center gap-3">
                                                <div className="w-8 h-8 rounded-lg bg-sky-700 dark:bg-sky-700/10 flex items-center justify-center text-sky-700 text-xs font-bold">
                                                    {order.quantity}×
                                                </div>
                                                <div>
                                                    <span className="text-sm font-semibold text-gray-900 dark:text-white">
                                                        {order.custom_cocktail_name || order.drink?.name || `Drink #${order.item_id}`}
                                                    </span>
                                                    {order.custom_cocktail_name && (
                                                        <span className="ml-1 px-1.5 py-0.5 rounded text-[9px] font-bold bg-sky-600 dark:bg-sky-700/10 text-sky-600">custom</span>
                                                    )}
                                                    <span className={`ml-2 px-2 py-0.5 rounded text-[10px] font-bold ${
                                                        order.status === 'new' ? 'bg-sky-500 dark:bg-sky-700/10 text-sky-700' : 'bg-sky-600 dark:bg-sky-700/10 text-sky-700'
                                                    }`}>
                                                        {order.status}
                                                    </span>
                                                </div>
                                            </div>
                                            <span className="text-sm font-bold text-gray-700 dark:text-gray-300">
                                                {(order.subtotal || 0).toLocaleString()} Ft
                                            </span>
                                        </div>
                                    ))}
                                </div>
                            ) : (
                                <div className="p-6 text-center text-gray-400 text-sm">No orders yet</div>
                            )}

                            {/* Total + Pay */}
                            <div className="px-4 py-4 bg-gray-50 dark:bg-white/[0.02] border-t border-gray-100 dark:border-white/5">
                                <div className="flex items-center justify-between mb-3">
                                    <span className="text-sm font-bold text-gray-500 dark:text-gray-400">Total</span>
                                    <span className="text-xl font-black text-gray-900 dark:text-white">
                                        {(openBill.totalSum || 0).toLocaleString()} Ft
                                    </span>
                                </div>

                                {payingWith === null ? (
                                    <div className="space-y-2">
                                        <button
                                            onClick={() => navigate('/order-drink?continue=true')}
                                            className="w-full py-3 rounded-xl bg-gradient-to-r from-sky-700 to-sky-950 text-white font-bold text-sm shadow-lg shadow-sky-700/10 hover:from-sky-700 hover:to-sky-950 transition-all hover:-translate-y-0.5 active:translate-y-0"
                                        >
                                            + Order More
                                        </button>
                                        <button
                                            onClick={() => setPayingWith('choosing')}
                                            disabled={!openBill.orders || openBill.orders.length === 0}
                                            className="w-full py-3 rounded-xl bg-gradient-to-r from-sky-700 to-sky-950 text-white font-bold text-sm shadow-lg shadow-sky-700/10 hover:from-sky-700 hover:to-sky-950 transition-all hover:-translate-y-0.5 active:translate-y-0 disabled:opacity-50 disabled:cursor-not-allowed"
                                        >
                                            Pay Bill
                                        </button>
                                    </div>
                                ) : (
                                    <div className="space-y-2">
                                        <p className="text-xs text-gray-500 dark:text-gray-400 text-center mb-2 font-medium">Confirm card payment:</p>
                                        <button
                                            onClick={() => handleCheckout('Card')}
                                            disabled={checkoutLoading}
                                            className="w-full py-3 rounded-xl bg-gradient-to-r from-sky-700 to-sky-950 text-white font-bold text-sm shadow-lg shadow-sky-700/10 hover:from-sky-700 hover:to-sky-950 transition-all hover:-translate-y-0.5 active:translate-y-0 disabled:opacity-50"
                                        >
                                            {checkoutLoading ? 'Processing...' : '💳 Pay with Card'}
                                        </button>
                                        <button
                                            onClick={() => setPayingWith(null)}
                                            className="w-full py-2 text-xs text-gray-400 hover:text-gray-600 transition-colors"
                                        >
                                            Cancel
                                        </button>
                                    </div>
                                )}
                            </div>
                        </div>
                    </div>
                )}

                {/* No active bill message */}
                {!loading && !openBill && (
                    <div className="mb-6 bg-white dark:bg-[#0c0d12] rounded-2xl shadow-xl border border-gray-100 dark:border-white/5 p-6 text-center">
                        <p className="text-gray-400 text-sm">No active bill. Scan a QR code to start ordering!</p>
                    </div>
                )}

                {/* =================== PAID HISTORY =================== */}
                {!loading && (
                    <div className="mb-6">
                        <div className="flex items-center gap-3 mb-4">
                            <div className="w-8 h-8 rounded-lg bg-sky-600 dark:bg-sky-700/10 flex items-center justify-center">
                                <svg className="w-4 h-4 text-sky-700" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
                                </svg>
                            </div>
                            <h2 className="text-xl font-bold text-gray-900 dark:text-white">Order History</h2>
                        </div>

                        {paidBills.length === 0 ? (
                            <div className="bg-white dark:bg-[#0c0d12] rounded-2xl shadow-xl border border-gray-100 dark:border-white/5 p-8 text-center">
                                <p className="text-gray-400 text-sm">No past orders yet</p>
                            </div>
                        ) : (
                            paidBills.map(bill => {
                                const date = new Date(bill.paid_at);
                                const dateStr = date.toLocaleDateString('hu-HU', { year: 'numeric', month: 'long', day: 'numeric' });
                                const timeStr = date.toLocaleTimeString('hu-HU', { hour: '2-digit', minute: '2-digit' });

                                return (
                                    <div key={bill.id} className="bg-white dark:bg-[#0c0d12] rounded-2xl shadow-xl border border-gray-100 dark:border-white/5 overflow-hidden mb-4">
                                        {/* Bill header */}
                                        <div className="flex items-center justify-between p-4 border-b border-gray-100 dark:border-white/5">
                                            <div className="text-sm text-gray-500 dark:text-gray-400">
                                                <span className="font-semibold text-gray-900 dark:text-white">{dateStr}</span>
                                                <span className="ml-2">{timeStr}</span>
                                            </div>
                                            <div className="flex items-center gap-2">
                                                <span className="px-2.5 py-1 rounded-lg bg-gray-100 dark:bg-white/5 text-xs font-bold text-gray-600 dark:text-gray-300">
                                                    📍 Box {bill.box?.number}
                                                </span>
                                                <span className="px-2.5 py-1 rounded-lg bg-sky-600 dark:bg-sky-700/10 text-xs font-bold text-sky-700 dark:text-sky-700">
                                                    💳 {bill.paid_with}
                                                </span>
                                            </div>
                                        </div>

                                        {/* Bill items */}
                                        {bill.orders && bill.orders.length > 0 && (
                                            <div className="p-4 space-y-2">
                                                {bill.orders.map(order => (
                                                    <div key={order.id} className="flex items-center justify-between py-1">
                                                        <div className="flex items-center gap-3">
                                                            <div className="w-7 h-7 rounded-lg bg-sky-600 dark:bg-sky-700/10 flex items-center justify-center text-sky-700 text-[10px] font-bold">
                                                                {order.quantity}×
                                                            </div>
                                                            <span className="text-sm text-gray-700 dark:text-gray-300">
                                                                {order.custom_cocktail_name || order.drink?.name || `Drink #${order.item_id}`}
                                                            </span>
                                                            {order.custom_cocktail_name && (
                                                                <span className="ml-1 px-1.5 py-0.5 rounded text-[9px] font-bold bg-sky-600 dark:bg-sky-700/10 text-sky-700">custom</span>
                                                            )}
                                                        </div>
                                                        <span className="text-sm font-medium text-gray-500 dark:text-gray-400">
                                                            {(order.subtotal || 0).toLocaleString()} Ft
                                                        </span>
                                                    </div>
                                                ))}
                                            </div>
                                        )}

                                        {/* Bill total */}
                                        <div className="flex items-center justify-between px-4 py-3 bg-gray-50 dark:bg-white/[0.02] border-t border-gray-100 dark:border-white/5">
                                            <span className="text-sm font-bold text-gray-500 dark:text-gray-400">Total</span>
                                            <span className="text-lg font-black text-gray-900 dark:text-white">{(bill.total || 0).toLocaleString()} Ft</span>
                                        </div>
                                    </div>
                                );
                            })
                        )}
                    </div>
                )}
            </div>
        </Layout>
    );
}
