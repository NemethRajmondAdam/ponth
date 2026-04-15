using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ponth
{
    public partial class frmOrderDetails : Form
    {
        public event Action<int> TableStatusChanged;

        private int tableId;
        private Panel panelContainer;
        private Button btnServeAll;

        public frmOrderDetails(int tableId)
        {
            InitializeComponent();
            this.tableId = tableId;

            this.Text = $"Asztal {tableId} rendelései";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;

            // Panel a rendelés paneleknek
            panelContainer = new Panel();
            panelContainer.Dock = DockStyle.Top;
            panelContainer.AutoScroll = true;
            panelContainer.Height = 400;

            this.Controls.Add(panelContainer);

            // Felszolgálás gomb
            btnServeAll = new Button();
            btnServeAll.Text = "Felszolgál";
            btnServeAll.Dock = DockStyle.Bottom;
            btnServeAll.Height = 40;
            btnServeAll.BackColor = Color.LightBlue;
            btnServeAll.Click += BtnServeAll_Click;

            this.Controls.Add(btnServeAll);

            LoadOrders();
        }

        private void LoadOrders()
        {
            panelContainer.Controls.Clear();

            string query = $@"
                SELECT o.id, ob.box_id AS tableNumber, d.name AS productName, o.quantity, o.status
                FROM orders o
                INNER JOIN drinks d ON o.item_id = d.id
                INNER JOIN open_bills ob ON o.bills_id = ob.id
                WHERE ob.box_id = {tableId} AND o.status <> 'served'
                ORDER BY o.id DESC;
            ";

            DataTable dt = DatabaseHelper.GetData(query);

            int y = 10;

            foreach (DataRow row in dt.Rows)
            {
                int orderId = Convert.ToInt32(row["id"]);

                // Load extras for this order
                DataTable extras = DatabaseHelper.GetData(
                    $@"SELECT oe.*, 
                        CASE 
                            WHEN oe.type = 'Drink' THEN d.name 
                            ELSE i.name 
                        END AS extra_name
                    FROM orders_extra oe
                    LEFT JOIN drinks d ON oe.type = 'Drink' AND oe.item_id = d.id
                    LEFT JOIN ingredients i ON oe.type = 'Ingredient' AND oe.item_id = i.id
                    WHERE oe.order_id = {orderId}");

                int extraCount = extras.Rows.Count;
                int panelHeight = 45 + (extraCount * 22);
                if (panelHeight < 80) panelHeight = 80;

                Panel orderPanel = new Panel();
                orderPanel.Width = panelContainer.ClientSize.Width - 25;
                orderPanel.Height = panelHeight;
                orderPanel.Location = new Point(10, y);
                orderPanel.BorderStyle = BorderStyle.FixedSingle;

                // STATUS SZÍNEK
                string status = row["status"].ToString().ToLower();
                orderPanel.BackColor = GetStatusColor(status);

                // RENDELÉS LABEL — show extras count
                string extraText = extraCount > 0 ? $"  (+{extraCount} extra)" : "";
                Label lbl = new Label();
                lbl.Text = $"ID: {row["id"]}  Termék: {row["productName"]}  Mennyiség: {row["quantity"]}  Status: {row["status"]}{extraText}";
                lbl.ForeColor = Color.Black;
                lbl.Font = new Font("Arial", 10, FontStyle.Bold);
                lbl.Location = new Point(10, 10);
                lbl.AutoSize = true;


                orderPanel.Click += (s, e) => ToggleOrderStatus(orderId, orderPanel, lbl);
                lbl.Click += (s, e) => ToggleOrderStatus(orderId, orderPanel, lbl);

                orderPanel.Controls.Add(lbl);

                // Show extras as sub-labels
                int extraY = 35;
                foreach (DataRow extraRow in extras.Rows)
                {
                    string extraName = extraRow["extra_name"] != DBNull.Value
                        ? extraRow["extra_name"].ToString() : "?";
                    int qty = Convert.ToInt32(extraRow["quantity"]);
                    string type = extraRow["type"].ToString();
                    int price = Convert.ToInt32(extraRow["price"]);

                    Label lblExtra = new Label()
                    {
                        Text = $"  + {extraName} ({qty}x, {type}) - {price} Ft",
                        ForeColor = Color.DarkGreen,
                        Font = new Font("Arial", 9, FontStyle.Italic),
                        Location = new Point(20, extraY),
                        AutoSize = true
                    };
                    orderPanel.Controls.Add(lblExtra);
                    extraY += 22;
                }

                panelContainer.Controls.Add(orderPanel);

                y += orderPanel.Height + 10;
            }
        }

        private Color GetStatusColor(string status)
        {
            switch (status.ToLower())
            {
                case "new":
                    return Color.Red;
                case "preparing":
                    return Color.Yellow;
                case "served":
                    return Color.Green;
                default:
                    return Color.LightGray;
            }
        }

        //STATUSZ VALTAS
        private void ToggleOrderStatus(int orderId, Panel orderPanel, Label lbl)
        {
            string currentStatus = DatabaseHelper.ExecuteScalar($"SELECT status FROM orders WHERE id={orderId}")?.ToString()?.ToLower();
            if (currentStatus == null) return;

            string newStatus;
            switch (currentStatus)
            {
                case "new":
                    newStatus = "preparing";
                    break;
                case "preparing":
                    newStatus = "served";
                    break;
                case "served":
                    newStatus = "served";
                    break;
                default:
                    newStatus = currentStatus;
                    break;
            }

            DatabaseHelper.ExecuteNonQuery($"UPDATE orders SET status='{newStatus}' WHERE id={orderId}");

            orderPanel.BackColor = GetStatusColor(newStatus);
            lbl.Text = lbl.Text.Split(new string[] { "Status:" }, StringSplitOptions.None)[0] + $"Status: {newStatus}";

            TableStatusChanged?.Invoke(tableId);

        }

        //FELSZOLGAL
        private void BtnServeAll_Click(object sender, EventArgs e)
        {
            DatabaseHelper.ExecuteNonQuery($@"
                UPDATE orders 
                SET status='served' 
                WHERE bills_id IN (
                    SELECT id FROM open_bills WHERE box_id = {tableId}
                )");

            foreach (Panel panel in panelContainer.Controls.OfType<Panel>())
            {
                panel.BackColor = Color.Green;
                Label lbl = panel.Controls.OfType<Label>().FirstOrDefault();
                if (lbl != null)
                    lbl.Text = lbl.Text.Split(new string[] { "Status:" }, StringSplitOptions.None)[0] + "Status: served";
            }

            TableStatusChanged?.Invoke(tableId);

            this.Close();


        }

    }
}