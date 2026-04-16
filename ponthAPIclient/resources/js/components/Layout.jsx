import React, { useState } from 'react';
import SidebarNavigation from './SidebarNavigation';

export default function Layout({ children }) {
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);

    return (
        <div className="min-h-screen bg-[#f8fafc] dark:bg-[#06070a] text-gray-900 dark:text-gray-100 font-sans transition-colors duration-300">
            <SidebarNavigation isOpen={isSidebarOpen} setIsOpen={setIsSidebarOpen} />

            {/* Minimal Header just for the toggle button & logo */}
            <header className="fixed top-0 left-0 right-0 h-16 bg-white/80 dark:bg-[#0c0d12]/80 backdrop-blur-md border-b border-gray-200 dark:border-white/5 flex items-center px-4 md:px-6 z-30 transition-colors duration-300">
                <button
                    onClick={() => setIsSidebarOpen(true)}
                    className="p-2 -ml-2 rounded-xl text-gray-600 dark:text-gray-400 hover:bg-gray-100 dark:hover:bg-white/5 transition-all focus:outline-none focus:ring-2 focus:ring-sky-700"
                >
                    <svg className="w-7 h-7" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
                    </svg>
                </button>
                <div className="ml-4 font-black text-xl tracking-tight">
                    <span className="text-gray-900 dark:text-white">Ponth</span>
                    <span className="text-sky-700">.</span>
                </div>
            </header>

            {/* Main Content Area */}
            <main className="pt-16 min-h-screen flex flex-col">
                <div className="w-full flex-1">
                    {children}
                </div>
            </main>
        </div>
    );
}
