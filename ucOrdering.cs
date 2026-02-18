using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ponth.CostumeControls;
using System.Data.SqlClient;

namespace ponth
{
    public partial class ucOrdering : UserControl
    {
        public event Action<int> OrderingConfirmed;

        private int currentTableId = 0;

        public ucOrdering(int tableId)
        {
            InitializeComponent();
            currentTableId = tableId;
        }
        Cart cart = new Cart();

        private void ucOrdering_Load(object sender, EventArgs e)
        {
            LoadDrinks();
            RefreshCart();
        }

        private void AddToCart(int id, string name, int price, string type)
        {
            cart.AddItem(id, name, price, type);
            RefreshCart();
        }

        private void btnOrderCart_Click(object sender, EventArgs e)
        {
            // Rendelés leadása, átadjuk a tableId-t
            OrderingConfirmed?.Invoke(currentTableId);
        }

        private void RefreshCart()
        {
            CartPanel.Controls.Clear();
            int panelWidth = CartPanel.ClientSize.Width;

            Panel itemsPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            CartPanel.Controls.Add(itemsPanel);
            itemsPanel.BringToFront();

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

                cButtons btnRemoveOne = new cButtons()
                {
                    Text = "-",
                    Tag = item.Id,
                    Size = new Size(35, 25),
                    Location = new Point(itemsPanel.ClientSize.Width - 80, y - 3),
                    BorderRadius = 10,
                    BackgroundColor = Color.IndianRed,
                    TextColor = Color.White
                };
                btnRemoveOne.Click += (s, e) =>
                {
                    int id = (int)((cButtons)s).Tag;
                    cart.RemoveOne(id);
                    RefreshCart();
                };
                itemsPanel.Controls.Add(btnRemoveOne);

                cButtons btnRemoveAll = new cButtons()
                {
                    Text = "X",
                    Tag = item.Id,
                    Size = new Size(35, 25),
                    Location = new Point(itemsPanel.ClientSize.Width - 40, y - 3),
                    BorderRadius = 10,
                    BackgroundColor = Color.DarkRed,
                    TextColor = Color.White
                };
                btnRemoveAll.Click += (s, e) =>
                {
                    int id = (int)((cButtons)s).Tag;
                    cart.RemoveItem(id);
                    RefreshCart();
                };
                itemsPanel.Controls.Add(btnRemoveAll);

                y += 30;
            }

            Panel bottomPanel = new Panel()
            {
                Dock = DockStyle.Bottom,
                Height = 70
            };
            CartPanel.Controls.Add(bottomPanel);

