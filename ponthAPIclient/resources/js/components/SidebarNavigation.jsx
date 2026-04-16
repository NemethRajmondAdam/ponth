import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import useIsMobile from '../hooks/useIsMobile';

export default function SidebarNavigation({ isOpen, setIsOpen }) {
    const { isLoggedIn, logout, user } = useAuth();
    const location = useLocation();
    const isMobile = useIsMobile();

    // Reusable link component
    const NavLink = ({ to, children, icon }) => {
        const isActive = location.pathname === to;
        return (
            <Link
                to={to}
                onClick={() => setIsOpen(false)}
                className={`flex items-center gap-4 px-6 py-4 mx-4 my-2 rounded-xl transition-all duration-300 group ${isActive
                    ? 'bg-gradient-to-r from-sky-700 to-sky-950 text-white shadow-lg shadow-sky-500/10'
                    : 'text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-white/5 hover:text-sky-700 dark:hover:text-sky-700'
                    }`}
            >
                <span className={`transition-transform duration-300 ${isActive ? 'scale-110' : 'group-hover:scale-110'}`}>
                    {icon}
                </span>
                <span className="font-medium tracking-wide">{children}</span>
            </Link>
        );
    };

    return (
        <>
            {/* Backdrop */}
            <div
                className={`fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-opacity duration-300 ${isOpen ? 'opacity-100' : 'opacity-0 pointer-events-none'}`}
                onClick={() => setIsOpen(false)}
            />

            {/* Sidebar container */}
            <aside
                className={`fixed top-0 left-0 h-full w-72 md:w-80 bg-white/95 dark:bg-[#0c0d12]/95 backdrop-blur-xl border-r border-gray-200 dark:border-white/5 shadow-2xl z-50 transform transition-transform duration-300 ease-[cubic-bezier(0.4,0,0.2,1)] ${isOpen ? 'translate-x-0' : '-translate-x-full'
                    } flex flex-col`}
            >
                {/* Header */}
                <div className="flex items-center justify-between px-8 py-8 border-b border-gray-200 dark:border-white/5">
                    <Link to="/" onClick={() => setIsOpen(false)} className="flex items-center gap-3">
                        <img src="/ponth_icon.ico" alt="Ponth" className="w-10 h-10 rounded-xl shadow-lg shadow-sky-500/10 object-cover" />
                        <span className="text-2xl font-black tracking-tight text-gray-900 dark:text-white">
                            Ponth<span className="text-sky-700">.</span>
                        </span>
                    </Link>
                    <button
                        onClick={() => setIsOpen(false)}
                        className="p-2 rounded-full hover:bg-gray-100 dark:hover:bg-white/10 text-gray-500 dark:text-gray-400 hover:text-gray-900 dark:hover:text-white transition-colors"
                    >
                        <svg className="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                        </svg>
                    </button>
                </div>

                {/* Navigation Links */}
                <nav className="flex-1 overflow-y-auto py-6 custom-scrollbar">
                    <div className="mb-4 px-8 text-xs font-bold text-gray-400 uppercase tracking-widest">Menu</div>

                    <NavLink to="/" icon={
                        <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" /></svg>
                    }>
                        Home
                    </NavLink>

                    <NavLink to="/drinks" icon={
                        <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 15v4a2 2 0 01-2 2H5a2 2 0 01-2-2v-4m20-4a8 8 0 10-16 0h16zM3 15h18" /></svg>
                    }>
                        Drinks
                    </NavLink>

                    {/* Authenticated Links */}
                    {isLoggedIn && (
                        <>
                            <div className="mt-8 mb-4 px-8 text-xs font-bold text-gray-400 uppercase tracking-widest">Dashboard</div>
                            <NavLink to="/profile" icon={
                                <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" /></svg>
                            }>
                                Profile
                            </NavLink>
                            <NavLink to="/reservations" icon={
                                <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" /></svg>
                            }>
                                Reservations
                            </NavLink>
                            {isMobile && (
                                <NavLink to="/order-drink" icon={
                                    <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v1m6 11h2m-6 0h-2v4m0-11v3m0 0h.01M12 12h4.01M16 20h4M4 12h4m12 0h.01M5 8h2a1 1 0 001-1V5a1 1 0 00-1-1H5a1 1 0 00-1 1v2a1 1 0 001 1zm12 0h2a1 1 0 001-1V5a1 1 0 00-1-1h-2a1 1 0 00-1 1v2a1 1 0 001 1zM5 20h2a1 1 0 001-1v-2a1 1 0 00-1-1H5a1 1 0 00-1 1v2a1 1 0 001 1z" /></svg>
                                }>
                                    Order Drink
                                </NavLink>
                            )}
                            {/* Admin link */}
                            {user?.is_admin && (
                                <>
                                    <div className="mt-8 mb-4 px-8 text-xs font-bold text-gray-400 uppercase tracking-widest">Admin</div>
                                    <NavLink to="/admin" icon={
                                        <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" /></svg>
                                    }>
                                        Manage Users
                                    </NavLink>
                                </>
                            )}
                        </>
                    )}
                </nav>

                {/* Footer Auth Actions */}
                <div className="p-6 border-t border-gray-200 dark:border-white/5 space-y-3">
                    {isLoggedIn ? (
                        <>
                            <Link
                                to="/profile"
                                onClick={() => setIsOpen(false)}
                                className="flex items-center gap-3 w-full px-4 py-3 rounded-xl hover:bg-gray-100 dark:hover:bg-white/5 transition-colors group"
                            >
                                <div className="w-10 h-10 rounded-full bg-gradient-to-r from-sky-700 to-sky-950 flex items-center justify-center text-white font-bold shadow-md uppercase">
                                    {user?.name ? user.name.charAt(0) : 'U'}
                                </div>
                                <div className="flex-1 text-left">
                                    <p className="text-sm font-semibold text-gray-900 dark:text-white truncate pr-2" title={user?.name || 'User'}>
                                        {user?.name || 'User'}
                                    </p>
                                    <p className="text-xs text-gray-500">View Profile</p>
                                </div>
                            </Link>
                            <button
                                onClick={() => {
                                    logout();
                                    setIsOpen(false);
                                }}
                                className="flex items-center justify-center gap-2 w-full px-4 py-3 text-sm font-bold text-gray-700 dark:text-gray-300 bg-gray-100 dark:bg-white/5 hover:bg-red-50 dark:hover:bg-red-500/10 hover:text-red-600 dark:hover:text-red-400 rounded-xl transition-all border border-transparent hover:border-red-100 dark:hover:border-red-500/20"
                            >
                                <svg className="w-5 h-5 transition-transform" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
                                </svg>
                                Sign Out
                            </button>
                        </>
                    ) : (
                        <>
                            <Link
                                to="/login"
                                onClick={() => setIsOpen(false)}
                                className="flex items-center justify-center gap-2 w-full px-4 py-3 bg-gradient-to-tr from-sky-700 to-sky-950 hover:from-sky-600 hover:to-sky-900 text-white font-bold rounded-xl shadow-lg shadow-sky-700/10 transition-all hover:-translate-y-0.5 active:translate-y-0"
                            >
                                <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 16l-4-4m0 0l4-4m-4 4h14m-5 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h7a3 3 0 013 3v1" />
                                </svg>
                                Log In
                            </Link>
                            <Link
                                to="/register"
                                onClick={() => setIsOpen(false)}
                                className="flex items-center justify-center gap-2 w-full px-4 py-3 text-sm font-bold text-gray-700 dark:text-gray-300 bg-gray-100 dark:bg-white/5 hover:bg-sky-70 dark:hover:bg-sky-700/10 hover:text-sky-700 dark:hover:text-sky-700 rounded-xl transition-all border border-transparent hover:border-sky-700 dark:hover:border-sky-700/10"
                            >
                                <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z" />
                                </svg>
                                Register
                            </Link>
                        </>
                    )}
                </div>
            </aside>
        </>
    );
}
