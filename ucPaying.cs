using ponth.CostumeControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ponth
{
    public partial class ucPaying : UserControl
    {

        public event Action CancelRequested;
        public event Action PaymentDone;

        public ucPaying()
        {
            InitializeComponent();
        }

        DataTable tables;

        private void GetTables()
        {
            
            tables = new DataTable();
            tables.Columns.Add("tableNumber",typeof(int));
            tables.Columns.Add("total",typeof(int));

            string sql = "SELECT * FROM boxes_details ORDER BY box_id";
            DataTable openBills = DatabaseHelper.GetData(sql);

            if (openBills != null && openBills.Rows.Count != 0)
            {
                foreach (DataRow billDetails in openBills.Rows)
                {
                    tables.Rows.Add(
                        Convert.ToInt32(billDetails["box_id"]),
                        Convert.ToInt32(billDetails["totalSum"])
                    );
                }
            }
            else
            {
                btnDropdown.Text = "Nincs fizetendő számla";
                btnDropdown.Enabled = false;
                return;
            }

        }

        private void btnDropdown_Click(object sender, EventArgs e)
        {
            GetTables();

            cDropDown popup = new cDropDown(tables);

            var location = btnDropdown.PointToScreen(new Point(0, btnDropdown.Height));
            popup.Location = location;

            popup.ItemSelected += (tableNumber, total) =>
            {
                btnDropdown.Text = tableNumber.ToString();
                btnDropdown.Tag = total;
            };

            popup.Show();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelRequested?.Invoke();
        }

        private bool realNumer()
        {
            if (tables == null || tables.Rows.Count == 0)
                return false;

            int selectedTable;

            if (!int.TryParse(btnDropdown.Text, out selectedTable))
                return false;

            foreach (DataRow row in tables.Rows)
            {
                if ((int)row["tableNumber"] == selectedTable)
                    return true;
            }

            return false;
        }

        private void billClosing()
        {
            string sql = $"DELETE FROM boxes_details WHERE box_id = {btnDropdown.Text}";
            DatabaseHelper.ExecuteNonQuery(sql);
        }

        private void Payment()
        {
            if (realNumer())
            {
                billClosing();
                MessageBox.Show($"fizetve {btnDropdown.Tag} FT ..., a(z) {btnDropdown.Text} asztalnal");
                PaymentDone?.Invoke();
            }
            else
            {
                MessageBox.Show($"valaszon asztalt");
            }
        }

        private void btnCard_Click(object sender, EventArgs e)
        {
            //Payment()

            if (realNumer())
            {
                billClosing();
                MessageBox.Show($"fizetve {btnDropdown.Tag} FT kartyaval, a(z) {btnDropdown.Text} asztalnal");
                PaymentDone?.Invoke();
            }
            else
            {
                MessageBox.Show($"valaszon asztalt");
            }
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            //Payment()

            if (realNumer())
            {
                billClosing();
                MessageBox.Show($"fizetve {btnDropdown.Tag} FT keszpenzzel, a(z) {btnDropdown.Text} asztalnal");
                PaymentDone?.Invoke();
            }
            else
            {
                MessageBox.Show($"valaszon asztalt");
            }
        }
    }
}
