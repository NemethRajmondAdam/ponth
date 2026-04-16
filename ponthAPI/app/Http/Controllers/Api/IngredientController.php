<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Models\Ingredient;
use App\Models\Quantity;
use Illuminate\Http\Request;

class IngredientController extends Controller {
    public function index() { return Ingredient::with('quantityUnit')->get(); }
    public function store(Request $request) {
        return Ingredient::create($request->validate([
            'name' => 'required|string|unique:ingredients,name',
            'quantity_id' => 'required|exists:quantities,id',
            'price' => 'required|integer'
        ]));
    }
}

class QuantityController extends Controller {
    public function index() { return Quantity::all(); }
}
