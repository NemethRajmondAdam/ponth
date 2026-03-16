-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2026. Már 16. 12:25
-- Kiszolgáló verziója: 10.4.28-MariaDB
-- PHP verzió: 8.2.4

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
  `paid_with` enum('Card','Cash','','') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `bills`
--

INSERT INTO `bills` (`id`, `box_id`, `total`, `paid_at`, `paid_with`) VALUES
(1, 5, 11100, '0000-00-00 00:00:00', 'Card'),
(2, 4, 3550, '0000-00-00 00:00:00', 'Cash'),
(3, 4, 16800, '2026-02-26 09:14:08', 'Card'),
(4, 2, 2600, '2026-02-26 09:14:12', 'Cash');

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
(2, 13, 'Pina Colada'),
(3, 14, 'Cosmopolitan'),
(4, 15, 'Tequila Sunrise'),
(5, 12, 'Mojito');

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
(1, 1, 16, 50),
(2, 2, 16, 50),
(3, 3, 2, 40),
(4, 3, 4, 20),
(5, 4, 3, 50);

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
(1, 1, 1, 1),
(2, 1, 2, 20),
(3, 1, 3, 5),
(4, 1, 7, 1),
(5, 2, 4, 1),
(6, 2, 6, 60),
(7, 3, 5, 1),
(8, 4, 8, 1);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `custom_cocktail`
--

