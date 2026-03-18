using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ponth
{
    public partial class ucTables : UserControl
    {
        private List<Button> tableButtons = new List<Button>();

        public ucTables()
        {
            InitializeComponent();
            LoadTables();
        }

        //ASZTALOK BETOLTESE
        public void LoadTables()
        {
            this.Controls.Clear();
            tableButtons.Clear();

            string query = "SELECT id FROM boxes ORDER BY id";
            DataTable dt = DatabaseHelper.GetData(query);

            int index = 0;

            foreach (DataRow row in dt.Rows)
            {
                int tableNum = Convert.ToInt32(row["id"]);

                Button btn = new Button();
                btn.Width = 100;
                btn.Height = 80;
                btn.Text = $"Table {tableNum}";
                btn.Font = new Font("Arial", 12, FontStyle.Bold);

                // grid elrendezés (5 oszlop)
                int x = (index % 5) * 110;
                int y = (index / 5) * 90;

                btn.Location = new Point(x, y);

                btn.BackColor = Color.Green;
                btn.ForeColor = Color.White;

                int tableId = tableNum;
                btn.Click += (s, e) => TableClicked(tableId);

                tableButtons.Add(btn);
                this.Controls.Add(btn);

                index++;
            }

            UpdateTableStatus();
        }

        //RENDELES MEGJELENITESE
        private void TableClicked(int tableId)
        {
            frmOrderDetails orderForm = new frmOrderDetails(tableId);

            orderForm.TableStatusChanged += (id) =>
            {
                RefreshTableStatus(id);
            };


            orderForm.Show();

        }

        //ASZTALOK SZINEZESE
        public void UpdateTableStatus()
        {
            string query = @"
                            SELECT ob.box_id, o.status 
                            FROM orders o
                            JOIN open_bills ob ON o.bills_id = ob.id
                            WHERE o.status IN ('new','preparing')";
            DataTable dt = DatabaseHelper.GetData(query);

            //ZOLD MINDEGYIK
            foreach (var btn in tableButtons)
            {
                btn.BackColor = Color.Green;
                btn.Text = btn.Text.Split(' ')[0] + " " + btn.Text.Split(' ')[1];
            }

            //STATUSZ SZINEK (Zold/Nincs semmi, Sarga/Keszul, Piros/Uj)
            foreach (DataRow row in dt.Rows)
            {
                int tableNum = Convert.ToInt32(row["box_id"]);
                string status = row["status"].ToString();

                if (tableNum - 1 >= 0 && tableNum - 1 < tableButtons.Count)
                {
                    Button btn = tableButtons[tableNum - 1];

                    switch (status)
                    {
                        case "new":
                            btn.BackColor = Color.Red;
                            btn.Text = $"Table {tableNum}\nNEW";
                            break;
                        case "preparing":
                            btn.BackColor = Color.Yellow;
                            btn.Text = $"Table {tableNum}\nPrep";
                            break;
                    }
                }
            }
        }

        public void RefreshTableStatus(int tableId)
        {
            string query = $@"
                SELECT o.status 
                FROM orders o
                JOIN open_bills ob ON o.bills_id = ob.id
                WHERE ob.box_id = {tableId}";

            DataTable dt = DatabaseHelper.GetData(query);

            bool hasNew = false;
            bool hasPreparing = false;

            foreach (DataRow row in dt.Rows)
            {
                string status = row["status"].ToString();

                if (status == "new") hasNew = true;
                if (status == "preparing") hasPreparing = true;
            }

            Button tableButton = tableButtons[tableId-1];

            if (hasNew)
                tableButton.BackColor = Color.Red;
            else if (hasPreparing)
                tableButton.BackColor = Color.Yellow;
            else
                tableButton.BackColor = Color.Green;
        }
    }
}