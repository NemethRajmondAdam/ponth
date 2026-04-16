<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\HasMany;

class BoxDetail extends Model
{
    protected $table = 'boxes_details'; // Többes szám az SQL alapján
    public $timestamps = false;
    protected $fillable = ['box_id', 'sum'];

    public function box(): BelongsTo
    {
        return $this->belongsTo(Box::class, 'box_id');
    }

    public function orders(): HasMany
    {
        return $this->hasMany(Order::class, 'box_detail_id');
    }
}