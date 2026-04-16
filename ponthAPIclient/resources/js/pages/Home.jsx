import React from 'react';
import Layout from '../components/Layout';
import Carousel from '../components/Carousel';
import { Link } from 'react-router-dom';

export default function Home() {
    return (
        <Layout>
            <div className="w-full">
                {/* Hero Section */}
                <div className="px-4 py-8 sm:px-6 lg:px-8 max-w-7xl mx-auto mb-12 text-center md:text-left">
                    <h1 className="text-5xl md:text-7xl font-black text-gray-900 dark:text-white tracking-tighter mb-4">
                        Welcome to <span className="text-transparent bg-clip-text bg-gradient-to-r from-sky-700 to-sky-950">Ponth</span><span>.</span>
                    </h1>
                    <p className="text-xl md:text-2xl text-gray-600 dark:text-gray-400 max-w-2xl font-light mb-8 md:mb-0">
                        Experience the finest craft cocktails and the most vibrant atmosphere in town.
                    </p>
                </div>

                {/* Carousel Full Width Container */}
                <div className="px-4 sm:px-6 lg:px-8 max-w-[100rem] mx-auto pb-16">
                    <Carousel />
                </div>
            </div>
        </Layout>
    );
}
