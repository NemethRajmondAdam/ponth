<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;

class Reservation extends Model
{
    public $timestamps = false;
    protected $fillable = [
        'boxes_id', 'reservation_date', 'reservation_time',
        'reserved_at', 'duration_minutes', 'name',
        'phone', 'email', 'status', 'user_id'
    ];

    public function box(): BelongsTo
    {
        return $this->belongsTo(Box::class , 'boxes_id');
    }

    public function user(): BelongsTo
    {
        return $this->belongsTo(User::class);
    }
}
