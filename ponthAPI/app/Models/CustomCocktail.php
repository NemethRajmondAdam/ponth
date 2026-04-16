<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\BelongsToMany;

class CustomCocktail extends Model
{
    protected $table = 'custom_cocktail';
    public $timestamps = false; // Nincs az SQL-ben timestamp

    protected $fillable = ['cocktail_name', 'user_id', 'price', 'cocktail_id'];

    public function user(): BelongsTo
    {
        return $this->belongsTo(User::class);
    }

    public function baseCocktail(): BelongsTo
    {
        return $this->belongsTo(Cocktail::class, 'cocktail_id');
    }

    // Kapcsolat az extra italokkal
    public function drinks(): BelongsToMany
    {
        return $this->belongsToMany(Drink::class, 'custom_cocktail_drinks', 'custom_cocktail_id', 'drink_id')
                    ->withPivot('quantity');
    }

    // Kapcsolat az extra összetevőkkel
    public function ingredients(): BelongsToMany
    {
        return $this->belongsToMany(Ingredient::class, 'custom_cocktail_ingredients', 'custom_cocktail_id', 'ingredient_id')
                    ->withPivot('quantity');
    }
}
