<?php

namespace App\Http\Controllers\Auth;

use Illuminate\Support\Facades\Http;
use App\Http\Controllers\Controller;
use App\Http\Requests\Auth\LoginRequest;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\View\View;

class AuthenticatedSessionController extends Controller
{
    /**
     * Display the login view.
     */
    public function create(): View
    {
        return view('auth.login');
    }

    /**
     * Handle an incoming authentication request.
     */
    public function store(LoginRequest $request): RedirectResponse
{
    // API login hívás
    $response = Http::api()->post('/user/login', [
        'email' => $request->email,
        'password' => $request->password,
    ]);

    if ($response->successful()) {
        $responseBody = json_decode($response->body());

        if (empty($responseBody->data)) {
            return back()->withErrors([
                'message' => $responseBody->message,
            ]);
        }

        // Token és felhasználói adatok mentése session-be
        session([
            'api_token' => $responseBody->data->token,
            'user_name' => $responseBody->data->name,
            'user_email' => $responseBody->data->email,
        ]);

        return redirect()->intended('/');
    }

    return back()->withErrors([
        'email' => 'Hibás bejelentkezési adatok.',
    ]);
}

public function destroy(Request $request): RedirectResponse
{
    session()->forget('api_token');
    session()->forget('user_name');
    session()->forget('user_email');

    return redirect('/');
}
}
