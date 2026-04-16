<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\HasMany;

class Order extends Model
{
    public $timestamps = true;
    const UPDATED_AT = null;

    protected $fillable = ['item_id', 'quantity', 'subtotal', 'status', 'bills_id', 'paid_bill_id'];

    public function drink(): BelongsTo
    {
        return $this->belongsTo(Drink::class, 'item_id');
    }

    public function openBill(): BelongsTo
    {
        return $this->belongsTo(OpenBill::class, 'bills_id');
    }

    public function paidBill(): BelongsTo
    {
        return $this->belongsTo(Bill::class, 'paid_bill_id');
    }

    public function extras(): HasMany
    {
        return $this->hasMany(OrderExtra::class);
    }
}