import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Layout from '../components/Layout';
import { useAuth } from '../context/AuthContext';
import api from '../api';

export default function Reservations() {
    const { user, isLoggedIn } = useAuth();
    const navigate = useNavigate();

    const [boxes, setBoxes] = useState([]);
    const [reservations, setReservations] = useState([]);
    const [unavailableSlots, setUnavailableSlots] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    // Form state
    const [selectedBox, setSelectedBox] = useState('');
    const [date, setDate] = useState('');
    const [time, setTime] = useState('');
    const [duration, setDuration] = useState(60);
    const [name, setName] = useState('');
    const [phone, setPhone] = useState('+36');
    const [phoneError, setPhoneError] = useState('');
    const [submitting, setSubmitting] = useState(false);

    // Validate Hungarian phone number: +36 followed by exactly 7 digits
    const validatePhone = (value) => {
        if (!value || value === '+36') {
            setPhoneError('Phone number is required.');
            return false;
        }
        const phoneRegex = /^\+36\d{9}$/;
        if (!phoneRegex.test(value)) {
            setPhoneError('Format: +36 followed by 9 digits (e.g. +36201234567)');
            return false;
        }
        setPhoneError('');
        return true;
    };

    const handlePhoneChange = (e) => {
        let value = e.target.value;
        // Always keep the +36 prefix
        if (!value.startsWith('+36')) {
            value = '+36';
        }
        // Only allow +, digits — no letters
        value = value.replace(/[^+\d]/g, '');
        // Limit to +36 + 9 digits = 12 chars
        if (value.length > 12) value = value.slice(0, 12);
        setPhone(value);
        if (value.length > 3) validatePhone(value);
        else setPhoneError('');
    };

    useEffect(() => {
        if (!isLoggedIn) {
            navigate('/login');
            return;
        }
        setName(user?.name || '');
        fetchData();
    }, [isLoggedIn]);

    const fetchData = async () => {
        setLoading(true);
        try {
            const [boxRes, resRes, unavailRes] = await Promise.all([
                api.get('/boxes'),
                api.get('/reservations'),
                api.get('/reservations/unavailable')
            ]);
            setBoxes(boxRes.data);
            setReservations(resRes.data.filter(r => r.status !== 'lemondva'));
            setUnavailableSlots(unavailRes.data);
        } catch (err) {
            console.error(err);
            setError('Failed to load data.');
        } finally {
            setLoading(false);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setSuccess('');

        if (!validatePhone(phone)) {
            return;
        }

        setSubmitting(true);

        try {
            await api.post('/reservations', {
                boxes_id: selectedBox,
                reservation_date: date,
                reservation_time: time,
                duration_minutes: duration,
                name,
                phone
            });
            setSuccess('Reservation created successfully!');
            setSelectedBox('');
            setDate('');
            setTime('');
            setDuration(60);
            setPhone('+36');
            fetchData();
        } catch (err) {
            if (err.response?.data?.message) {
                setError(err.response.data.message);
            } else {
                setError('Failed to create reservation.');
            }
        } finally {
            setSubmitting(false);
        }
    };

    const cancelReservation = async (id) => {
        if (!window.confirm('Are you sure you want to cancel this reservation?')) return;
        setError('');
        setSuccess('');
        try {
            await api.delete(`/reservations/${id}`);
            setSuccess('Reservation cancelled.');
            fetchData();
        } catch (err) {
            setError('Failed to cancel reservation.');
        }
    };

    const isBoxAvailable = (boxId) => {
        if (!date || !time) return true; // Show all if date/time not yet picked

        const [selHours, selMins] = time.split(':').map(Number);
        const selStart = selHours * 60 + selMins;
        const selEnd = selStart + duration;

        for (const slot of unavailableSlots) {
            if (slot.boxes_id === boxId && slot.reservation_date === date) {
                const [slotHours, slotMins] = slot.reservation_time.split(':').map(Number);
                const slotStart = slotHours * 60 + slotMins;
                const slotEnd = slotStart + slot.duration_minutes;

                // Check overlap (Strict overlap: end exactly at start is fine)
                if (selStart < slotEnd && selEnd > slotStart) {
                    return false;
                }
            }
        }
        return true;
    };

    // Deselect if active box becomes unavailable
    useEffect(() => {
        if (selectedBox && !isBoxAvailable(selectedBox)) {
            setSelectedBox('');
        }
    }, [date, time, duration, unavailableSlots]);

    // Get today's date string for min date
    const today = new Date().toISOString().split('T')[0];

    // Time slots
    const timeSlots = [];
    for (let h = 16; h <= 23; h++) {
        timeSlots.push(`${h.toString().padStart(2, '0')}:00`);
        timeSlots.push(`${h.toString().padStart(2, '0')}:30`);
    }

    if (!isLoggedIn) return null;

    return (
        <Layout>
            <div className="min-h-[80vh] px-4 sm:px-6 lg:px-8 py-10 max-w-5xl mx-auto">
                {/* Header */}
                <div className="mb-10">
                    <div className="inline-flex items-center justify-center w-14 h-14 rounded-2xl bg-gradient-to-br from-sky-700 to-sky-950 shadow-lg shadow-sky-700/10 mb-5">
                        <svg className="w-7 h-7 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                        </svg>
                    </div>
                    <h1 className="text-3xl font-black tracking-tight text-gray-900 dark:text-white">
                        Reserve a Box
                    </h1>
                    <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">
                        Book your private box for a perfect evening
                    </p>
                </div>

                {/* Messages */}
                {error && (
                    <div className="mb-6 bg-red-50 dark:bg-red-500/10 text-red-500 text-sm p-4 rounded-xl text-center font-medium">
                        {error}
                    </div>
                )}
                {success && (
                    <div className="mb-6 bg-emerald-50 dark:bg-emerald-500/10 text-emerald-500 text-sm p-4 rounded-xl text-center font-medium">
                        {success}
                    </div>
                )}

                {loading ? (
                    <div className="flex justify-center py-20">
                        <div className="w-10 h-10 border-4 border-sky-700 border-t-transparent rounded-full animate-spin"></div>
                    </div>
                ) : (
                    <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                        {/* Reservation Form */}
                        <div className="bg-white dark:bg-[#0c0d12] rounded-2xl shadow-xl border border-gray-100 dark:border-white/5 p-8">
                            <h2 className="text-xl font-bold text-gray-900 dark:text-white mb-6">New Reservation</h2>
                            <form className="space-y-5" onSubmit={handleSubmit}>
                                {/* Box selection */}
                                <div>
                                    <label className="block text-sm font-semibold text-gray-900 dark:text-gray-200 mb-2">
                                        Select Box
                                    </label>
                                    <div className="grid grid-cols-2 sm:grid-cols-3 gap-3">
                                        {boxes.filter(b => b.online).map((box) => {
                                            const available = isBoxAvailable(box.id);
                                            return (
                                                <button
                                                    key={box.id}
                                                    type="button"
                                                    disabled={!available}
                                                    onClick={() => setSelectedBox(box.id)}
                                                    className={`p-4 rounded-xl border-2 transition-all text-center ${!available
                                                        ? 'opacity-40 cursor-not-allowed border-gray-100 dark:border-white/5 bg-gray-50 dark:bg-white/5 grayscale'
                                                        : selectedBox === box.id
                                                            ? 'border-sky-700 bg-sky-70 dark:bg-sky-700/10 shadow-lg shadow-sky-700/10'
                                                            : 'border-gray-200 dark:border-white/10 hover:border-sky-700 dark:hover:border-sky-700/10'
                                                        }`}
                                                >
                                                    <div className={`text-lg font-black ${!available ? 'text-gray-500'
                                                        : selectedBox === box.id ? 'text-sky-700 dark:text-sky-700'
                                                            : 'text-gray-900 dark:text-white'
                                                        }`}>
                                                        Box {box.number}
                                                    </div>
                                                    <div className="text-xs text-gray-500 dark:text-gray-400 mt-1">
                                                        {box.seats} seats {!available && '· Reserved'}
                                                    </div>
                                                </button>
                                            );
                                        })}
                                    </div>
                                </div>

                                {/* Date */}
                                <div>
                                    <label htmlFor="res-date" className="block text-sm font-semibold text-gray-900 dark:text-gray-200 mb-2">
                                        Date
                                    </label>
                                    <input
                                        id="res-date"
                                        type="date"
                                        min={today}
                                        required
                                        className="block w-full rounded-xl border-0 py-3 px-4 text-gray-900 dark:text-white dark:bg-[#1a1c23] shadow-sm ring-1 ring-inset ring-gray-300 dark:ring-white/10 focus:ring-2 focus:ring-inset focus:ring-sky-700 sm:text-sm transition-all"
                                        value={date}
                                        onChange={(e) => setDate(e.target.value)}
                                    />
                                </div>

                                {/* Time */}
                                <div>
                                    <label htmlFor="res-time" className="block text-sm font-semibold text-gray-900 dark:text-gray-200 mb-2">
                                        Time
                                    </label>
                                    <select
                                        id="res-time"
                                        required
                                        className="block w-full rounded-xl border-0 py-3 px-4 text-gray-900 dark:text-white dark:bg-[#1a1c23] shadow-sm ring-1 ring-inset ring-gray-300 dark:ring-white/10 focus:ring-2 focus:ring-inset focus:ring-sky-700 sm:text-sm transition-all"
                                        value={time}
                                        onChange={(e) => setTime(e.target.value)}
                                    >
                                        <option value="">Select time...</option>
                                        {timeSlots.map(t => (
                                            <option key={t} value={t}>{t}</option>
                                        ))}
                                    </select>
                                </div>

                                {/* Duration */}
                                <div>
                                    <label htmlFor="res-duration" className="block text-sm font-semibold text-gray-900 dark:text-gray-200 mb-2">
                                        Duration
                                    </label>
                                    <select
                                        id="res-duration"
                                        className="block w-full rounded-xl border-0 py-3 px-4 text-gray-900 dark:text-white dark:bg-[#1a1c23] shadow-sm ring-1 ring-inset ring-gray-300 dark:ring-white/10 focus:ring-2 focus:ring-inset focus:ring-sky-700 sm:text-sm transition-all"
                                        value={duration}
                                        onChange={(e) => setDuration(parseInt(e.target.value))}
                                    >
                                        <option value={60}>1 hour</option>
                                        <option value={90}>1.5 hours</option>
                                        <option value={120}>2 hours</option>
                                        <option value={180}>3 hours</option>
                                    </select>
                                </div>

                                {/* Name */}
                                <div>
                                    <label htmlFor="res-name" className="block text-sm font-semibold text-gray-900 dark:text-gray-200 mb-2">
                                        Name
                                    </label>
                                    <input
                                        id="res-name"
                                        type="text"
                                        required
                                        className="block w-full rounded-xl border-0 py-3 px-4 text-gray-900 dark:text-white dark:bg-[#1a1c23] shadow-sm ring-1 ring-inset ring-gray-300 dark:ring-white/10 focus:ring-2 focus:ring-inset focus:ring-sky-700 sm:text-sm transition-all"
                                        value={name}
                                        onChange={(e) => setName(e.target.value)}
                                        placeholder="Your name"
                                    />
                                </div>

                                {/* Phone */}
                                <div>
                                    <label htmlFor="res-phone" className="block text-sm font-semibold text-gray-900 dark:text-gray-200 mb-2">
                                        Phone
                                    </label>
                                    <input
                                        id="res-phone"
                                        type="tel"
                                        required
                                        className={`block w-full rounded-xl border-0 py-3 px-4 text-gray-900 dark:text-white dark:bg-[#1a1c23] shadow-sm ring-1 ring-inset sm:text-sm transition-all ${phoneError
                                                ? 'ring-red-400 dark:ring-red-500 focus:ring-2 focus:ring-inset focus:ring-red-500'
                                                : 'ring-gray-300 dark:ring-white/10 focus:ring-2 focus:ring-inset focus:ring-sky-700'
                                            }`}
                                        value={phone}
                                        onChange={handlePhoneChange}
                                        placeholder="+36201234567"
                                        maxLength={12}
                                    />
                                    {phoneError && (
                                        <p className="mt-1.5 text-xs text-red-500 font-medium">{phoneError}</p>
                                    )}
                                </div>

                                {/* Submit */}
                                <button
                                    type="submit"
                                    disabled={!selectedBox || submitting}
                                    className="flex w-full justify-center rounded-xl bg-gradient-to-r from-sky-700 to-sky-950 px-3 py-3 text-sm font-bold text-white shadow-lg shadow-sky-700/10 hover:from-sky-700 hover:to-sky-950 transition-all hover:-translate-y-0.5 active:translate-y-0 disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:translate-y-0"
                                >
                                    {submitting ? 'Reserving...' : 'Reserve Box'}
                                </button>
                            </form>
                        </div>

                        {/* Existing Reservations */}
                        <div className="bg-white dark:bg-[#0c0d12] rounded-2xl shadow-xl border border-gray-100 dark:border-white/5 overflow-hidden">
                            <div className="px-8 py-6 border-b border-gray-200 dark:border-white/5">
                                <h2 className="text-xl font-bold text-gray-900 dark:text-white">Upcoming Reservations</h2>
                                <p className="text-xs text-gray-500 dark:text-gray-400 mt-1">Active bookings</p>
                            </div>

                            {reservations.length === 0 ? (
                                <div className="px-8 py-16 text-center">
                                    <svg className="w-12 h-12 text-gray-300 dark:text-gray-600 mx-auto mb-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                                    </svg>
                                    <p className="text-sm text-gray-400 dark:text-gray-500">No reservations yet</p>
                                </div>
                            ) : (
                                <div className="divide-y divide-gray-100 dark:divide-white/5 max-h-[600px] overflow-y-auto">
                                    {reservations.map((res) => (
                                        <div key={res.id} className="px-6 py-4 hover:bg-gray-50 dark:hover:bg-white/[0.02] transition-colors">
                                            <div className="flex items-center justify-between">
                                                <div className="flex items-center gap-4">
                                                    <div className="w-11 h-11 rounded-xl bg-gradient-to-br from-sky-700 to-sky-950 flex items-center justify-center text-white font-black text-sm flex-shrink-0">
                                                        {res.box?.number || '?'}
                                                    </div>
                                                    <div>
                                                        <p className="text-sm font-semibold text-gray-900 dark:text-white">
                                                            Box {res.box?.number} · {res.name}
                                                        </p>
                                                        <p className="text-xs text-gray-500 dark:text-gray-400 mt-0.5">
                                                            {res.reservation_date} at {res.reservation_time} · {res.duration_minutes} min
                                                        </p>
                                                        {res.phone && (
                                                            <p className="text-xs text-gray-500 dark:text-gray-400 mt-0.5">
                                                                📞 {res.phone}
                                                            </p>
                                                        )}
                                                    </div>
                                                </div>
                                                <div className="flex items-center gap-2">
                                                    <span className={`px-2 py-1 text-xs font-bold rounded-lg ${res.status === 'foglalva' ? 'bg-sky-700 dark:bg-sky-700/10 text-sky-600 dark:text-sky-600'
                                                        : res.status === 'megerositve' ? 'bg-sky-600 dark:bg-sky-700/10 text-sky-600 dark:text-sky-400'
                                                            : 'bg-gray-100 dark:bg-white/5 text-gray-500'
                                                        }`}>
                                                        {res.status}
                                                    </span>
                                                    {res.status !== 'lemondva' && (
                                                        <button
                                                            onClick={() => cancelReservation(res.id)}
                                                            className="p-1.5 rounded-lg text-gray-400 hover:text-red-500 hover:bg-red-50 dark:hover:bg-red-500/10 transition-all"
                                                            title="Cancel"
                                                        >
                                                            <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                                                            </svg>
                                                        </button>
                                                    )}
                                                </div>
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            )}

                            <div className="px-6 py-4 border-t border-gray-200 dark:border-white/5 bg-gray-50 dark:bg-white/[0.02]">
                                <p className="text-xs text-gray-500 dark:text-gray-400">
                                    <span className="font-bold text-gray-700 dark:text-gray-200">{reservations.length}</span> active reservations
                                </p>
                            </div>
                        </div>
                    </div>
                )}
            </div>
        </Layout>
    );
}
