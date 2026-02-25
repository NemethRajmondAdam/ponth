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
    }
}
