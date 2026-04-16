<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\HasMany;

class Bill extends Model
{
    protected $table = 'bills';
    public $timestamps = false;
    protected $fillable = ['box_id', 'user_id', 'total', 'paid_at', 'paid_with'];

    public function user(): BelongsTo
    {
        return $this->belongsTo(User::class);
    }

    public function box(): BelongsTo
    {
        return $this->belongsTo(Box::class);
    }

    public function orders(): HasMany
    {
        return $this->hasMany(Order::class, 'paid_bill_id');
    }
}
