using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public static class DatabaseHelper
{
    private static string GetConnectionString()
    {
        return ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;
    }

    public static DataTable GetData(string query)
    {
        DataTable dt = new DataTable();
        string s = GetConnectionString();
        using (MySqlConnection conn = new MySqlConnection(s))
        {
            conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
             
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
        }
        return dt;
    }

    public static int ExecuteNonQuery(string query)
    {
        int rowsAffected = 0;
        using (MySqlConnection conn = new MySqlConnection(GetConnectionString()))
        {
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
        }
        return rowsAffected;
    }

    public static object ExecuteScalar(string query)
    {
        object result;
        using (MySqlConnection conn = new MySqlConnection(GetConnectionString()))
        {
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                result = cmd.ExecuteScalar();
            }
        }
        return result;
    }

    public static void ExecuteUpdate(string query)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(GetConnectionString()))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Hiba történt az UPDATE végrehajtása közben: {ex.Message}");
        }
    }

}
