<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\BelongsToMany;

class Cocktail extends Model
{
    public $timestamps = false;
    protected $fillable = ['drink_id', 'name'];

    public function baseDrink(): BelongsTo
    {
        return $this->belongsTo(Drink::class, 'drink_id');
    }

    public function drinks(): BelongsToMany
    {
        return $this->belongsToMany(Drink::class, 'cocktail_drinks')
                    ->withPivot('quantity');
    }

    public function ingredients(): BelongsToMany
    {
        return $this->belongsToMany(Ingredient::class, 'cocktail_ingredients')
                    ->withPivot('quantity');
    }
}
