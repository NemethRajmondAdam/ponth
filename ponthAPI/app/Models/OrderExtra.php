<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;

class OrderExtra extends Model
{
    protected $table = 'orders_extra';
    public $timestamps = false;
    protected $fillable = ['order_id', 'item_id', 'type', 'box_detail_id', 'quantity', 'price'];

    public function order(): BelongsTo
    {
        return $this->belongsTo(Order::class);
    }

    public function boxDetail(): BelongsTo
    {
        return $this->belongsTo(BoxDetail::class, 'box_detail_id');
    }
}
