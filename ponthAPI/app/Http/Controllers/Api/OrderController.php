<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Models\Order;
use App\Models\Drink;
use App\Models\Bill;
use App\Models\OpenBill;
use App\Models\OrderExtra;
use App\Models\CustomCocktail;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;

class OrderController extends Controller
{
    /**
     * Check/create an open bill after QR scan.
     * - No open bill → create one
     * - Same box → continue
     * - Different box → 409 error
     */
    public function checkBill(Request $request, $boxId)
    {
        $user = $request->user();

        $openBill = OpenBill::where('user_id', $user->id)->first();

        if (!$openBill) {
            // Create a new open bill
            $openBill = OpenBill::create([
                'user_id' => $user->id,
                'box_id' => $boxId,
                'totalSum' => 0,
            ]);

            return response()->json([
                'status' => 'opened',
                'bill' => $openBill->load('box'),
            ], 201);
        }

        if ($openBill->box_id == $boxId) {
            return response()->json([
                'status' => 'existing',
                'bill' => $this->billWithCocktailNames($openBill->load('box', 'orders.drink', 'orders.extras')),
            ]);
        }

        // Different box — unresolved bill
        $openBill->load('box');
        return response()->json([
            'error' => "You have an unresolved bill at Box {$openBill->box->number}. Please pay it first.",
            'bill' => $openBill,
        ], 409);
    }

    /**
     * Place orders on the user's active open bill.
     */
    public function mobileStore(Request $request)
    {
        $request->validate([
            'items' => 'required|array|min:1',
            'items.*.drink_id' => 'sometimes|exists:drinks,id',
            'items.*.custom_cocktail_id' => 'sometimes|exists:custom_cocktail,id',
            'items.*.quantity' => 'required|integer|min:1',
            'items.*.extras' => 'sometimes|array',
            'items.*.extras.*.type' => 'required_with:items.*.extras|in:Drink,Ingredient',
            'items.*.extras.*.item_id' => 'required_with:items.*.extras|integer',
            'items.*.extras.*.delta' => 'required_with:items.*.extras|integer',
        ]);

        $user = $request->user();
        $openBill = OpenBill::where('user_id', $user->id)->first();

        if (!$openBill) {
            return response()->json([
                'error' => 'No open bill found. Please scan a QR code first.',
            ], 400);
        }

        return DB::transaction(function () use ($request, $openBill) {
            $orders = [];
            $totalAdded = 0;

            foreach ($request->items as $item) {
                // Custom cocktail order
                if (!empty($item['custom_cocktail_id'])) {
                    $custom = \App\Models\CustomCocktail::findOrFail($item['custom_cocktail_id']);
                    $subtotal = $custom->price * $item['quantity'];

                    $order = Order::create([
                        'item_id' => 1, // CostumeCocktail placeholder drink
                        'quantity' => $item['quantity'],
                        'subtotal' => $subtotal,
                        'status' => 'new',
                        'bills_id' => $openBill->id,
                    ]);

                    // Store custom cocktail reference in orders_extra (type='' as marker)
                    OrderExtra::create([
                        'order_id' => $order->id,
                        'item_id'  => $custom->id,
                        'type'     => '',
                        'box_detail_id' => $openBill->box_id,
                        'quantity' => 0,
                        'price'    => 0,
                    ]);

                    $orders[] = $order;
                    $totalAdded += $subtotal;
                    continue;
                }

                // Standard drink order
                $drink = Drink::findOrFail($item['drink_id']);
                $extrasCost = 0;

                // Calculate extras cost (only charge for positive deltas = added extras)
                if (!empty($item['extras'])) {
                    foreach ($item['extras'] as $extra) {
                        if ($extra['delta'] > 0) {
                            if ($extra['type'] === 'Drink') {
                                $extraDrink = Drink::findOrFail($extra['item_id']);
                                $extrasCost += $extraDrink->mixing_price * $extra['delta'];
                            } else {
                                $ingredient = \App\Models\Ingredient::findOrFail($extra['item_id']);
                                $extrasCost += $ingredient->price * $extra['delta'];
                            }
                        }
                    }
                }

                $subtotal = ($drink->price + $extrasCost) * $item['quantity'];

                $order = Order::create([
                    'item_id' => $item['drink_id'],
                    'quantity' => $item['quantity'],
                    'subtotal' => $subtotal,
                    'status' => 'new',
                    'bills_id' => $openBill->id,
                ]);

                // Save extras to orders_extra
                if (!empty($item['extras'])) {
                    foreach ($item['extras'] as $extra) {
                        if ($extra['delta'] != 0) {
                            $price = 0;
                            if ($extra['type'] === 'Drink') {
                                $extraDrink = Drink::find($extra['item_id']);
                                $price = $extraDrink ? $extraDrink->mixing_price * max(0, $extra['delta']) : 0;
                            } else {
                                $ingredient = \App\Models\Ingredient::find($extra['item_id']);
                                $price = $ingredient ? $ingredient->price * max(0, $extra['delta']) : 0;
                            }
                            OrderExtra::create([
                                'order_id' => $order->id,
                                'item_id' => $extra['item_id'],
                                'type' => $extra['type'],
                                'box_detail_id' => $openBill->box_id,
                                'quantity' => $extra['delta'],
                                'price' => $price,
                            ]);
                        }
                    }
                }

                $orders[] = $order;
                $totalAdded += $subtotal;
            }

            // Update the open bill total
            $openBill->increment('totalSum', $totalAdded);

            return response()->json([
                'message' => 'Order placed successfully!',
                'orders' => $orders,
                'billTotal' => $openBill->fresh()->totalSum,
            ], 201);
        });
    }

