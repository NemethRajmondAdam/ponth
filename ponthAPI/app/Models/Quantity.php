<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Quantity extends Model
{
    protected $table = 'quantities';
    public $timestamps = false;
    protected $fillable = ['quantity'];
}
