<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\HasMany;

class Box extends Model
{
    public $timestamps = false;
    protected $fillable = ['number', 'seats', 'online'];

    public function reservations(): HasMany
    {
        return $this->hasMany(Reservation::class, 'boxes_id');
    }

    public function details(): HasMany
    {
        return $this->hasMany(BoxDetail::class, 'box_id');
    }
}
