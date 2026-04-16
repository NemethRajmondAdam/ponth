|  URL	                | HTTP method | Auth | JSON Response
|-----------------------|-------------|------|---------------
|/register	            |    POST     |		 | Sikeres regisztráció + Token
|/login	                |    POST     |		 | User adatok + Token
|/logout	            |    POST     |	 Y	 | Kijelentkezési üzenet
|/drinks	            |    GET      |		 | Összes ital listája
|/drinks	            |    POST     |	 Y	 | Új ital hozzáadva
|/drinks/{id}	        |    PATCH/PUT|	 Y	 | Módosított ital adatai
|/drinks/{id}	        |    DELETE   |	 Y	 | Törlés megerősítése
|/cocktails	            |    GET      | 	 | Összes alap koktél listája
|/cocktails	            |    POST     |	 Y	 | Új koktél + összetevők mentve
|/cocktails/{id}        |    PATCH/PUT|	 Y	 | Módosított koktél adatai
|/cocktails/{id}        |    DELETE   |	 Y	 | Törlés megerősítése
|/custom-cocktails      |    GET      |		 | Összes egyedi koktél listája
|/custom-cocktails      |    POST     |	 Y	 | Felhasználóhoz kötött egyedi koktél
|/custom-cocktails/{id}	|    DELETE   |	 Y	 | Egyedi koktél törlése (csak tulajdonos)
|/ingredients	        |    GET      |		 | Összes alapanyag listája
|/ingredients	        |    POST     |	 Y	 | Új alapanyag hozzáadva
|/ingredients/{id}	    |    DELETE   |	 Y	 | Alapanyag törlése
|/boxes	                |    GET      |		 | Összes asztal állapota
|/boxes	                |    POST     |	 Y	 | Új asztal felvétele
|/reservations	        |    GET      |	 Y	 | Összes foglalás listája
|/reservations	        |    POST     |	 Y	 | Új foglalás (időpont ellenőrzéssel)
|/orders	            |    POST     |	 Y	 | Új rendelés (asztal egyenleg frissítéssel)
|/quantities	        |    GET      |		 | Összes mértékegység listája