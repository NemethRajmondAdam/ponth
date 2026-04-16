import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './bootstrap';
import '../css/app.css';
import { AuthProvider } from './context/AuthContext';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import Drinks from './pages/Drinks';
import AdminDashboard from './pages/AdminDashboard';
import Reservations from './pages/Reservations';
import OrderDrink from './pages/OrderDrink';
import Profile from './pages/Profile';
import CocktailLab from './pages/CocktailLab';

function NotFound() {
    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-100 dark:bg-[#0c0d12]">
            <h1 className="text-2xl font-bold text-red-500">404 - Page Not Found</h1>
        </div>
    );
}

function App() {
    return (
        <AuthProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/drinks" element={<Drinks />} />
                    <Route path="/reservations" element={<Reservations />} />
                    <Route path="/admin" element={<AdminDashboard />} />
                    <Route path="/order-drink" element={<OrderDrink />} />
                    <Route path="/profile" element={<Profile />} />
                    <Route path="/cocktail-lab" element={<CocktailLab />} />
                    <Route path="*" element={<NotFound />} />
                </Routes>
            </BrowserRouter>
        </AuthProvider>
    );
}

const rootElement = document.getElementById('app');
if (rootElement) {
    ReactDOM.createRoot(rootElement).render(
        <React.StrictMode>
            <App />
        </React.StrictMode>
    );
}