            Label totalLabel = new Label()
            {
                Text = $"Összesen: {cart.TotalPrice()} Ft",
                Location = new Point(10, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            itemsPanel.Controls.Add(totalLabel);

            cButtons btnOrderCart = new cButtons()
            {
                Text = "Rendelés",
                Width = panelWidth / 2 - 10,
                Height = 35,
                Location = new Point(0, 35),
                BorderRadius = 15,
                BackgroundColor = Color.MediumSeaGreen,
                TextColor = Color.White
            };
            btnOrderCart.Click += (s, e) =>
            {
                MessageBox.Show("Rendelés leadva!");
                cart.Clear();
            };
            bottomPanel.Controls.Add(btnOrderCart);

            cButtons btnCancelCart = new cButtons()
            {
                Text = "Mégse",
                Width = panelWidth / 2 - 10,
                Height = 35,
                Location = new Point(panelWidth / 2, 35),
                BorderRadius = 15,
                BackgroundColor = Color.Gray,
                TextColor = Color.White
            };
            btnCancelCart.Click += (s, e) =>
            {
                var parentForm = this.FindForm();
                if (parentForm != null)
                    parentForm.Close();
            };
            bottomPanel.Controls.Add(btnCancelCart);

            cButtons btnClearCart = new cButtons()
            {
                Text = "Összes törlése",
                Width = panelWidth - 20,
                Height = 30,
                Location = new Point(10, 5),
                BorderRadius = 15,
                BackgroundColor = Color.DarkOrange,
                TextColor = Color.White
            };
            btnClearCart.Click += (s, e) =>
            {
                cart.Clear();
                RefreshCart();
            };
            bottomPanel.Controls.Add(btnClearCart);
        }

        private void LoadDrinks()
        {
            DrinksPanel.Controls.Clear();

            string currentType = null;

            // Join lekérdezés
            string sql = @"
                SELECT d.id, d.name, d.price, d.type
                FROM drinks d
                ORDER BY d.type";

            DataTable dt = DatabaseHelper.GetData(sql);

            foreach (DataRow row in dt.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string name = row["name"].ToString();
                int price = Convert.ToInt32(row["price"]);
                string type = row["type"].ToString();

                if (currentType != type)
                {
                    currentType = type;
                    DrinksPanel.Controls.Add(CreateHeader(type));
                }

                DrinksPanel.Controls.Add(CreateDrinkCard(id, name, price, type));
            }
        }

        private Panel CreateHeader(string type)
        {
            Panel header = new Panel
            {
                Width = DrinksPanel.ClientSize.Width - 40,
                Height = 40,
                Margin = new Padding(5, 20, 5, 5),
                BackColor = Color.FromArgb(230, 230, 230)
            };

            Label lbl = new Label
            {
                Text = type.ToUpper(),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(10, 5),
                AutoSize = true
            };

            header.Controls.Add(lbl);
            return header;
        }

        private Panel CreateDrinkCard(int id, string name, int price, string type)
        {
            Panel card = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                Width = DrinksPanel.ClientSize.Width / 3 - 30,
                Height = 180,
                Margin = new Padding(10),
                BackColor = Color.WhiteSmoke
            };

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

            cButtons btnOrder = new cButtons()
            {
                Text = "Rendelés",
                Location = new Point(10, 115),
                Tag = id,
                Width = 130,
                Height = 40,
                BorderRadius = 20,
                BackgroundColor = Color.MediumSlateBlue,
                TextColor = Color.White
            };
            btnOrder.Click += (s, e) =>
            {
                cButtons b = s as cButtons;
                int drinkId = (int)b.Tag;
                AddToCart(drinkId, name, price, type);
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
                    LinkLabel link = s as LinkLabel;
                    if (link == null) return;

                    int drinkId = (int)link.Tag;
                    editCocktailIngredients(s, e, drinkId);
                };

                card.Controls.Add(linkEdit);
            }

            return card;
        }

        private void editCocktailIngredients(object sender, EventArgs e, int drinkId)
        {
            // Ingredients lekérdezés
            string sqlIngredients = $@"
        SELECT 
            i.id, i.name, ci.quantity, q.quantity AS quantity_type, i.price
        FROM cocktails c
        INNER JOIN cocktail_ingredients ci ON ci.cocktail_id = c.id
        INNER JOIN ingredients i ON i.id = ci.ingredient_id
        INNER JOIN quantities q ON q.id = i.quantity_id
        WHERE c.drink_id = {drinkId}";

            DataTable ingredients = DatabaseHelper.GetData(sqlIngredients);

            // Drinks lekérdezés (eredeti kód szerint)
            string sqlDrinks = $@"
        SELECT d.id, d.type, d.name, d.mixing_price AS price, cd.quantity, q.quantity AS quantity_type
        FROM cocktails c
        INNER JOIN cocktail_drinks cd ON cd.cocktail_id = c.id
        INNER JOIN drinks d ON d.id = cd.drink_id
        INNER JOIN quantities q ON q.id = d.mixing_quantity_id
        WHERE c.drink_id = {drinkId}";

            DataTable drinks = DatabaseHelper.GetData(sqlDrinks);

            // CocktailEditPageForm két paraméterrel
            CocktailEditPageForm frm = new CocktailEditPageForm(ingredients, drinks);
            frm.ShowDialog();
        }

    }
}

