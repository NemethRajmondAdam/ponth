<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Models\Cocktail;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;

class CocktailController extends Controller {
    public function index() {
        return Cocktail::with(['drinks', 'ingredients'])->get();
    }

    public function store(Request $request) {
        $validated = $request->validate([
            'name' => 'required|string|unique:cocktails,name',
            'drink_id' => 'required|exists:drinks,id',
            'drinks' => 'array',
            'ingredients' => 'array'
        ]);

        return DB::transaction(function () use ($validated, $request) {
            $cocktail = Cocktail::create($validated);
            if ($request->has('drinks')) $cocktail->drinks()->sync($request->drinks);
            if ($request->has('ingredients')) $cocktail->ingredients()->sync($request->ingredients);
            return response()->json($cocktail->load(['drinks', 'ingredients']), 201);
        });
    }

    public function update(Request $request, $id) {
        $cocktail = Cocktail::findOrFail($id);
        $request->validate(['name' => 'string|unique:cocktails,name,' . $id]);

        DB::transaction(function () use ($request, $cocktail) {
            $cocktail->update($request->only(['name', 'drink_id']));
            if ($request->has('drinks')) $cocktail->drinks()->sync($request->drinks);
            if ($request->has('ingredients')) $cocktail->ingredients()->sync($request->ingredients);
        });
        return response()->json($cocktail->load(['drinks', 'ingredients']));
    }

    public function destroy($id) {
        Cocktail::destroy($id);
        return response()->json(null, 204);
    }
}
