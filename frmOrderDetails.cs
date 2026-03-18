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
                Panel orderPanel = new Panel();
                orderPanel.Width = panelContainer.ClientSize.Width - 25;
                orderPanel.Height = 80;
                orderPanel.Location = new Point(10, y);
                orderPanel.BorderStyle = BorderStyle.FixedSingle;

                // STATUS SZÍNEK
                string status = row["status"].ToString().ToLower();
                orderPanel.BackColor = GetStatusColor(status);

                // RENDELÉS LABEL
                Label lbl = new Label();
                lbl.Text = $"ID: {row["id"]}  Termék: {row["productName"]}  Mennyiség: {row["quantity"]}  Status: {row["status"]}";
                lbl.ForeColor = Color.Black;
                lbl.Font = new Font("Arial", 10, FontStyle.Bold);
                lbl.Location = new Point(10, 10);
                lbl.AutoSize = true;


                int orderId = Convert.ToInt32(row["id"]);
                orderPanel.Click += (s, e) => ToggleOrderStatus(orderId, orderPanel, lbl);
                lbl.Click += (s, e) => ToggleOrderStatus(orderId, orderPanel, lbl);

                orderPanel.Controls.Add(lbl);
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