    /**
     * Get the user's active open bill with all orders.
     */
    public function myOpenBill(Request $request)
    {
        $user = $request->user();

        $openBill = OpenBill::with(['box', 'orders.drink', 'orders.extras'])
            ->where('user_id', $user->id)
            ->first();

        if (!$openBill) {
            return response()->json(['bill' => null]);
        }

        $this->resolveCustomCocktailNames($openBill->orders);

        return response()->json(['bill' => $openBill]);
    }

    /**
     * Checkout: pay the open bill, archive to bills, clean up.
     */
    public function checkout(Request $request)
    {
        $request->validate([
            'paid_with' => 'required|in:Card,Cash',
        ]);

        $user = $request->user();
        $openBill = OpenBill::where('user_id', $user->id)->first();

        if (!$openBill) {
            return response()->json([
                'error' => 'No open bill to pay.',
            ], 400);
        }

        return DB::transaction(function () use ($request, $user, $openBill) {
            // Calculate actual total from orders
            $total = Order::where('bills_id', $openBill->id)->sum('subtotal');

            // Create the archived bill
            $bill = Bill::create([
                'box_id' => $openBill->box_id,
                'user_id' => $user->id,
                'total' => $total,
                'paid_at' => now(),
                'paid_with' => $request->paid_with,
            ]);

            // Link orders to the paid bill and set status
            Order::where('bills_id', $openBill->id)
                ->update([
                'paid_bill_id' => $bill->id,
                'status' => 'served',
            ]);

            // Delete the open bill
            $openBill->delete();

            return response()->json([
                'message' => 'Bill paid successfully!',
                'bill' => $bill->load('box'),
            ]);
        });
    }

    /**
     * Get the user's paid bill history.
     */
    public function myBills(Request $request)
    {
        $user = $request->user();

        $bills = Bill::with(['box', 'orders.drink', 'orders.extras'])
            ->where('user_id', $user->id)
            ->orderBy('paid_at', 'desc')
            ->get();

        foreach ($bills as $bill) {
            $this->resolveCustomCocktailNames($bill->orders);
        }

        return response()->json($bills);
    }

    /**
     * Legacy store method (non-mobile ordering).
     */
    public function store(Request $request)
    {
        $request->validate([
            'box_detail_id' => 'required|exists:boxes_details,id',
            'item_id' => 'required|exists:drinks,id',
            'quantity' => 'required|integer|min:1'
        ]);

        return DB::transaction(function () use ($request) {
            $drink = Drink::findOrFail($request->item_id);
            $subtotal = $drink->price * $request->quantity;

            $order = Order::updateOrCreate(
            ['box_detail_id' => $request->box_detail_id, 'item_id' => $request->item_id],
            ['quantity' => DB::raw("quantity + $request->quantity"), 'subtotal' => DB::raw("subtotal + $subtotal")]
            );

            return response()->json($order, 201);
        });
    }

    /**
     * For orders with item_id=1 (CostumeCocktail placeholder),
     * resolve the actual custom cocktail name from the orders_extra marker.
     */
    private function resolveCustomCocktailNames($orders)
    {
        $ccIds = [];
        foreach ($orders as $order) {
            if ($order->item_id == 1) {
                $marker = $order->extras->firstWhere('type', '');
                if ($marker) {
                    $ccIds[] = $marker->item_id;
                }
            }
        }

        if (empty($ccIds)) return;

        $names = CustomCocktail::whereIn('id', $ccIds)->pluck('cocktail_name', 'id');

        foreach ($orders as $order) {
            if ($order->item_id == 1) {
                $marker = $order->extras->firstWhere('type', '');
                if ($marker && isset($names[$marker->item_id])) {
                    $order->setAttribute('custom_cocktail_name', $names[$marker->item_id]);
                }
            }
        }
    }

    /**
     * Helper to resolve cocktail names on a single bill and return it.
     */
    private function billWithCocktailNames($bill)
    {
        $this->resolveCustomCocktailNames($bill->orders);
        return $bill;
    }
}
