<?php
namespace Tests\Feature;

use Tests\TestCase;
use App\Models\User;
use Illuminate\Support\Facades\DB;
use Illuminate\Foundation\Testing\RefreshDatabase;

class UserManagementTest extends TestCase
{
    use RefreshDatabase;

    /**
     * Regisztráció validációjának és a jelszó titkosításának tesztelése.
     */
    public function test_successful_registration_and_password_hashing()
    {
        $response = $this->postJson('/api/register', [
            'name' => 'Teszt Felhasználó',
            'email' => 'teszt@gmail.com',
            'password' => '123456',
            'password_confirmation' => '123456',
        ]);

        $response->assertStatus(201)
                 ->assertJsonStructure(['access_token', 'user']);

        $this->assertDatabaseHas('users', ['email' => 'teszt@gmail.hu']);
    }

    /**
     * Pénzügyi biztonsági korlát: Felhasználó nem törölhető nyitott számlával.
     */
    public function test_cannot_delete_user_with_open_bill()
    {
        $admin = User::factory()->create(['is_admin' => true]);
        $user = User::factory()->create();

        // Nyitott számla rögzítése az adatbázisban
        DB::table('open_bills')->insert([
            'user_id' => $user->id,
            'totalSum' => 1200,
            'box_id' => 1
        ]);

        $response = $this->actingAs($admin)
                         ->deleteJson("/api/users/{$user->id}");

        $response->assertStatus(409)
                 ->assertJson(['error' => 'open_invoices']);
    }
}