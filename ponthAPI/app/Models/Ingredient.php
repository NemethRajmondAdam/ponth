<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;

class Ingredient extends Model
{
    public $timestamps = false;
    protected $fillable = ['name', 'quantity_id', 'price'];

    public function quantityUnit(): BelongsTo
    {
        return $this->belongsTo(Quantity::class, 'quantity_id');
    }
}
