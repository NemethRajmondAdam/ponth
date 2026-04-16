<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Models\Box;
use App\Models\BoxDetail;
use Illuminate\Http\Request;

class BoxController extends Controller
{
    public function index()
    {
        return Box::all();
    }

    // Új asztal nyitása (Box + BoxDetail létrehozása)
    public function openTable(Request $request)
    {
        $request->validate(['box_id' => 'required|exists:boxes,id']);
        return BoxDetail::create(['box_id' => $request->box_id, 'sum' => 0]);
    }
}
