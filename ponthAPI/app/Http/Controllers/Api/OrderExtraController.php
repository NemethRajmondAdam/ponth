<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Models\OrderExtra;
use App\Models\BoxDetail;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;

class OrderExtraController extends Controller {
    public function store(Request $request) {
        return DB::transaction(function () use ($request) {
            $extra = OrderExtra::create($request->validate([
                'order_id' => 'required|exists:orders,id',
                'item_id' => 'required|exists:drinks,id',
                'box_detail_id' => 'required|exists:boxes_details,id',
                'quantity' => 'required|integer',
                'price' => 'required|integer'
            ]));

            BoxDetail::find($request->box_detail_id)->increment('sum', $request->price * $request->quantity);
            return $extra;
        });
    }
}