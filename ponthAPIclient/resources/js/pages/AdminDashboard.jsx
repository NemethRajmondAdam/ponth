import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Layout from '../components/Layout';
import { useAuth } from '../context/AuthContext';
import api from '../api';

export default function AdminDashboard() {
    const { user, isLoggedIn } = useAuth();
    const navigate = useNavigate();
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [successMsg, setSuccessMsg] = useState('');

    // Delete modal state
    const [deleteModal, setDeleteModal] = useState(null);   // { user, checkData, loading }
    const [deleting, setDeleting] = useState(false);

    // Redirect if not admin
    useEffect(() => {
        if (!isLoggedIn || !user?.is_admin) {
            navigate('/');
        }
    }, [isLoggedIn, user, navigate]);

    // Fetch users
    useEffect(() => {
        if (isLoggedIn && user?.is_admin) {
            fetchUsers();
        }
    }, [isLoggedIn, user]);

    const fetchUsers = async () => {
        setLoading(true);
        setError('');
        try {
            const response = await api.get('/users');
            setUsers(response.data);
        } catch (err) {
            setError('Failed to load users.');
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    const toggleAdmin = async (userId, currentStatus) => {
        setError('');
        setSuccessMsg('');
        try {
            await api.put(`/users/${userId}`, {
                is_admin: !currentStatus
            });
            setSuccessMsg(`User admin status updated.`);
            fetchUsers();
        } catch (err) {
            setError(err.response?.data?.message || 'Failed to update user.');
        }
    };

    // Phase 1: Pre-flight check
    const initiateDelete = async (targetUser) => {
        setError('');
        setSuccessMsg('');
        setDeleteModal({ user: targetUser, checkData: null, loading: true });
        try {
            const res = await api.get(`/users/${targetUser.id}/delete-check`);
            setDeleteModal({ user: targetUser, checkData: res.data, loading: false });
        } catch (err) {
            setDeleteModal(null);
            setError(err.response?.data?.message || 'Failed to check user dependencies.');
        }
    };

    // Phase 2: Execute deletion
    const confirmDelete = async (forceCloseBills = false) => {
        if (!deleteModal?.user) return;
        setDeleting(true);
        try {
            const url = `/users/${deleteModal.user.id}${forceCloseBills ? '?force_close_bills=true' : ''}`;
            await api.delete(url);
            setSuccessMsg(`User "${deleteModal.user.name}" and all associated data deleted.`);
            setDeleteModal(null);
            fetchUsers();
        } catch (err) {
            if (err.response?.status === 409) {
                setError(err.response.data.message);
            } else {
                setError(err.response?.data?.message || 'Failed to delete user.');
            }
            setDeleteModal(null);
        } finally {
            setDeleting(false);
        }
    };

    if (!isLoggedIn || !user?.is_admin) return null;

    return (
        <Layout>
            <div className="min-h-[80vh] px-4 sm:px-6 lg:px-8 py-10 max-w-5xl mx-auto">
                {/* Header */}
                <div className="mb-10">
                    <div className="inline-flex items-center justify-center w-14 h-14 rounded-2xl bg-gradient-to-br from-sky-700 to-sky-950 shadow-lg shadow-sky-700/10 mb-5">
                        <svg className="w-7 h-7 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" />
                        </svg>
                    </div>
                    <h1 className="text-3xl font-black tracking-tight text-gray-900 dark:text-white">
                        User Management
                    </h1>
                    <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">
                        Manage all registered accounts
                    </p>
                </div>

                {/* Messages */}
                {error && (
                    <div className="mb-6 bg-red-50 dark:bg-red-500/10 text-red-500 text-sm p-4 rounded-xl text-center font-medium">
                        {error}
                    </div>
                )}
                {successMsg && (
                    <div className="mb-6 bg-emerald-50 dark:bg-emerald-500/10 text-emerald-500 text-sm p-4 rounded-xl text-center font-medium">
                        {successMsg}
                    </div>
                )}

                {/* User Table */}
                {loading ? (
                    <div className="flex justify-center py-20">
                        <div className="w-10 h-10 border-4 border-sky-700 border-t-transparent rounded-full animate-spin"></div>
                    </div>
                ) : (
                    <div className="bg-white dark:bg-[#0c0d12] rounded-2xl shadow-xl border border-gray-100 dark:border-white/5 overflow-hidden">
                        <div className="overflow-x-auto">
                            <table className="w-full">
                                <thead>
                                    <tr className="border-b border-gray-200 dark:border-white/5">
                                        <th className="px-6 py-4 text-left text-xs font-bold text-gray-400 uppercase tracking-widest">ID</th>
                                        <th className="px-6 py-4 text-left text-xs font-bold text-gray-400 uppercase tracking-widest">Name</th>
                                        <th className="px-6 py-4 text-left text-xs font-bold text-gray-400 uppercase tracking-widest">Email</th>
                                        <th className="px-6 py-4 text-left text-xs font-bold text-gray-400 uppercase tracking-widest">Role</th>
                                        <th className="px-6 py-4 text-right text-xs font-bold text-gray-400 uppercase tracking-widest">Actions</th>
                                    </tr>
                                </thead>
                                <tbody className="divide-y divide-gray-100 dark:divide-white/5">
                                    {users.map((u) => (
                                        <tr key={u.id} className="hover:bg-gray-50 dark:hover:bg-white/[0.02] transition-colors">
                                            <td className="px-6 py-4 text-sm text-gray-500 dark:text-gray-400 font-mono">
                                                #{u.id}
                                            </td>
                                            <td className="px-6 py-4">
                                                <div className="flex items-center gap-3">
                                                    <div className="w-9 h-9 rounded-full bg-gradient-to-r from-sky-700 to-sky-950 flex items-center justify-center text-white text-sm font-bold uppercase flex-shrink-0">
                                                        {u.name?.charAt(0) || '?'}
                                                    </div>
                                                    <span className="text-sm font-semibold text-gray-900 dark:text-white">{u.name}</span>
                                                </div>
                                            </td>
                                            <td className="px-6 py-4 text-sm text-gray-500 dark:text-gray-400">{u.email}</td>
                                            <td className="px-6 py-4">
                                                <span className={`inline-flex items-center px-3 py-1 rounded-full text-xs font-bold ${u.is_admin
                                                    ? 'bg-sky-600 dark:bg-sky-700/10 text-sky-700 dark:text-sky-700'
                                                    : 'bg-gray-100 dark:bg-white/5 text-gray-500 dark:text-gray-400'
                                                    }`}>
                                                    {u.is_admin ? '★ Admin' : 'User'}
                                                </span>
                                            </td>
                                            <td className="px-6 py-4">
                                                <div className="flex items-center justify-end gap-2">
                                                    {/* Don't show actions for the current logged-in admin */}
                                                    {u.id !== user.id && (
                                                        <>
                                                            <button
                                                                onClick={() => toggleAdmin(u.id, u.is_admin)}
                                                                className={`px-3 py-1.5 text-xs font-bold rounded-lg transition-all ${u.is_admin
                                                                    ? 'bg-gray-100 dark:bg-white/5 text-gray-600 dark:text-gray-300 hover:bg-gray-200 dark:hover:bg-white/10'
                                                                    : 'bg-sky-70 dark:bg-sky-700/10 text-sky-700 dark:text-sky-700 hover:bg-sky-700 dark:hover:bg-sky-700/10'
                                                                    }`}
                                                                title={u.is_admin ? 'Demote to User' : 'Promote to Admin'}
                                                            >
                                                                {u.is_admin ? 'Demote' : 'Promote'}
                                                            </button>
                                                            <button
                                                                onClick={() => initiateDelete(u)}
                                                                className="px-3 py-1.5 text-xs font-bold rounded-lg bg-red-50 dark:bg-red-500/10 text-red-500 hover:bg-red-100 dark:hover:bg-red-500/20 transition-all"
                                                                title="Delete User"
                                                            >
                                                                Delete
                                                            </button>
                                                        </>
                                                    )}
                                                    {u.id === user.id && (
                                                        <span className="text-xs text-gray-400 italic">You</span>
                                                    )}
                                                </div>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>

                        {/* Summary footer */}
                        <div className="px-6 py-4 border-t border-gray-200 dark:border-white/5 bg-gray-50 dark:bg-white/[0.02]">
                            <p className="text-xs text-gray-500 dark:text-gray-400">
                                Total: <span className="font-bold text-gray-700 dark:text-gray-200">{users.length}</span> users
                                {' · '}
                                <span className="font-bold text-sky-700">{users.filter(u => u.is_admin).length}</span> admins
                            </p>
                        </div>
                    </div>
                )}
            </div>

            {/* Delete Modal */}
            {deleteModal && (
                <div className="fixed inset-0 z-50 flex items-center justify-center p-4" onClick={() => !deleting && setDeleteModal(null)}>
                    <div className="fixed inset-0 bg-black/50 backdrop-blur-sm" />
                    <div
                        className="relative w-full max-w-md bg-white dark:bg-[#0c0d12] rounded-2xl shadow-2xl border border-gray-100 dark:border-white/10 overflow-hidden"
                        onClick={e => e.stopPropagation()}
                    >
                        {/* Loading state */}
                        {deleteModal.loading && (
                            <div className="p-8 flex flex-col items-center gap-3">
                                <div className="w-8 h-8 border-3 border-red-400 border-t-transparent rounded-full animate-spin"></div>
                                <p className="text-sm text-gray-500 dark:text-gray-400">Checking dependencies...</p>
                            </div>
                        )}

                        {/* Warning: Open invoices */}
                        {!deleteModal.loading && deleteModal.checkData && !deleteModal.checkData.can_delete && (
                            <>
                                <div className="p-6 text-center">
                                    <div className="inline-flex items-center justify-center w-14 h-14 rounded-full bg-sky-600 dark:bg-sky-700/10 mb-4">
                                        <svg className="w-7 h-7 text-sky-700" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L4.082 16.5c-.77.833.192 2.5 1.732 2.5z" />
                                        </svg>
                                    </div>
                                    <h3 className="text-lg font-black text-gray-900 dark:text-white mb-2">Open Invoice Detected</h3>
                                    <p className="text-sm text-gray-500 dark:text-gray-400 mb-4">
                                        <span className="font-bold text-gray-900 dark:text-white">{deleteModal.user.name}</span> has unsettled invoices.
                                    </p>

                                    {/* Open bills list */}
                                    <div className="space-y-2 mb-4">
                                        {deleteModal.checkData.open_bills.map(bill => (
                                            <div key={bill.id} className="flex items-center justify-between px-4 py-2.5 rounded-xl bg-sky-70 dark:bg-sky-700/10 border border-sky-700 dark:border-sky-700/10">
                                                <span className="text-xs font-semibold text-gray-700 dark:text-gray-300">Box {bill.box_id}</span>
                                                <span className="text-xs font-bold text-sky-700 dark:text-sky-700">{bill.totalSum?.toLocaleString()} Ft</span>
                                            </div>
                                        ))}
                                    </div>

                                    <p className="text-xs text-gray-400">You can close the bill and proceed with deletion, or cancel.</p>
                                </div>
                                <div className="p-4 border-t border-gray-100 dark:border-white/5 flex gap-3">
                                    <button
                                        onClick={() => setDeleteModal(null)}
                                        disabled={deleting}
                                        className="flex-1 py-2.5 rounded-xl bg-gray-100 dark:bg-white/5 text-gray-700 dark:text-gray-300 font-bold text-sm hover:bg-gray-200 dark:hover:bg-white/10 transition-all disabled:opacity-50"
                                    >
                                        Cancel
                                    </button>
                                    <button
                                        onClick={() => confirmDelete(true)}
                                        disabled={deleting}
                                        className="flex-1 py-2.5 rounded-xl bg-red-500 text-white font-bold text-sm shadow-lg shadow-red-500/30 hover:bg-red-600 transition-all disabled:opacity-50 disabled:cursor-not-allowed"
                                    >
                                        {deleting ? 'Processing...' : 'Close Bill & Delete'}
                                    </button>
                                </div>
                            </>
                        )}

                        {/* Confirmation: Can delete */}
                        {!deleteModal.loading && deleteModal.checkData && deleteModal.checkData.can_delete && (
                            <>
                                <div className="p-6 text-center">
                                    <div className="inline-flex items-center justify-center w-14 h-14 rounded-full bg-red-100 dark:bg-red-500/10 mb-4">
                                        <svg className="w-7 h-7 text-red-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                                        </svg>
                                    </div>
                                    <h3 className="text-lg font-black text-gray-900 dark:text-white mb-2">Delete User</h3>
                                    <p className="text-sm text-gray-500 dark:text-gray-400 mb-4">
                                        Permanently delete <span className="font-bold text-gray-900 dark:text-white">{deleteModal.user.name}</span> and all associated data?
                                    </p>

                                    {/* Dependency summary */}
                                    {(deleteModal.checkData.custom_cocktails_count > 0 || deleteModal.checkData.reservations_count > 0 || deleteModal.checkData.bills_count > 0) && (
                                        <div className="space-y-1.5 mb-4 text-left">
                                            <p className="text-[10px] font-bold text-gray-400 uppercase tracking-wider mb-2">The following will be affected:</p>
                                            {deleteModal.checkData.custom_cocktails_count > 0 && (
                                                <div className="flex items-center gap-2 px-3 py-2 rounded-lg bg-red-50 dark:bg-red-500/5">
                                                    <span className="text-xs">🍹</span>
                                                    <span className="text-xs font-medium text-gray-700 dark:text-gray-300">
                                                        {deleteModal.checkData.custom_cocktails_count} custom cocktail{deleteModal.checkData.custom_cocktails_count !== 1 ? 's' : ''} will be <span className="font-bold text-red-500">deleted</span>
                                                    </span>
                                                </div>
                                            )}
                                            {deleteModal.checkData.reservations_count > 0 && (
                                                <div className="flex items-center gap-2 px-3 py-2 rounded-lg bg-amber-50 dark:bg-amber-500/5">
                                                    <span className="text-xs">📅</span>
                                                    <span className="text-xs font-medium text-gray-700 dark:text-gray-300">
                                                        {deleteModal.checkData.reservations_count} reservation{deleteModal.checkData.reservations_count !== 1 ? 's' : ''} will be <span className="font-bold text-red-500">deleted</span>
                                                    </span>
                                                </div>
                                            )}
                                            {deleteModal.checkData.bills_count > 0 && (
                                                <div className="flex items-center gap-2 px-3 py-2 rounded-lg bg-blue-50 dark:bg-blue-500/5">
                                                    <span className="text-xs">💳</span>
                                                    <span className="text-xs font-medium text-gray-700 dark:text-gray-300">
                                                        {deleteModal.checkData.bills_count} paid bill{deleteModal.checkData.bills_count !== 1 ? 's' : ''} will be <span className="font-bold text-blue-500">preserved (unlinked)</span>
                                                    </span>
                                                </div>
                                            )}
                                        </div>
                                    )}
                                </div>
                                <div className="p-4 border-t border-gray-100 dark:border-white/5 flex gap-3">
                                    <button
                                        onClick={() => setDeleteModal(null)}
                                        disabled={deleting}
                                        className="flex-1 py-2.5 rounded-xl bg-gray-100 dark:bg-white/5 text-gray-700 dark:text-gray-300 font-bold text-sm hover:bg-gray-200 dark:hover:bg-white/10 transition-all disabled:opacity-50"
                                    >
                                        Cancel
                                    </button>
                                    <button
                                        onClick={confirmDelete}
                                        disabled={deleting}
                                        className="flex-1 py-2.5 rounded-xl bg-red-500 text-white font-bold text-sm shadow-lg shadow-red-500/30 hover:bg-red-600 transition-all disabled:opacity-50 disabled:cursor-not-allowed"
                                    >
                                        {deleting ? 'Deleting...' : 'Delete Permanently'}
                                    </button>
                                </div>
                            </>
                        )}
                    </div>
                </div>
            )}
        </Layout>
    );
}
