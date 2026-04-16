<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Models\Reservation;
use Illuminate\Http\Request;

class ReservationController extends Controller
{
    // Returns only the logged-in user's reservations
    public function index(Request $request)
    {
        return Reservation::with('box')
            ->where('user_id', $request->user()->id)
            ->where('status', '!=', 'Cancelled')
            ->orderBy('reservation_date')
            ->orderBy('reservation_time')
            ->get();
    }

    // Returns all active reservations (without personal details) so users can see unavailable slots
    public function unavailableSlots()
    {
        return Reservation::where('status', '!=', 'Cancelled')
            ->select('boxes_id', 'reservation_date', 'reservation_time', 'duration_minutes')
            ->orderBy('reservation_date')
            ->orderBy('reservation_time')
            ->get();
    }

    public function store(Request $request)
    {
        $request->validate([
            'boxes_id' => 'required|exists:boxes,id',
            'reservation_date' => 'required|date',
            'reservation_time' => 'required',
            'name' => 'required|string',
            'phone' => 'required|string',
            'duration_minutes' => 'required|integer'
        ]);

        $conflict = Reservation::where('boxes_id', $request->boxes_id)
            ->where('reservation_date', $request->reservation_date)
            ->where('reservation_time', $request->reservation_time)
            ->where('status', '!=', 'Cancelled')
            ->exists();

        if ($conflict)
            return response()->json(['message' => 'This time slot is already taken'], 422);

        $reservation = Reservation::create([
            'boxes_id' => $request->boxes_id,
            'reservation_date' => $request->reservation_date,
            'reservation_time' => $request->reservation_time,
            'duration_minutes' => $request->duration_minutes,
            'name' => $request->name,
            'phone' => $request->phone,
            'user_id' => $request->user()->id,
            'reserved_at' => now(),
            'status' => 'Reserved'
        ]);

        return response()->json($reservation->load('box'), 201);
    }

    // Only the owner can cancel their reservation
    public function destroy(Request $request, $id)
    {
        $reservation = Reservation::findOrFail($id);

        if ($reservation->user_id !== $request->user()->id) {
            return response()->json(['message' => 'You can only cancel your own reservations'], 403);
        }

        $reservation->update(['status' => 'Cancelled']);
        return response()->json(['message' => 'Reservation cancelled']);
    }
}