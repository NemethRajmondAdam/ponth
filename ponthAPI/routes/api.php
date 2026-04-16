<?php

use App\Http\Controllers\UsersController;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\Api\{CocktailController, CustomCocktailController, DrinkController, OrderController, ReservationController, BoxController, IngredientController};

// Publikus
Route::post('/register', [UsersController::class , 'register']);
Route::post('/login', [UsersController::class , 'login']);
Route::get('/drinks', [DrinkController::class , 'index']);
Route::get('/boxes', [BoxController::class , 'index']);

// Védett
Route::middleware('auth:sanctum')->group(function () {
    Route::apiResource('cocktails', CocktailController::class)->except(['index']);
    Route::apiResource('custom-cocktails', CustomCocktailController::class);
    Route::post('/orders', [OrderController::class , 'store']);
    Route::post('/check-bill/{boxId}', [OrderController::class , 'checkBill']);
    Route::post('/mobile-order', [OrderController::class , 'mobileStore']);
    Route::get('/my-open-bill', [OrderController::class , 'myOpenBill']);
    Route::post('/checkout', [OrderController::class , 'checkout']);
    Route::get('/my-bills', [OrderController::class , 'myBills']);
    Route::get('/my-custom-cocktails', [CustomCocktailController::class , 'index']);
    Route::get('/reservations/unavailable', [ReservationController::class , 'unavailableSlots']);
    Route::apiResource('reservations', ReservationController::class);
    Route::apiResource('boxes', BoxController::class)->except(['index']);
    Route::apiResource('ingredients', IngredientController::class);
    Route::post('/logout', [UsersController::class , 'logout']);

    // Admin routes
    Route::get('/users', [UsersController::class , 'listUsers']);
    Route::put('/users/{id}', [UsersController::class , 'updateUser']);
    Route::get('/users/{id}/delete-check', [UsersController::class, 'deleteCheck']);
    Route::delete('/users/{id}', [UsersController::class, 'deleteUser']);
});

/*Route::get('/user', function (Request $request) {
 return $request->user(); })->middleware('auth:sanctum');*/
