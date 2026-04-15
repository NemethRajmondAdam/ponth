using ponth.CostumeControls;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ponth
{
    public partial class ucPaying : UserControl
    {
        public event Action CancelRequested;
        public event Action PaymentDone;

        DataTable tables;

        public ucPaying()
        {
            InitializeComponent();
        }

        // ===============================
        // SZÁMLÁK BETÖLTÉSE
        // ===============================
        private void GetTables()
        {
            tables = new DataTable();

            tables.Columns.Add("billId", typeof(int));
            tables.Columns.Add("tableNumber", typeof(int));
            tables.Columns.Add("total", typeof(int));
            tables.Columns.Add("display", typeof(string));
            tables.Columns.Add("userId", typeof(int));

            string sql =
            @"SELECT ob.id,
                     ob.box_id,
                     ob.totalSum,
                     ob.user_id,
                     u.name
              FROM open_bills ob
              LEFT JOIN users u ON ob.user_id = u.id
              ORDER BY ob.box_id";

            DataTable openBills = DatabaseHelper.GetData(sql);

            if (openBills.Rows.Count == 0)
            {
                btnDropdown.Text = "Nincs fizetendő számla";
                btnDropdown.Enabled = false;
                return;
            }

            foreach (DataRow row in openBills.Rows)
            {
                int billId = Convert.ToInt32(row["id"]);
                int tableNumber = Convert.ToInt32(row["box_id"]);
                int total = Convert.ToInt32(row["totalSum"]);

                string display;

                if (row["user_id"] != DBNull.Value)
                {
                    string name = row["name"].ToString();
                    display = $"{tableNumber} - {name}";
                }
                else
                {
                    display = tableNumber.ToString();
                }

                tables.Rows.Add(
                    billId,
                    tableNumber,
                    total,
                    display,
                    row["user_id"] == DBNull.Value ? DBNull.Value : row["user_id"]
                );
            }
        }

        // ===============================
        // DROPDOWN MEGNYITÁS
        // ===============================
        private void btnDropdown_Click(object sender, EventArgs e)
        {
            GetTables();

            cDropDown popup = new cDropDown(tables);

            var location = btnDropdown.PointToScreen(
                new Point(0, btnDropdown.Height));

            popup.Location = location;

            popup.ItemSelected += (billId, total, display) =>
            {
                btnDropdown.Text = display;
                btnDropdown.Tag = billId;

                lb_Total.Text = total + " FT";
            };

            popup.Show();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelRequested?.Invoke();
        }

        // ===============================
        // VAN-E KIVÁLASZTOTT SZÁMLA
        // ===============================
        private bool realNumer()
        {
            return btnDropdown.Tag != null;
        }

        // ===============================
        // SZÁMLA ZÁRÁSA
        // ===============================
        private void billClosing(string payMethod)
        {
            int billId = Convert.ToInt32(btnDropdown.Tag);

            string getBill =
                $"SELECT box_id,totalSum,user_id FROM open_bills WHERE id={billId}";

            DataTable dt = DatabaseHelper.GetData(getBill);

            int boxId = Convert.ToInt32(dt.Rows[0]["box_id"]);
            int total = Convert.ToInt32(dt.Rows[0]["totalSum"]);

            string sql;

            if (dt.Rows[0]["user_id"] == DBNull.Value)
            {
                sql =
                $"INSERT INTO bills (box_id,total,paid_at,paid_with) " +
                $"VALUES ({boxId},{total},'{DateTime.Now:yyyy-MM-dd HH:mm:ss}','{payMethod}')";
            }
            else
            {
                int userId = Convert.ToInt32(dt.Rows[0]["user_id"]);

                sql =
                $"INSERT INTO bills (box_id,total,paid_at,paid_with,user_id) " +
                $"VALUES ({boxId},{total},'{DateTime.Now:yyyy-MM-dd HH:mm:ss}','{payMethod}',{userId})";
            }

            DatabaseHelper.ExecuteNonQuery(sql);

            // ⭐ CSAK EZT AZ EGY SZÁMLÁT TÖRÖLJÜK
            DatabaseHelper.ExecuteNonQuery(
                $"DELETE FROM open_bills WHERE id = {billId}");
        }

        // ===============================
        // FIZETÉS
        // ===============================
        private void Payment(string payMethod)
        {
            if (realNumer())
            {
                billClosing(payMethod);

                MessageBox.Show(
                    $"Fizetve {lb_Total.Text}");

                PaymentDone?.Invoke();
            }
            else
            {
                MessageBox.Show("Válassz számlát!");
            }
        }

        private void btnCard_Click(object sender, EventArgs e)
        {
            Payment("Card");
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            Payment("Cash");
        }
    }
}