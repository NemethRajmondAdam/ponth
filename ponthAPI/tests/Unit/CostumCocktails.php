<?php
namespace Tests\Feature;

use Tests\TestCase;
use App\Models\User;
use App\Models\Drink;
use App\Models\Ingredient;
use App\Models\CustomCocktail;
use Illuminate\Foundation\Testing\RefreshDatabase;

class CustomCocktailTest extends TestCase
{
    use RefreshDatabase;

    /**
     * Egyedi koktél létrehozásának és az árkalkulációnak a tesztelése.
     */
    public function test_custom_cocktail_creation_and_price_calculation()
    {
        $user = User::factory()->create();
        $drink = Drink::create(['name' => 'Vodka', 'mixing_price' => 500]);
        $ingredient = Ingredient::create(['name' => 'Lime', 'price' => 150]);

        $response = $this->actingAs($user)->postJson('/api/custom-cocktails', [
            'cocktail_name' => 'Teszt Koktél',
            'drinks' => [['id' => $drink->id, 'quantity' => 2]], // 2 * 500 = 1000
            'ingredients' => [['id' => $ingredient->id, 'quantity' => 1]] // 1 * 150 = 150
        ]);

        $response->assertStatus(201);
        // Elvárt ár: 1150
        $this->assertDatabaseHas('custom_cocktail', [
            'cocktail_name' => 'Teszt Koktél',
            'price' => 1150,
            'user_id' => $user->id
        ]);
    }

    /**
     * Idegen koktél törlésének megakadályozása.
     */
    public function test_cannot_delete_someone_elses_cocktail()
    {
        $user1 = User::factory()->create();
        $user2 = User::factory()->create();
        $cocktail = CustomCocktail::create([
            'cocktail_name' => 'User1 Itala',
            'user_id' => $user1->id,
            'price' => 1000
        ]);

        $response = $this->actingAs($user2)->deleteJson("/api/custom-cocktails/{$cocktail->id}");

        $response->assertStatus(403);
    }
}