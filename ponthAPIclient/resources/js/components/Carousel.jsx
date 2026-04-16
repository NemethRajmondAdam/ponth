import React, { useState, useEffect } from 'react';

const IMAGES = [
    {
        url: 'https://images.unsplash.com/photo-1514933651103-005eec06c04b?q=80&w=1934&auto=format&fit=crop',
        title: 'Craft Cocktails',
        subtitle: 'Signature drinks mixed to perfection.'
    },
    {
        url: 'https://images.unsplash.com/photo-1543007630-9710e4a00a20?q=80&w=2070&auto=format&fit=crop',
        title: 'Vibrant Atmosphere',
        subtitle: 'Unforgettable nights out.'
    },
    {
        url: 'https://images.unsplash.com/photo-1572116469696-31de0f17cc34?q=80&w=1974&auto=format&fit=crop',
        title: 'Premium Selection',
        subtitle: 'Explore our curated spirits.'
    }
];

export default function Carousel() {
    const [currentIndex, setCurrentIndex] = useState(0);

    useEffect(() => {
        const timer = setInterval(() => {
            setCurrentIndex((prevIndex) =>
                prevIndex === IMAGES.length - 1 ? 0 : prevIndex + 1
            );
        }, 5000);
        return () => clearInterval(timer);
    }, []);

    const goToNext = () => {
        setCurrentIndex((prevIndex) => prevIndex === IMAGES.length - 1 ? 0 : prevIndex + 1);
    };

    const goToPrev = () => {
        setCurrentIndex((prevIndex) => prevIndex === 0 ? IMAGES.length - 1 : prevIndex - 1);
    };

    return (
        <div className="relative w-full h-[60vh] md:h-[75vh] group overflow-hidden bg-black rounded-2xl md:rounded-[2.5rem] shadow-2xl shadow-black/20">
            {/* Images */}
            {IMAGES.map((image, index) => (
                <div
                    key={index}
                    className={`absolute inset-0 transition-opacity duration-1000 ease-in-out ${index === currentIndex ? 'opacity-100 z-10' : 'opacity-0 z-0'
                        }`}
                >
                    <div className="absolute inset-0 bg-gradient-to-t from-black/80 via-black/20 to-transparent z-10" />
                    <img
                        src={image.url}
                        alt={image.title}
                        className={`w-full h-full object-cover transition-transform duration-[10000ms] ease-linear ${index === currentIndex ? 'scale-110' : 'scale-100'
                            }`}
                    />

                    {/* Text Content */}
                    <div className={`absolute bottom-0 left-0 right-0 p-8 md:p-16 z-20 transition-all duration-700 delay-300 ${index === currentIndex ? 'translate-y-0 opacity-100' : 'translate-y-10 opacity-0'
                        }`}>
                        <h2 className="text-4xl md:text-6xl font-black text-white mb-4 tracking-tight">
                            {image.title}
                        </h2>
                        <p className="text-xl md:text-2xl text-gray-200 font-light max-w-2xl">
                            {image.subtitle}
                        </p>
                    </div>
                </div>
            ))}

            {/* Navigation Arrows */}
            <button
                onClick={goToPrev}
                className="absolute left-6 top-1/2 -translate-y-1/2 z-30 p-3 rounded-full bg-white/10 hover:bg-white/20 text-white backdrop-blur-md opacity-0 group-hover:opacity-100 transition-all duration-300 pointer-events-none group-hover:pointer-events-auto"
            >
                <svg className="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" />
                </svg>
            </button>
            <button
                onClick={goToNext}
                className="absolute right-6 top-1/2 -translate-y-1/2 z-30 p-3 rounded-full bg-white/10 hover:bg-white/20 text-white backdrop-blur-md opacity-0 group-hover:opacity-100 transition-all duration-300 pointer-events-none group-hover:pointer-events-auto"
            >
                <svg className="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5l7 7-7 7" />
                </svg>
            </button>

            {/* Pagination Indicators */}
            <div className="absolute bottom-6 md:bottom-12 right-8 md:right-16 z-30 flex gap-3">
                {IMAGES.map((_, index) => (
                    <button
                        key={index}
                        onClick={() => setCurrentIndex(index)}
                        className={`h-1.5 rounded-full transition-all duration-500 ease-out ${index === currentIndex ? 'w-10 bg-sky-700' : 'w-4 bg-white/40 hover:bg-white/60'
                            }`}
                        aria-label={`Go to slide ${index + 1}`}
                    />
                ))}
            </div>
        </div>
    );
}
