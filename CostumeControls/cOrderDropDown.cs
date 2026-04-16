using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ponth.CostumeControls
{
    public partial class cOrderDropDown : Form
    {
        public event Action<int> BillSelected;

        public cOrderDropDown(DataTable orders)
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;
            BackColor = Color.White;
            AutoSize = true;

            FlowLayoutPanel mainPanel = new FlowLayoutPanel();
            mainPanel.FlowDirection = FlowDirection.TopDown;
            mainPanel.WrapContents = false;
            mainPanel.AutoSize = true;
            mainPanel.Padding = new Padding(10);
            mainPanel.MaximumSize = new Size(350, 500);
            mainPanel.AutoScroll = true;

            var groupedOrders = orders.AsEnumerable()
                .GroupBy(r => r.Field<int>("orderId"));

            foreach (var order in groupedOrders)
            {
                DataRow first = order.First();

                string drinkName = first["drinkName"].ToString();
                int quantity = Convert.ToInt32(first["quantity"]);
                int subtotal = Convert.ToInt32(first["subtotal"]);

                Panel orderPanel = new Panel();
                orderPanel.AutoSize = true;
                orderPanel.Width = 320;
                orderPanel.Padding = new Padding(5);

                Label mainLabel = new Label();
                mainLabel.AutoSize = true;
                mainLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                mainLabel.Text =
                    $"{drinkName}  x{quantity}   {subtotal} Ft";

                orderPanel.Controls.Add(mainLabel);

                int y = mainLabel.Bottom + 2;

                foreach (var row in order)
                {
                    if (row["extraName"] == DBNull.Value)
                        continue;

                    string extraName = row["extraName"].ToString();
                    int extraQty =
                        Convert.ToInt32(row["extraQuantity"]);
                    int extraPrice =
                        Convert.ToInt32(row["extraPrice"]);

                    Label extraLabel = new Label();
                    extraLabel.AutoSize = true;
                    extraLabel.Location = new Point(20, y);
                    extraLabel.ForeColor = Color.DimGray;

                    extraLabel.Text =
                        $"+ {extraName} x{extraQty} ({extraPrice} Ft)";

                    orderPanel.Controls.Add(extraLabel);

                    y = extraLabel.Bottom + 2;
                }

                mainPanel.Controls.Add(orderPanel);
            }

            Controls.Add(mainPanel);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            Close();
        }
    }
}