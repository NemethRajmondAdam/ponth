<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\HasMany;

class OpenBill extends Model
{
    protected $table = 'open_bills';
    public $timestamps = false;
    protected $fillable = ['user_id', 'box_id', 'totalSum'];

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
        return $this->hasMany(Order::class, 'bills_id');
    }
}
