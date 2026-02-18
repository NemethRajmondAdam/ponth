using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ponth
{
    public partial class ucOrdering : UserControl
    {
        public event Action<int> OrderingConfirmed;

        private int currentTableId = 0;
        Cart cart = new Cart();

        public ucOrdering(int tableId)
        {
            InitializeComponent();
            currentTableId = tableId;
            this.Load += ucOrdering_Load;
        }

        private void ucOrdering_Load(object sender, EventArgs e)
        {
            DrinksPanel.Dock = DockStyle.Fill;
            DrinksPanel.AutoScroll = true;

            // FONTOS JAVÍTÁS
            DrinksPanel.FlowDirection = FlowDirection.TopDown;
            DrinksPanel.WrapContents = false;

            LoadDrinks();
            RefreshCart();
        }

        // ===============================
        // ITALOK BETÖLTÉSE
        // ===============================
        private void LoadDrinks()
        {
            DrinksPanel.Controls.Clear();

            DataTable dt = DatabaseHelper.GetData(
                "SELECT * FROM drinks ORDER BY type, name");

            if (dt == null || dt.Rows.Count == 0)
            {
                Label empty = new Label()
                {
                    Text = "Nincs ital az adatbázisban!",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.Red
                };

                DrinksPanel.Controls.Add(empty);
                return;
            }

            string currentType = null;
            FlowLayoutPanel categoryPanel = null;

            foreach (DataRow row in dt.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string name = row["Name"].ToString();
                int price = Convert.ToInt32(row["price"]);
                string type = row["Type"].ToString();

                // ÚJ KATEGÓRIA
                if (currentType != type)
                {
                    currentType = type;

                    // HEADER
                    Panel header = CreateHeader(type);
                    header.Width = DrinksPanel.ClientSize.Width - 25;
                    DrinksPanel.Controls.Add(header);

                    // KATEGÓRIA PANEL (3 oszlopos)
                    categoryPanel = new FlowLayoutPanel();
                    categoryPanel.Width = DrinksPanel.ClientSize.Width - 25;
                    categoryPanel.AutoSize = true;
                    categoryPanel.WrapContents = true;
                    categoryPanel.FlowDirection = FlowDirection.LeftToRight;
                    categoryPanel.Margin = new Padding(5);

                    DrinksPanel.Controls.Add(categoryPanel);
                }

                if (categoryPanel != null)
                    categoryPanel.Controls.Add(CreateDrinkCard(id, name, price, type));
            }
        }

        private Panel CreateHeader(string type)
        {
            Panel header = new Panel();
            header.Height = 45;
            header.Margin = new Padding(5, 25, 5, 10);
            header.BackColor = Color.FromArgb(230, 230, 230);
            header.Dock = DockStyle.Top;

            Label lbl = new Label();
            lbl.Text = type.ToUpper();
            lbl.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lbl.Location = new Point(10, 8);
            lbl.AutoSize = true;

            header.Controls.Add(lbl);
            return header;
        }

        private Panel CreateDrinkCard(int id, string name, int price, string type)
        {
            int cardWidth = (DrinksPanel.ClientSize.Width / 3) - 35;
            if (cardWidth < 250)
                cardWidth = 250;

            Panel card = new Panel();
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Width = cardWidth;
            card.Height = 180;
            card.Margin = new Padding(10);
            card.BackColor = Color.WhiteSmoke;

            Label lblName = new Label()
            {
                Text = name,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            Label lblPrice = new Label()
            {
                Text = $"{price} Ft",
                Font = new Font("Segoe UI", 12),
                Location = new Point(10, 45),
                AutoSize = true
            };

            Label lblType = new Label()
            {
                Text = $"Típus: {type}",
                Font = new Font("Segoe UI", 11, FontStyle.Italic),
                ForeColor = Color.DimGray,
                Location = new Point(10, 75),
                AutoSize = true
            };

            Button btnOrder = new Button()
            {
                Text = "Rendelés",
                Location = new Point(10, 115),
                Tag = id,
                Width = 120,
                Height = 35
            };

            btnOrder.Click += (s, e) =>
            {
                AddToCart(id, name, price, type);
            };

            card.Controls.Add(lblName);
            card.Controls.Add(lblPrice);
            card.Controls.Add(lblType);
            card.Controls.Add(btnOrder);

            if (type == "Cocktail")
            {
                LinkLabel linkEdit = new LinkLabel()
                {
                    Text = "Szerkesztés",
                    Location = new Point(150, 125),
                    AutoSize = true,
                    Tag = id,
                };

                linkEdit.Click += (s, e) =>
                {
                    int drinkId = (int)((LinkLabel)s).Tag;
                    editCocktailIngredients(s, e, drinkId);
                };

                card.Controls.Add(linkEdit);
            }

            return card;
        }

        // ===============================
        // KOSÁR
        // ===============================
        private void AddToCart(int id, string name, int price, string type)
        {
            cart.AddItem(id, name, price, type);
            RefreshCart();
        }

        private void RefreshCart()
        {
            CartPanel.Controls.Clear();

            Panel itemsPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            Panel bottomPanel = new Panel()
            {
                Dock = DockStyle.Bottom,
                Height = 80
            };

            CartPanel.Controls.Add(itemsPanel);
            CartPanel.Controls.Add(bottomPanel);

            int y = 20;

            foreach (var item in cart.Items)
            {
                Label lbl = new Label()
                {
                    Text = $"{item.Name} ({item.Quantity}x) - {item.Price * item.Quantity} Ft",
                    Location = new Point(10, y),
                    AutoSize = true
                };

                itemsPanel.Controls.Add(lbl);
                y += 30;
            }

            Label totalLabel = new Label()
            {
                Text = $"Összesen: {cart.TotalPrice()} Ft",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            Button btnOrder = new Button()
            {
                Text = "Rendelés",
                Width = 120,
                Height = 35,
                Location = new Point(10, 40)
            };

            btnOrder.Click += (s, e) =>
            {
                MessageBox.Show("Rendelés leadva!");
                cart.Clear();
                RefreshCart();
            };

            bottomPanel.Controls.Add(totalLabel);
            bottomPanel.Controls.Add(btnOrder);
        }

        public static DataTable ingridients;
        public static DataTable drinks;

        private void editCocktailIngredients(object sender, EventArgs e, int drinkId)
        {
            DataTable ingridientsInCocktails =
                DatabaseHelper.GetData("SELECT * FROM cocktail_ingredients WHERE cocktail_id=(SELECT id FROM cocktails WHERE drink_id=" + drinkId + ")");

            ingridients = new DataTable();
            drinks = new DataTable();

            ingridients.Columns.Add("ID", typeof(int));
            ingridients.Columns.Add("name", typeof(string));
            ingridients.Columns.Add("quantity", typeof(int));
            ingridients.Columns.Add("quantity_type", typeof(string));
            ingridients.Columns.Add("price", typeof(int));

            foreach (DataRow row in ingridientsInCocktails.Rows)
            {
                DataTable ingridientDetails =
                    DatabaseHelper.GetData("SELECT * FROM ingredients WHERE id=" + row["ingredient_id"]);

                if (ingridientDetails.Rows.Count == 0)
                    continue;

                ingridients.Rows.Add(
                    ingridientDetails.Rows[0]["id"],
                    ingridientDetails.Rows[0]["name"],
                    row["quantity"],
                    "",
                    ingridientDetails.Rows[0]["price"]);
            }

            CocktailEditPageForm frm = new CocktailEditPageForm(ingridients, drinks);
            frm.ShowDialog();
        }
    }
}
