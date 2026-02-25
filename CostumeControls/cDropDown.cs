using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace ponth.CostumeControls
{
    public class cDropDown : Form
    {
        // Mindkét értéket visszaadjuk
        public event Action<int, int> ItemSelected;

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
                int tableNumber = Convert.ToInt32(row["tableNumber"]);
                int total = Convert.ToInt32(row["total"]);

                cButtons btn = new cButtons();
                btn.Text = tableNumber.ToString();   // Gomb neve az asztalszám
                btn.Tag = total;                     // Tag-ben a total
                btn.Size = new Size(100, 100);
                btn.ForeColor = Color.White;
                btn.Margin = new Padding(5);

                btn.Click += (s, e) =>
                {
                    ItemSelected?.Invoke(tableNumber, total);
                    this.Close();
                };

                panel.Controls.Add(btn);
            }

            Controls.Add(panel);
        }
    }
}