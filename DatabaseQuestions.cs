using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ponth
{
    public class DatabaseQuestions
    {
        public static bool IsDataExists(string query)
        {
            try
            {
                // Végrehajtjuk a lekérdezést, és megnézzük, hogy van-e eredmény.
                object result = DatabaseHelper.ExecuteScalar(query);

                // Ha van találat (nem null vagy nem 0), akkor az adat létezik
                return result != null && Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                // Hibakezelés: itt naplózhatod a hibát, vagy egy hibaüzenetet is megjeleníthetsz
                //MessageBox.Show($"Hiba történt a lekérdezés végrehajtása közben: {ex.Message}");
                return false;
            }
        }
    }
}