CREATE TABLE `custom_cocktail` (
  `id` int(11) NOT NULL,
  `cocktail_name` varchar(100) NOT NULL,
  `user_id` bigint(20) UNSIGNED NOT NULL,
  `price` int(11) NOT NULL,
  `cocktail_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `custom_cocktail_drinks`
--

CREATE TABLE `custom_cocktail_drinks` (
  `id` int(11) NOT NULL,
  `custom_cocktail_id` int(11) NOT NULL,
  `drink_id` int(11) NOT NULL
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
(2, 'Alcohol', 'Vodka', 850, 1, 9, 3),
(3, 'Alcohol', 'Tequila', 950, 1, 10, 3),
(4, 'Alcohol', 'Triple Sec', 900, 1, 9, 3),
(5, 'Softdrink', 'Cola', 450, 1, 5, 3),
(6, 'Softdrink', 'Soda Water', 300, 1, 3, 3),
(7, 'Softdrink', 'Pineapple Juice', 350, 1, 4, 3),
(8, 'Softdrink', 'Orange Juice', 350, 1, 4, 3),
(9, 'Softdrink', 'Cranberry Juice', 350, 1, 4, 3),
(10, 'Softdrink', 'Coconut Cream', 400, 1, 4, 3),
(11, 'Softdrink', 'Lime Juice', 300, 1, 3, 3),
(12, 'Cocktail', 'Mojito', 2400, 2, 0, 2),
(13, 'Cocktail', 'Piña Colada', 2600, 2, 0, 2),
(14, 'Cocktail', 'Cosmopolitan', 2700, 2, 0, 2),
(15, 'Cocktail', 'Tequila Sunrise', 2500, 2, 0, 2),
(16, 'Alcohol', 'White Rum', 900, 1, 9, 3);

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
(1, 'Lime', 2, 150),
(2, 'Sugar Syrup', 3, 70),
(3, 'Mint Leaves', 2, 50),
(4, 'Pineapple Juice', 1, 350),
(5, 'Cranberry Juice', 1, 350),
(6, 'Coconut Cream', 1, 400),
(7, 'Soda Water', 1, 300),
(8, 'Orange Juice', 1, 350);

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

--
-- A tábla adatainak kiíratása `migrations`
--

INSERT INTO `migrations` (`id`, `migration`, `batch`) VALUES
(1, '0001_01_01_000000_create_users_table', 1),
(2, '0001_01_01_000001_create_cache_table', 1),
(3, '0001_01_01_000002_create_jobs_table', 1),
(4, '2026_01_08_080534_create_personal_access_tokens_table', 1);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `open_bills`
--

CREATE TABLE `open_bills` (
  `id` int(11) NOT NULL,
  `box_id` int(11) DEFAULT NULL,
  `totalSum` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `open_bills`
--

INSERT INTO `open_bills` (`id`, `box_id`, `totalSum`) VALUES
(4, 1, 5950);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `orders`
--

CREATE TABLE `orders` (
  `id` int(11) NOT NULL,
  `box_id` int(11) DEFAULT NULL,
  `item_id` int(11) DEFAULT NULL,
  `quantity` int(11) DEFAULT NULL,
  `subtotal` int(11) DEFAULT NULL,
  `status` varchar(20) NOT NULL DEFAULT 'new'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `orders`
--

INSERT INTO `orders` (`id`, `box_id`, `item_id`, `quantity`, `subtotal`, `status`) VALUES
(2, 3, 13, 1, 2600, 'served'),
(3, 3, 15, 1, 2500, 'served'),
(4, 3, 12, 1, 2400, 'served'),
(5, 3, 14, 1, 2700, 'served'),
(6, 4, 3, 1, 950, 'served'),
(7, 5, 12, 9, 21600, 'served'),
(8, 5, 13, 9, 23400, 'served'),
(9, 5, 15, 4, 10000, 'served'),
(10, 5, 14, 2, 5400, 'served'),
(11, 5, 2, 10, 8500, 'served'),
(12, 5, 13, 12, 31200, 'served'),
(13, 5, 13, 1, 2600, 'served'),
(14, 5, 12, 2, 4800, 'served'),
(15, 5, 2, 1, 850, 'served'),
(17, 4, 13, 1, 2600, 'served'),
(21, 4, 13, 1, 2600, 'served'),
(22, 5, 15, 3, 7500, 'served'),
(23, 5, 14, 1, 2700, 'served'),
(24, 5, 4, 1, 900, 'served'),
(25, 2, 13, 1, 2600, 'served'),
(26, 4, 13, 1, 2600, 'served'),
(27, 4, 12, 1, 2400, 'served'),
(28, 4, 13, 1, 2600, 'served'),
(29, 4, 3, 1, 950, 'served'),
(30, 4, 12, 7, 16800, 'served'),
(31, 1, 12, 1, 2400, 'served');

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

--
-- A tábla adatainak kiíratása `personal_access_tokens`
--

INSERT INTO `personal_access_tokens` (`id`, `tokenable_type`, `tokenable_id`, `name`, `token`, `abilities`, `last_used_at`, `expires_at`, `created_at`, `updated_at`) VALUES
(14, 'App\\Models\\User', 1, 'access', 'a1ab931070fc268608840863dd2308c637cee3b37a66295437495423220b64d6', '[\"*\"]', '2026-03-04 14:18:33', NULL, '2026-03-04 14:17:54', '2026-03-04 14:18:33'),
(15, 'App\\Models\\User', 4, 'access', '6fc34b99c53cea06937751a180d5e41c7096c75be6421a294f8f19a7c2329095', '[\"*\"]', '2026-03-09 09:52:30', NULL, '2026-03-04 14:18:48', '2026-03-09 09:52:30');

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
(1, 'dl'),
(2, 'piece'),
(3, 'ml');

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

--
-- A tábla adatainak kiíratása `reservations`
--

INSERT INTO `reservations` (`id`, `boxes_id`, `reservation_date`, `reservation_time`, `reserved_at`, `duration_minutes`, `name`, `phone`, `user_id`, `status`) VALUES
(1, 3, '2026-03-14', '17:00:00', '2026-03-04 10:55:28', 60, 'Vivike', '+36305137847', NULL, 'lemondva'),
(2, 2, '2026-03-06', '19:30:00', '2026-03-04 15:17:38', 60, 'Vivike', '+36305137847', NULL, 'lemondva'),
(3, 5, '2026-03-04', '16:00:00', '2026-03-04 15:18:31', 60, 'test', '+3630513784', NULL, 'lemondva'),
(4, 5, '2026-03-05', '23:30:00', '2026-03-04 16:09:16', 60, 'Vivike', '+36305137847', NULL, 'foglalva');

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

--
-- A tábla adatainak kiíratása `sessions`
--

INSERT INTO `sessions` (`id`, `user_id`, `ip_address`, `user_agent`, `payload`, `last_activity`) VALUES
('vqUSIDKfmrVWRBSc66LJ1CwfybfP4cVJz68ljCph', NULL, '127.0.0.1', 'PostmanRuntime/7.51.0', 'YTozOntzOjY6Il90b2tlbiI7czo0MDoidWduRjFBR2Npc3RycDJZaUxnWFdJcG1LTldZVFdsZlpRenNMUW11VCI7czo5OiJfcHJldmlvdXMiO2E6Mjp7czozOiJ1cmwiO3M6MjE6Imh0dHA6Ly9sb2NhbGhvc3Q6ODAwMCI7czo1OiJyb3V0ZSI7Tjt9czo2OiJfZmxhc2giO2E6Mjp7czozOiJvbGQiO2E6MDp7fXM6MzoibmV3IjthOjA6e319fQ==', 1769674425);

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
(1, 'test', 'test@gmail.com', '2026-01-26 11:07:24', '$2y$12$fJb/cx.bwR7FWFfjbJ4px.V5Uy2SG6zxZWAM97hKTkUH0ebconoVW', 'hTx0aAyW9Q', '2026-01-26 11:07:25', '2026-01-26 11:07:25', 1),
(4, 'Vivike', 'vivike@gmail.com', NULL, '$2y$12$Bmgxml/Lhh3DCn2Gjn1RXONskBKstH282XBI6UrglkEBLAfl.1m1S', NULL, '2026-03-04 08:59:52', '2026-03-04 09:01:37', 0);

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `bills`
--
ALTER TABLE `bills`
  ADD PRIMARY KEY (`id`),
  ADD KEY `box_id` (`box_id`);

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
  ADD KEY `box_id` (`box_id`);

--
-- A tábla indexei `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`id`),
  ADD KEY `boxes_deatils_id` (`box_id`),
  ADD KEY `item_id` (`item_id`),
  ADD KEY `box_id` (`box_id`);

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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT a táblához `boxes`
--
ALTER TABLE `boxes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT a táblához `cocktails`
--
ALTER TABLE `cocktails`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT a táblához `cocktail_drinks`
--
ALTER TABLE `cocktail_drinks`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT a táblához `cocktail_ingredients`
--
ALTER TABLE `cocktail_ingredients`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

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
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT a táblához `open_bills`
--
ALTER TABLE `open_bills`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT a táblához `orders`
--
ALTER TABLE `orders`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32;

--
-- AUTO_INCREMENT a táblához `orders_extra`
--
ALTER TABLE `orders_extra`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `personal_access_tokens`
--
ALTER TABLE `personal_access_tokens`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT a táblához `quantities`
--
ALTER TABLE `quantities`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT a táblához `reservations`
--
ALTER TABLE `reservations`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT a táblához `users`
--
ALTER TABLE `users`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `bills`
--
ALTER TABLE `bills`
  ADD CONSTRAINT `bills_ibfk_1` FOREIGN KEY (`box_id`) REFERENCES `boxes` (`id`);

--
-- Megkötések a táblához `cocktails`
--
ALTER TABLE `cocktails`
  ADD CONSTRAINT `cocktails_ibfk_1` FOREIGN KEY (`drink_id`) REFERENCES `drinks` (`id`);

--
-- Megkötések a táblához `cocktail_drinks`
--
ALTER TABLE `cocktail_drinks`
  ADD CONSTRAINT `cocktail_drinks_ibfk_1` FOREIGN KEY (`cocktail_id`) REFERENCES `cocktails` (`id`),
  ADD CONSTRAINT `cocktail_drinks_ibfk_2` FOREIGN KEY (`drink_id`) REFERENCES `drinks` (`id`);

--
-- Megkötések a táblához `cocktail_ingredients`
--
ALTER TABLE `cocktail_ingredients`
  ADD CONSTRAINT `cocktail_ingredients_ibfk_1` FOREIGN KEY (`cocktail_id`) REFERENCES `cocktails` (`id`),
  ADD CONSTRAINT `cocktail_ingredients_ibfk_2` FOREIGN KEY (`ingredient_id`) REFERENCES `ingredients` (`id`);

--
-- Megkötések a táblához `custom_cocktail`
--
ALTER TABLE `custom_cocktail`
  ADD CONSTRAINT `custom_cocktail_ibfk_1` FOREIGN KEY (`cocktail_id`) REFERENCES `cocktails` (`id`),
  ADD CONSTRAINT `custom_cocktail_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

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
  ADD CONSTRAINT `open_bills_ibfk_1` FOREIGN KEY (`box_id`) REFERENCES `boxes` (`id`);

--
-- Megkötések a táblához `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `orders_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `drinks` (`id`),
  ADD CONSTRAINT `orders_ibfk_3` FOREIGN KEY (`box_id`) REFERENCES `boxes` (`id`);

--
-- Megkötések a táblához `orders_extra`
--
ALTER TABLE `orders_extra`
  ADD CONSTRAINT `orders_extra_ibfk_3` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`),
  ADD CONSTRAINT `orders_extra_ibfk_4` FOREIGN KEY (`box_detail_id`) REFERENCES `boxes` (`id`);

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
