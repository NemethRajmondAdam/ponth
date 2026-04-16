import axios from 'axios';

// Dynamically determine the API base URL based on the current browser host.
// This ensures the app works on both localhost and from other devices (like phones on the same network).
const API_BASE_URL = `http://${window.location.hostname}:8000/api`;

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
    },
});

// Attach auth token to every request if available
api.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

// Auto-logout on 401 responses (expired/invalid token)
api.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response?.status === 401) {
            localStorage.removeItem('isLoggedIn');
            localStorage.removeItem('token');
            localStorage.removeItem('user');
            // Reload to reset the entire auth state
            window.location.reload();
        }
        return Promise.reject(error);
    }
);

export default api;
