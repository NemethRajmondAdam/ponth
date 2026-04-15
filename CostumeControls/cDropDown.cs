using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ponth.CostumeControls
{
    public class cDropDown : Form
    {
        // billId + total + megjelenített szöveg
        public event Action<int, int, string> ItemSelected;

        public cDropDown(DataTable tables)
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.White;
            AutoSize = true;

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.FlowDirection = FlowDirection.LeftToRight;
            panel.WrapContents = true;
            panel.AutoSize = true;
            panel.MaximumSize = new Size(400, 0);
            panel.Padding = new Padding(5);

            foreach (DataRow row in tables.Rows)
            {
                int billId = Convert.ToInt32(row["billId"]);
                int total = Convert.ToInt32(row["total"]);
                string display = row["display"].ToString();

                cButtons btn = new cButtons();

                btn.Text = display;
                btn.Size = new Size(100, 100);
                btn.ForeColor = Color.White;
                btn.Margin = new Padding(5);

                btn.Click += (s, e) =>
                {
                    ItemSelected?.Invoke(billId, total, display);
                    this.Close();
                };

                panel.Controls.Add(btn);
            }

            Controls.Add(panel);
        }
    }
}