-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2026. Ápr 14. 13:27
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `ponth`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `bills`
--

CREATE TABLE `bills` (
  `id` int(11) NOT NULL,
  `box_id` int(11) NOT NULL,
  `total` int(11) NOT NULL,
  `paid_at` datetime NOT NULL,
  `paid_with` enum('Card','Cash','','') NOT NULL,
  `user_id` bigint(20) UNSIGNED DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `boxes`
--

CREATE TABLE `boxes` (
  `id` int(11) NOT NULL,
  `number` int(11) DEFAULT NULL,
  `seats` int(11) DEFAULT NULL,
  `online` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `boxes`
--

INSERT INTO `boxes` (`id`, `number`, `seats`, `online`) VALUES
(1, 1, 4, 1),
(2, 2, 4, 1),
(3, 3, 2, 1),
(4, 4, 5, 1),
(5, 5, 4, 1);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `cache`
--

CREATE TABLE `cache` (
  `key` varchar(255) NOT NULL,
  `value` mediumtext NOT NULL,
  `expiration` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `cache_locks`
--

CREATE TABLE `cache_locks` (
  `key` varchar(255) NOT NULL,
  `owner` varchar(255) NOT NULL,
  `expiration` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `cocktails`
--

CREATE TABLE `cocktails` (
  `id` int(11) NOT NULL,
  `drink_id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `cocktails`
--

INSERT INTO `cocktails` (`id`, `drink_id`, `name`) VALUES
(1, 1, 'CostumeCocktail'),
(2, 18, 'Mojito'),
(3, 19, 'Cosmopolitan'),
(4, 20, 'Gin Tonic'),
(5, 21, 'Pina Colada'),
(6, 22, 'Sex on the Beach'),
(7, 23, 'Tequila Sunrise'),
(8, 24, 'Long Island Iced Tea'),
(9, 25, 'Negroni'),
(10, 26, 'Whiskey Sour'),
(11, 27, 'Aperol Spritz'),
(12, 28, 'Cuba Libre'),
(13, 29, 'Blue Lagoon'),
(14, 30, 'Mai Tai'),
(15, 31, 'Vodka Lemon');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `cocktail_drinks`
--

CREATE TABLE `cocktail_drinks` (
  `id` int(11) NOT NULL,
  `cocktail_id` int(11) NOT NULL,
  `drink_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `cocktail_drinks`
--

INSERT INTO `cocktail_drinks` (`id`, `cocktail_id`, `drink_id`, `quantity`) VALUES
(1, 2, 3, 5),
(2, 2, 15, 2),
(3, 2, 11, 10),
(4, 3, 2, 4),
(5, 3, 7, 2),
(6, 3, 14, 3),
(7, 4, 5, 5),
(8, 4, 10, 15),
(9, 5, 3, 5),
(10, 5, 13, 10),
(11, 6, 2, 4),
(12, 6, 12, 4),
(13, 6, 14, 4),
(14, 7, 4, 5),
(15, 7, 12, 10),
(16, 7, 17, 2),
(17, 8, 2, 2),
(18, 8, 3, 2),
(19, 8, 4, 2),
(20, 8, 5, 2),
(21, 8, 7, 2),
(22, 8, 10, 6),
(23, 9, 5, 3),
(24, 9, 8, 3),
(25, 10, 6, 5),
(26, 10, 15, 3),
(27, 10, 16, 2),
(28, 11, 8, 6),
(29, 11, 9, 9),
(30, 11, 11, 3),
(31, 12, 3, 5),
(32, 12, 10, 12),
(33, 12, 15, 1),
(34, 13, 2, 4),
(35, 13, 17, 2),
(36, 13, 11, 10),
(37, 14, 3, 4),
(38, 14, 7, 2),
(39, 14, 15, 2),
(40, 15, 2, 5),
(41, 15, 11, 10);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `cocktail_ingredients`
--

CREATE TABLE `cocktail_ingredients` (
  `id` int(11) NOT NULL,
  `cocktail_id` int(11) NOT NULL,
  `ingredient_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `cocktail_ingredients`
--

INSERT INTO `cocktail_ingredients` (`id`, `cocktail_id`, `ingredient_id`, `quantity`) VALUES
(1, 2, 1, 1),
(2, 2, 3, 1),
(3, 2, 8, 1),
(4, 3, 2, 1),
(5, 4, 5, 1),
(6, 5, 4, 1),
(7, 6, 5, 1),
(8, 7, 5, 1),
(9, 8, 7, 1),
(10, 10, 2, 1),
(11, 11, 5, 1),
(12, 12, 1, 1),
(13, 13, 7, 1),
(14, 14, 1, 1),
(15, 15, 2, 1);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `custom_cocktail`
--

CREATE TABLE `custom_cocktail` (
  `id` int(11) NOT NULL,
  `cocktail_name` varchar(100) NOT NULL,
  `user_id` bigint(20) UNSIGNED NOT NULL,
  `price` int(11) NOT NULL,
  `cocktail_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `custom_cocktail_drinks`
--

CREATE TABLE `custom_cocktail_drinks` (
  `id` int(11) NOT NULL,
  `custom_cocktail_id` int(11) NOT NULL,
  `drink_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `custom_cocktail_ingredients`
--

CREATE TABLE `custom_cocktail_ingredients` (
  `id` int(11) NOT NULL,
  `custom_cocktail_id` int(11) NOT NULL,
  `ingredient_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `drinks`
--

CREATE TABLE `drinks` (
  `id` int(11) NOT NULL,
  `type` enum('Cocktail','Alcohol','Softdrink','') NOT NULL,
  `name` varchar(50) NOT NULL,
  `price` int(11) NOT NULL,
  `quantity_id` int(11) NOT NULL,
  `mixing_price` int(11) NOT NULL,
  `mixing_quantity_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `drinks`
--

INSERT INTO `drinks` (`id`, `type`, `name`, `price`, `quantity_id`, `mixing_price`, `mixing_quantity_id`) VALUES
(1, 'Cocktail', 'CostumeCocktail', 0, 1, 0, 1),
(2, 'Alcohol', 'Vodka', 850, 1, 210, 1),
(3, 'Alcohol', 'White Rum', 900, 1, 225, 1),
(4, 'Alcohol', 'Tequila', 950, 1, 240, 1),
(5, 'Alcohol', 'Gin', 900, 1, 225, 1),
(6, 'Alcohol', 'Whiskey', 1100, 1, 275, 1),
(7, 'Alcohol', 'Triple Sec', 900, 1, 225, 1),
(8, 'Alcohol', 'Aperol', 1200, 1, 200, 1),
(9, 'Alcohol', 'Prosecco', 1200, 6, 120, 1),
(10, 'Softdrink', 'Cola', 450, 6, 25, 1),
(11, 'Softdrink', 'Soda Water', 300, 6, 15, 1),
(12, 'Softdrink', 'Orange Juice', 350, 6, 18, 1),
(13, 'Softdrink', 'Pineapple Juice', 350, 6, 18, 1),
(14, 'Softdrink', 'Cranberry Juice', 350, 6, 18, 1),
(15, 'Softdrink', 'Lime Juice', 300, 1, 40, 1),
(16, 'Softdrink', 'Sugar Syrup', 200, 1, 20, 1),
(17, 'Softdrink', 'Grenadine', 220, 1, 22, 1),
(18, 'Cocktail', 'Mojito', 2490, 1, 0, 1),
(19, 'Cocktail', 'Cosmopolitan', 2490, 1, 0, 1),
(20, 'Cocktail', 'Gin Tonic', 1990, 1, 0, 1),
(21, 'Cocktail', 'Pina Colada', 2590, 1, 0, 1),
(22, 'Cocktail', 'Sex on the Beach', 2490, 1, 0, 1),
(23, 'Cocktail', 'Tequila Sunrise', 2390, 1, 0, 1),
(24, 'Cocktail', 'Long Island Iced Tea', 3290, 1, 0, 1),
(25, 'Cocktail', 'Negroni', 2690, 1, 0, 1),
(26, 'Cocktail', 'Whiskey Sour', 2590, 1, 0, 1),
(27, 'Cocktail', 'Aperol Spritz', 2490, 1, 0, 1),
(28, 'Cocktail', 'Cuba Libre', 2190, 1, 0, 1),
(29, 'Cocktail', 'Blue Lagoon', 2290, 1, 0, 1),
(30, 'Cocktail', 'Mai Tai', 2690, 1, 0, 1),
(31, 'Cocktail', 'Vodka Lemon', 1990, 1, 0, 1);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `failed_jobs`
--

CREATE TABLE `failed_jobs` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `uuid` varchar(255) NOT NULL,
  `connection` text NOT NULL,
  `queue` text NOT NULL,
  `payload` longtext NOT NULL,
  `exception` longtext NOT NULL,
  `failed_at` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `ingredients`
--

CREATE TABLE `ingredients` (
  `id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `quantity_id` int(11) NOT NULL,
  `price` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `ingredients`
--

INSERT INTO `ingredients` (`id`, `name`, `quantity_id`, `price`) VALUES
(1, 'Lime', 3, 150),
(2, 'Lemon', 3, 150),
(3, 'Mint Leaves', 4, 50),
(4, 'Strawberry', 3, 120),
(5, 'Orange Slice', 3, 80),
(6, 'Sugar Cube', 3, 40),
(7, 'Ice', 4, 0),
(8, 'Crushed Ice', 4, 0);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `jobs`
--

CREATE TABLE `jobs` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `queue` varchar(255) NOT NULL,
  `payload` longtext NOT NULL,
  `attempts` tinyint(3) UNSIGNED NOT NULL,
  `reserved_at` int(10) UNSIGNED DEFAULT NULL,
  `available_at` int(10) UNSIGNED NOT NULL,
  `created_at` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `job_batches`
--

CREATE TABLE `job_batches` (
  `id` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `total_jobs` int(11) NOT NULL,
  `pending_jobs` int(11) NOT NULL,
  `failed_jobs` int(11) NOT NULL,
  `failed_job_ids` longtext NOT NULL,
  `options` mediumtext DEFAULT NULL,
  `cancelled_at` int(11) DEFAULT NULL,
  `created_at` int(11) NOT NULL,
  `finished_at` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `migrations`
--

CREATE TABLE `migrations` (
  `id` int(10) UNSIGNED NOT NULL,
  `migration` varchar(255) NOT NULL,
  `batch` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `open_bills`
--

CREATE TABLE `open_bills` (
  `id` int(11) NOT NULL,
  `user_id` bigint(20) UNSIGNED DEFAULT NULL,
  `totalSum` int(11) NOT NULL,
  `box_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `orders`
--

CREATE TABLE `orders` (
  `id` int(11) NOT NULL,
  `item_id` int(11) DEFAULT NULL,
  `quantity` int(11) DEFAULT NULL,
  `subtotal` int(11) DEFAULT NULL,
  `status` varchar(20) NOT NULL DEFAULT 'new',
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  `bills_id` int(11) DEFAULT NULL,
  `paid_bill_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `orders_extra`
--

CREATE TABLE `orders_extra` (
  `id` int(11) NOT NULL,
  `order_id` int(11) NOT NULL,
  `item_id` int(11) NOT NULL,
  `type` enum('Drink','Ingredient','','') NOT NULL,
  `box_detail_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL,
  `price` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `password_reset_tokens`
--

CREATE TABLE `password_reset_tokens` (
  `email` varchar(255) NOT NULL,
  `token` varchar(255) NOT NULL,
  `created_at` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `personal_access_tokens`
--

CREATE TABLE `personal_access_tokens` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `tokenable_type` varchar(255) NOT NULL,
  `tokenable_id` bigint(20) UNSIGNED NOT NULL,
  `name` text NOT NULL,
  `token` varchar(64) NOT NULL,
  `abilities` text DEFAULT NULL,
  `last_used_at` timestamp NULL DEFAULT NULL,
  `expires_at` timestamp NULL DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `quantities`
--

CREATE TABLE `quantities` (
  `id` int(11) NOT NULL,
  `quantity` varchar(5) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `quantities`
--

INSERT INTO `quantities` (`id`, `quantity`) VALUES
(1, 'cl'),
(2, 'ml'),
(3, 'piece'),
(4, 'half'),
(5, 'tsp'),
(6, 'dl');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `reservations`
--

CREATE TABLE `reservations` (
  `id` int(11) NOT NULL,
  `boxes_id` int(11) DEFAULT NULL,
  `reservation_date` date DEFAULT NULL,
  `reservation_time` time DEFAULT NULL,
  `reserved_at` datetime NOT NULL,
  `duration_minutes` int(11) NOT NULL,
  `name` varchar(50) DEFAULT NULL,
  `phone` varchar(20) DEFAULT NULL,
  `user_id` bigint(10) UNSIGNED DEFAULT NULL,
  `status` varchar(20) NOT NULL COMMENT 'pl.: -foglalva,-lemondva,-megerositve'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `sessions`
--

CREATE TABLE `sessions` (
  `id` varchar(255) NOT NULL,
  `user_id` bigint(20) UNSIGNED DEFAULT NULL,
  `ip_address` varchar(45) DEFAULT NULL,
  `user_agent` text DEFAULT NULL,
  `payload` longtext NOT NULL,
  `last_activity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `users`
--

CREATE TABLE `users` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `name` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `email_verified_at` timestamp NULL DEFAULT NULL,
  `password` varchar(255) NOT NULL,
  `remember_token` varchar(100) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  `is_admin` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- A tábla adatainak kiíratása `users`
--

INSERT INTO `users` (`id`, `name`, `email`, `email_verified_at`, `password`, `remember_token`, `created_at`, `updated_at`, `is_admin`) VALUES
(0, 'test', 'test@gmail.com', '2026-01-26 11:07:24', '$2y$12$fJb/cx.bwR7FWFfjbJ4px.V5Uy2SG6zxZWAM97hKTkUH0ebconoVW', 'hTx0aAyW9Q', '2026-01-26 11:07:25', '2026-01-26 11:07:25', 1),
(4, 'Vivike', 'vivike@gmail.com', NULL, '$2y$12$Bmgxml/Lhh3DCn2Gjn1RXONskBKstH282XBI6UrglkEBLAfl.1m1S', NULL, '2026-03-04 08:59:52', '2026-03-16 12:18:02', 0),
(5, 'Elek', 'testelek@gmail.com', NULL, '$2y$12$Rh.GzoxU9W6B6dhE1YovTeA50TNNNBOxHlGMvE5pKZ1gbcWB7op9e', NULL, '2026-03-16 11:39:25', '2026-03-16 11:39:25', 0),
(6, 'Példa Béla', 'peldabela@gmail.com', NULL, '$2y$12$PHRZX2gfzSTCF.PTVgexpOv88MywiVT/6OHYvBjwU1u8r7VHa5jP2', NULL, '2026-03-16 12:13:16', '2026-03-16 12:13:16', 0);

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `bills`
--
ALTER TABLE `bills`
  ADD PRIMARY KEY (`id`),
  ADD KEY `box_id` (`box_id`),
  ADD KEY `user_id` (`user_id`);

--
-- A tábla indexei `boxes`
--
ALTER TABLE `boxes`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `cache`
--
ALTER TABLE `cache`
  ADD PRIMARY KEY (`key`);

--
-- A tábla indexei `cache_locks`
--
ALTER TABLE `cache_locks`
  ADD PRIMARY KEY (`key`);

--
-- A tábla indexei `cocktails`
--
ALTER TABLE `cocktails`
  ADD PRIMARY KEY (`id`),
  ADD KEY `drinks_id` (`drink_id`),
  ADD KEY `drink_id` (`drink_id`);

--
-- A tábla indexei `cocktail_drinks`
--
ALTER TABLE `cocktail_drinks`
  ADD PRIMARY KEY (`id`),
  ADD KEY `cocktail_id` (`cocktail_id`),
  ADD KEY `drink_id` (`drink_id`);

--
-- A tábla indexei `cocktail_ingredients`
--
ALTER TABLE `cocktail_ingredients`
  ADD PRIMARY KEY (`id`),
  ADD KEY `cocktail_id` (`cocktail_id`),
  ADD KEY `ingredient_id` (`ingredient_id`);

--
-- A tábla indexei `custom_cocktail`
--
ALTER TABLE `custom_cocktail`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user_id` (`user_id`),
  ADD KEY `cocktail_id` (`cocktail_id`);

--
-- A tábla indexei `custom_cocktail_drinks`
--
ALTER TABLE `custom_cocktail_drinks`
  ADD PRIMARY KEY (`id`),
  ADD KEY `costume_cocktail_id` (`custom_cocktail_id`),
  ADD KEY `drink_id` (`drink_id`);

--
-- A tábla indexei `custom_cocktail_ingredients`
--
ALTER TABLE `custom_cocktail_ingredients`
  ADD PRIMARY KEY (`id`),
  ADD KEY `costume_cocktail_id` (`custom_cocktail_id`),
  ADD KEY `ingredient_id` (`ingredient_id`);

--
-- A tábla indexei `drinks`
--
ALTER TABLE `drinks`
  ADD PRIMARY KEY (`id`),
  ADD KEY `mixing_quantity_id` (`mixing_quantity_id`),
  ADD KEY `quantity_id` (`quantity_id`);

--
-- A tábla indexei `failed_jobs`
--
ALTER TABLE `failed_jobs`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `failed_jobs_uuid_unique` (`uuid`);

--
-- A tábla indexei `ingredients`
--
ALTER TABLE `ingredients`
  ADD PRIMARY KEY (`id`),
  ADD KEY `quantity_id` (`quantity_id`);

--
-- A tábla indexei `jobs`
--
ALTER TABLE `jobs`
  ADD PRIMARY KEY (`id`),
  ADD KEY `jobs_queue_index` (`queue`);

--
-- A tábla indexei `job_batches`
--
ALTER TABLE `job_batches`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `migrations`
--
ALTER TABLE `migrations`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `open_bills`
--
ALTER TABLE `open_bills`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user_id` (`user_id`),
  ADD KEY `box_id` (`box_id`);

--
-- A tábla indexei `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`id`),
  ADD KEY `item_id` (`item_id`);

--
-- A tábla indexei `orders_extra`
--
ALTER TABLE `orders_extra`
  ADD PRIMARY KEY (`id`),
  ADD KEY `item_id` (`item_id`),
  ADD KEY `box_id` (`box_detail_id`),
  ADD KEY `item_id_2` (`item_id`),
  ADD KEY `order_id` (`order_id`),
  ADD KEY `item_id_3` (`item_id`),
  ADD KEY `item_id_4` (`item_id`);

--
-- A tábla indexei `password_reset_tokens`
--
ALTER TABLE `password_reset_tokens`
  ADD PRIMARY KEY (`email`);

--
-- A tábla indexei `personal_access_tokens`
--
ALTER TABLE `personal_access_tokens`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `personal_access_tokens_token_unique` (`token`),
  ADD KEY `personal_access_tokens_tokenable_type_tokenable_id_index` (`tokenable_type`,`tokenable_id`),
  ADD KEY `personal_access_tokens_expires_at_index` (`expires_at`);

--
-- A tábla indexei `quantities`
--
ALTER TABLE `quantities`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `reservations`
--
ALTER TABLE `reservations`
  ADD PRIMARY KEY (`id`),
  ADD KEY `boxes_id` (`boxes_id`),
  ADD KEY `user_id` (`user_id`);

--
-- A tábla indexei `sessions`
--
ALTER TABLE `sessions`
  ADD PRIMARY KEY (`id`),
  ADD KEY `sessions_user_id_index` (`user_id`),
  ADD KEY `sessions_last_activity_index` (`last_activity`);

--
-- A tábla indexei `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `users_email_unique` (`email`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `bills`
--
ALTER TABLE `bills`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `boxes`
--
ALTER TABLE `boxes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT a táblához `cocktails`
--
ALTER TABLE `cocktails`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT a táblához `cocktail_drinks`
--
ALTER TABLE `cocktail_drinks`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=42;

--
-- AUTO_INCREMENT a táblához `cocktail_ingredients`
--
ALTER TABLE `cocktail_ingredients`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT a táblához `custom_cocktail`
--
ALTER TABLE `custom_cocktail`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `custom_cocktail_drinks`
--
ALTER TABLE `custom_cocktail_drinks`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `custom_cocktail_ingredients`
--
ALTER TABLE `custom_cocktail_ingredients`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `drinks`
--
ALTER TABLE `drinks`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32;

--
-- AUTO_INCREMENT a táblához `failed_jobs`
--
ALTER TABLE `failed_jobs`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `ingredients`
--
ALTER TABLE `ingredients`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT a táblához `jobs`
--
ALTER TABLE `jobs`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `migrations`
--
ALTER TABLE `migrations`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `open_bills`
--
ALTER TABLE `open_bills`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `orders`
--
ALTER TABLE `orders`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `orders_extra`
--
ALTER TABLE `orders_extra`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `personal_access_tokens`
--
ALTER TABLE `personal_access_tokens`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `quantities`
--
ALTER TABLE `quantities`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT a táblához `reservations`
--
ALTER TABLE `reservations`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `users`
--
ALTER TABLE `users`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `bills`
--
ALTER TABLE `bills`
  ADD CONSTRAINT `bills_ibfk_1` FOREIGN KEY (`box_id`) REFERENCES `boxes` (`id`),
  ADD CONSTRAINT `bills_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

--
-- Megkötések a táblához `cocktails`
--
ALTER TABLE `cocktails`
  ADD CONSTRAINT `cocktails_ibfk_1` FOREIGN KEY (`drink_id`) REFERENCES `drinks` (`id`);

--
-- Megkötések a táblához `cocktail_drinks`
--
ALTER TABLE `cocktail_drinks`
  ADD CONSTRAINT `cocktail_drinks_ibfk_3` FOREIGN KEY (`cocktail_id`) REFERENCES `cocktails` (`id`),
  ADD CONSTRAINT `cocktail_drinks_ibfk_4` FOREIGN KEY (`drink_id`) REFERENCES `drinks` (`id`);

--
-- Megkötések a táblához `cocktail_ingredients`
--
ALTER TABLE `cocktail_ingredients`
  ADD CONSTRAINT `cocktail_ingredients_ibfk_3` FOREIGN KEY (`cocktail_id`) REFERENCES `cocktails` (`id`),
  ADD CONSTRAINT `cocktail_ingredients_ibfk_4` FOREIGN KEY (`ingredient_id`) REFERENCES `ingredients` (`id`);

--
-- Megkötések a táblához `custom_cocktail`
--
ALTER TABLE `custom_cocktail`
  ADD CONSTRAINT `custom_cocktail_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`),
  ADD CONSTRAINT `custom_cocktail_ibfk_3` FOREIGN KEY (`cocktail_id`) REFERENCES `cocktails` (`id`);

--
-- Megkötések a táblához `custom_cocktail_drinks`
--
ALTER TABLE `custom_cocktail_drinks`
  ADD CONSTRAINT `custom_cocktail_drinks_ibfk_1` FOREIGN KEY (`custom_cocktail_id`) REFERENCES `custom_cocktail` (`id`),
  ADD CONSTRAINT `custom_cocktail_drinks_ibfk_2` FOREIGN KEY (`drink_id`) REFERENCES `drinks` (`id`);

--
-- Megkötések a táblához `custom_cocktail_ingredients`
--
ALTER TABLE `custom_cocktail_ingredients`
  ADD CONSTRAINT `custom_cocktail_ingredients_ibfk_1` FOREIGN KEY (`custom_cocktail_id`) REFERENCES `custom_cocktail` (`id`),
  ADD CONSTRAINT `custom_cocktail_ingredients_ibfk_2` FOREIGN KEY (`ingredient_id`) REFERENCES `ingredients` (`id`);

--
-- Megkötések a táblához `drinks`
--
ALTER TABLE `drinks`
  ADD CONSTRAINT `drinks_ibfk_1` FOREIGN KEY (`quantity_id`) REFERENCES `quantities` (`id`),
  ADD CONSTRAINT `drinks_ibfk_2` FOREIGN KEY (`mixing_quantity_id`) REFERENCES `quantities` (`id`);

--
-- Megkötések a táblához `ingredients`
--
ALTER TABLE `ingredients`
  ADD CONSTRAINT `ingredients_ibfk_1` FOREIGN KEY (`quantity_id`) REFERENCES `quantities` (`id`);

--
-- Megkötések a táblához `open_bills`
--
ALTER TABLE `open_bills`
  ADD CONSTRAINT `open_bills_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`),
  ADD CONSTRAINT `open_bills_ibfk_2` FOREIGN KEY (`box_id`) REFERENCES `boxes` (`id`);

--
-- Megkötések a táblához `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`item_id`) REFERENCES `drinks` (`id`);

--
-- Megkötések a táblához `orders_extra`
--
ALTER TABLE `orders_extra`
  ADD CONSTRAINT `orders_extra_ibfk_4` FOREIGN KEY (`box_detail_id`) REFERENCES `boxes` (`id`),
  ADD CONSTRAINT `orders_extra_ibfk_5` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`);

--
-- Megkötések a táblához `reservations`
--
ALTER TABLE `reservations`
  ADD CONSTRAINT `reservations_ibfk_1` FOREIGN KEY (`boxes_id`) REFERENCES `boxes` (`id`),
  ADD CONSTRAINT `reservations_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
