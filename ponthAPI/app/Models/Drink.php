<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\HasOne;

class Drink extends Model
{
    public $timestamps = false;
    protected $fillable = ['type', 'name', 'price', 'quantity_id', 'mixing_price', 'mixing_quantity_id'];

    public function quantity(): BelongsTo
    {
        return $this->belongsTo(Quantity::class , 'quantity_id');
    }

    public function mixingQuantity(): BelongsTo
    {
        return $this->belongsTo(Quantity::class , 'mixing_quantity_id');
    }

    public function cocktail(): HasOne
    {
        return $this->hasOne(Cocktail::class , 'drink_id');
    }
}
