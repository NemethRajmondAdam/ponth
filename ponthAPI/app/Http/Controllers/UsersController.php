<?php
namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Models\User;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Facades\Validator;
use Illuminate\Support\Facades\DB;

class UsersController extends Controller
{
    // Regisztráció
    public function register(Request $request)
    {
        $validator = Validator::make($request->all(), [
            'name' => 'required|string|max:255',
            'email' => 'required|string|email|max:255|unique:users',
            'password' => 'required|string|min:6|confirmed', // password_confirmation mezőt is vár
        ]);

        if ($validator->fails()) {
            return response()->json($validator->errors(), 422);
        }

        $user = User::create([
            'name' => $request->name,
            'email' => $request->email,
            'password' => Hash::make($request->password),
        ]);

        $token = $user->createToken('access')->plainTextToken;

        return response()->json([
            'message' => 'Sikeres regisztráció!',
            'user' => [
                'id' => $user->id,
                'name' => $user->name,
                'email' => $user->email,
                'is_admin' => (bool)$user->is_admin,
            ],
            'access_token' => $token,
        ], 201);
    }

    // Bejelentkezés
    public function login(Request $request)
    {
        $request->validate([
            'email' => 'required|email',
            'password' => 'required',
        ]);

        $user = User::where('email', $request->email)->first();

        if (!$user || !Hash::check($request->password, $user->password)) {
            return response()->json(['message' => 'Érvénytelen e-mail vagy jelszó'], 401);
        }

        // Régi tokenek takarítása
        $user->tokens()->delete();

        // Új token generálása
        $token = $user->createToken('access')->plainTextToken;

        return response()->json([
            'user' => [
                'id' => $user->id,
                'name' => $user->name,
                'email' => $user->email,
                'is_admin' => (bool)$user->is_admin,
            ],
            'access_token' => $token,
            'token_type' => 'Bearer'
        ]);
    }

    // Kijelentkezés
    public function logout(Request $request)
    {
        // Az aktuális token törlése
        $request->user()->currentAccessToken()->delete();

        return response()->json(['message' => 'Sikeres kijelentkezés']);
    }

    // Admin: összes felhasználó lekérdezése
    public function listUsers(Request $request)
    {
        if (!$request->user()->is_admin) {
            return response()->json(['message' => 'Hozzáférés megtagadva'], 403);
        }

        $users = User::select('id', 'name', 'email', 'is_admin', 'created_at')->get();
        return response()->json($users);
    }

    // Admin: felhasználó frissítése (is_admin toggle)
    public function updateUser(Request $request, $id)
    {
        if (!$request->user()->is_admin) {
            return response()->json(['message' => 'Hozzáférés megtagadva'], 403);
        }

        $user = User::findOrFail($id);

        if ($request->has('is_admin')) {
            $user->is_admin = $request->is_admin;
        }
        if ($request->has('name')) {
            $user->name = $request->name;
        }
        if ($request->has('email')) {
            $user->email = $request->email;
        }

        $user->save();

        return response()->json(['message' => 'Felhasználó frissítve', 'user' => $user]);
    }

    // Admin: pre-flight check before deletion
    public function deleteCheck(Request $request, $id)
    {
        if (!$request->user()->is_admin) {
            return response()->json(['message' => 'Hozzáférés megtagadva'], 403);
        }

        $user = User::findOrFail($id);

        $openBills = DB::table('open_bills')->where('user_id', $user->id)->get(['id', 'totalSum', 'box_id']);
        $customCocktailsCount = $user->customCocktails()->count();
        $reservationsCount = DB::table('reservations')->where('user_id', $user->id)->count();
        $billsCount = DB::table('bills')->where('user_id', $user->id)->count();

        return response()->json([
            'can_delete' => $openBills->isEmpty(),
            'open_bills' => $openBills,
            'custom_cocktails_count' => $customCocktailsCount,
            'reservations_count' => $reservationsCount,
            'bills_count' => $billsCount,
        ]);
    }

    // Admin: felhasználó törlése (cascading, with financial safeguard)
    public function deleteUser(Request $request, $id)
    {
        if (!$request->user()->is_admin) {
            return response()->json(['message' => 'Hozzáférés megtagadva'], 403);
        }

        // Ne törölje saját magát
        if ($request->user()->id == $id) {
            return response()->json(['message' => 'Nem törölheted saját magad'], 400);
        }

        $user = User::findOrFail($id);
        $forceClose = $request->query('force_close_bills') === 'true';

        // Check for open invoices
        $openBills = DB::table('open_bills')->where('user_id', $user->id)->get();

        if ($openBills->isNotEmpty() && !$forceClose) {
            return response()->json([
                'message' => 'A felhasználónak nyitott számlája van. Először rendezze a számlát!',
                'error' => 'open_invoices',
            ], 409);
        }

        DB::transaction(function () use ($user, $openBills, $forceClose) {
            // 0. Force-close open bills if requested
            if ($forceClose && $openBills->isNotEmpty()) {
                foreach ($openBills as $ob) {
                    $total = DB::table('orders')->where('bills_id', $ob->id)->sum('subtotal');

                    // Archive to bills
                    $billId = DB::table('bills')->insertGetId([
                        'box_id' => $ob->box_id,
                        'user_id' => $user->id,
                        'total' => $total,
                        'paid_at' => now(),
                        'paid_with' => 'Cash',
                    ]);

                    // Link orders to the paid bill
                    DB::table('orders')->where('bills_id', $ob->id)->update([
                        'paid_bill_id' => $billId,
                        'status' => 'served',
                    ]);

                    // Delete the open bill
                    DB::table('open_bills')->where('id', $ob->id)->delete();
                }
            }

            // 1. Cascade: custom cocktails + pivot tables
            $ccIds = $user->customCocktails()->pluck('id');
            if ($ccIds->isNotEmpty()) {
                DB::table('custom_cocktail_drinks')->whereIn('custom_cocktail_id', $ccIds)->delete();
                DB::table('custom_cocktail_ingredients')->whereIn('custom_cocktail_id', $ccIds)->delete();
                $user->customCocktails()->delete();
            }

            // 2. Delete reservations & nullify bills (preserve financial history)
            DB::table('reservations')->where('user_id', $user->id)->delete();
            DB::table('bills')->where('user_id', $user->id)->update(['user_id' => null]);

            // 3. Revoke all tokens
            $user->tokens()->delete();

            // 4. Delete user
            $user->delete();
        });

        return response()->json(['message' => 'Felhasználó és összes kapcsolódó adat törölve']);
    }
}