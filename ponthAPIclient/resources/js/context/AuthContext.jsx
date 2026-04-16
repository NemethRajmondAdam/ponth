import React, { createContext, useState, useContext, useEffect } from 'react';
import axios from 'axios';

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }) => {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [user, setUser] = useState(null);
    const [authLoading, setAuthLoading] = useState(true);

    // Validate token against the server on load
    useEffect(() => {
        const token = localStorage.getItem('token');
        const storedUserstr = localStorage.getItem('user');

        if (token && storedUserstr) {
            axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;

            // Verify the token is still valid
            axios.get(`http://${window.location.hostname}:8000/api/user`)
                .then(res => {
                    setUser(res.data);
                    setIsLoggedIn(true);
                    // Keep localStorage in sync with server data
                    localStorage.setItem('user', JSON.stringify(res.data));
                })
                .catch(() => {
                    // Token expired or invalid — clean up
                    localStorage.removeItem('isLoggedIn');
                    localStorage.removeItem('token');
                    localStorage.removeItem('user');
                    delete axios.defaults.headers.common['Authorization'];
                    setIsLoggedIn(false);
                    setUser(null);
                })
                .finally(() => setAuthLoading(false));
        } else {
            setAuthLoading(false);
        }
    }, []);

    const login = (userData, token) => {
        setIsLoggedIn(true);
        setUser(userData);
        localStorage.setItem('isLoggedIn', 'true');
        localStorage.setItem('token', token);
        localStorage.setItem('user', JSON.stringify(userData));
        axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    };

    const logout = () => {
        setIsLoggedIn(false);
        setUser(null);
        localStorage.removeItem('isLoggedIn');
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        delete axios.defaults.headers.common['Authorization'];
    };

    return (
        <AuthContext.Provider value={{ isLoggedIn, user, login, logout, authLoading }}>
            {children}
        </AuthContext.Provider>
    );
};
