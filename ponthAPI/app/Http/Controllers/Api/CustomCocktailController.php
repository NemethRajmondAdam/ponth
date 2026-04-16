<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Models\CustomCocktail;
use App\Models\Drink;
use App\Models\Ingredient;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Validation\Rule;

class CustomCocktailController extends Controller {

    /**
     * List the authenticated user's custom cocktails.
     */
    public function index(Request $request) {
        return $request->user()
            ->customCocktails()
            ->with(['drinks.mixingQuantity', 'ingredients.quantityUnit'])
            ->get();
    }

    /**
     * Create a new custom cocktail. Price is auto-calculated.
     */
    public function store(Request $request) {
        $request->validate([
            'cocktail_name' => [
                'required', 'string', 'max:100',
                Rule::unique('custom_cocktail')->where('user_id', $request->user()->id)
            ],
            'drinks' => 'sometimes|array',
            'drinks.*.id' => 'required_with:drinks|exists:drinks,id',
            'drinks.*.quantity' => 'required_with:drinks|integer|min:1',
            'ingredients' => 'sometimes|array',
            'ingredients.*.id' => 'required_with:ingredients|exists:ingredients,id',
            'ingredients.*.quantity' => 'required_with:ingredients|integer|min:1',
        ]);

        return DB::transaction(function () use ($request) {
            // Calculate price
            $price = 0;

            $drinkSync = [];
            if ($request->has('drinks')) {
                foreach ($request->drinks as $d) {
                    $drink = Drink::findOrFail($d['id']);
                    $price += $drink->mixing_price * $d['quantity'];
                    $drinkSync[$d['id']] = ['quantity' => $d['quantity']];
                }
            }

            $ingredientSync = [];
            if ($request->has('ingredients')) {
                foreach ($request->ingredients as $i) {
                    $ingredient = Ingredient::findOrFail($i['id']);
                    $price += $ingredient->price * $i['quantity'];
                    $ingredientSync[$i['id']] = ['quantity' => $i['quantity']];
                }
            }

            $custom = $request->user()->customCocktails()->create([
                'cocktail_name' => $request->cocktail_name,
                'price' => $price,
            ]);

            $custom->drinks()->sync($drinkSync);
            $custom->ingredients()->sync($ingredientSync);

            return response()->json(
                $custom->load(['drinks.mixingQuantity', 'ingredients.quantityUnit']),
                201
            );
        });
    }

    /**
     * Update an existing custom cocktail.
     */
    public function update(Request $request, $id) {
        $custom = CustomCocktail::findOrFail($id);

        if ($custom->user_id !== $request->user()->id) {
            return response()->json(['error' => 'Unauthorized'], 403);
        }

        $request->validate([
            'cocktail_name' => [
                'sometimes', 'string', 'max:100',
                Rule::unique('custom_cocktail')->where('user_id', $request->user()->id)->ignore($id)
            ],
            'drinks' => 'sometimes|array',
            'drinks.*.id' => 'required_with:drinks|exists:drinks,id',
            'drinks.*.quantity' => 'required_with:drinks|integer|min:1',
            'ingredients' => 'sometimes|array',
            'ingredients.*.id' => 'required_with:ingredients|exists:ingredients,id',
            'ingredients.*.quantity' => 'required_with:ingredients|integer|min:1',
        ]);

        return DB::transaction(function () use ($request, $custom) {
            if ($request->has('cocktail_name')) {
                $custom->cocktail_name = $request->cocktail_name;
            }

            // Recalculate price
            $price = 0;

            if ($request->has('drinks')) {
                $drinkSync = [];
                foreach ($request->drinks as $d) {
                    $drink = Drink::findOrFail($d['id']);
                    $price += $drink->mixing_price * $d['quantity'];
                    $drinkSync[$d['id']] = ['quantity' => $d['quantity']];
                }
                $custom->drinks()->sync($drinkSync);
            } else {
                // Keep existing drinks in price
                foreach ($custom->drinks as $d) {
                    $price += $d->mixing_price * $d->pivot->quantity;
                }
            }

            if ($request->has('ingredients')) {
                $ingredientSync = [];
                foreach ($request->ingredients as $i) {
                    $ingredient = Ingredient::findOrFail($i['id']);
                    $price += $ingredient->price * $i['quantity'];
                    $ingredientSync[$i['id']] = ['quantity' => $i['quantity']];
                }
                $custom->ingredients()->sync($ingredientSync);
            } else {
                foreach ($custom->ingredients as $i) {
                    $price += $i->price * $i->pivot->quantity;
                }
            }

            $custom->price = $price;
            $custom->save();

            return response()->json(
                $custom->load(['drinks.mixingQuantity', 'ingredients.quantityUnit'])
            );
        });
    }

    /**
     * Delete a custom cocktail.
     */
    public function destroy($id, Request $request) {
        $item = CustomCocktail::findOrFail($id);
        if ($item->user_id !== $request->user()->id) {
            return response()->json(['error' => 'Unauthorized'], 403);
        }
        $item->drinks()->detach();
        $item->ingredients()->detach();
        $item->delete();
        return response()->json(null, 204);
    }
}
