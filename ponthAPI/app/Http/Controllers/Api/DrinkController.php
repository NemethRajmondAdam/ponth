<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Models\Drink;
use Illuminate\Http\Request;

class DrinkController extends Controller
{
    public function index()
    {
        return Drink::with(['quantity', 'mixingQuantity', 'cocktail.ingredients.quantityUnit', 'cocktail.drinks.mixingQuantity'])->get();
    }

    public function store(Request $request)
    {
        $validated = $request->validate([
            'name' => 'required|string|unique:drinks,name',
            'type' => 'required|in:Cocktail,Alcohol,Softdrink',
            'price' => 'required|integer',
            'quantity_id' => 'required|exists:quantities,id',
            'mixing_price' => 'required|integer',
            'mixing_quantity_id' => 'required|exists:quantities,id'
        ]);
        return Drink::create($validated);
    }

    public function update(Request $request, $id)
    {
        $drink = Drink::findOrFail($id);
        $drink->update($request->all());
        return $drink;
    }

    public function destroy($id)
    {
        Drink::destroy($id);
        return response()->json(null, 204);
    }
}