using ponth.CostumeControls;
using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ponth
{
    public partial class ucOrdering : UserControl
    {
        public event Action<int> OrderingConfirmed;
        public event Action CanceledOrder;

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

            DrinksPanel.FlowDirection = FlowDirection.TopDown;
            DrinksPanel.WrapContents = false;
            DrinksPanel.Margin = new Padding(50, 0, 0, 0);


            LoadDrinks();
            RefreshCart();
        }

        // ITALOK BETÖLTÉSE ADATBÁZISBÓL
        private void LoadDrinks()
        {
            DrinksPanel.Controls.Clear();

            DataTable dt = DatabaseHelper.GetData(
                "SELECT * FROM drinks WHERE id <> '1' ORDER BY type,name");

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

        // VIEW FELÉPÍTÉSE (KÁRTYÁK & FEJLÉCEK)

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

            cButtons btnOrder = new cButtons()
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

        // KOSÁR

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
                Height = 150  // Megnöveltük a panel magasságát, hogy a gomboknak több helye legyen
            };

            CartPanel.Controls.Add(itemsPanel);
            CartPanel.Controls.Add(bottomPanel);

            int y = 40;

            foreach (var item in cart.Items)
            {
                Label lbl = new Label()
                {
                    Text = $"{item.Name} ({item.Quantity}x) - {item.Price * item.Quantity} Ft",
                    Location = new Point(10, y),
                    AutoSize = true
                };
                Button btnRemoveOne = new Button()
                {
                    Text = "-",
                    Tag = item.Id,
                    Size = new Size(35, 25),
                    Location = new Point(itemsPanel.ClientSize.Width - 80, y - 3)
                };
                btnRemoveOne.Click += (s, e) =>
                {
                    int id = (int)((Button)s).Tag;
                    cart.RemoveOne(id);
                    RefreshCart();
                };
                itemsPanel.Controls.Add(btnRemoveOne);

                Button btnRemoveAll = new Button()
                {
                    Text = "X",
                    Tag = item.Id,
                    Size = new Size(35, 25),
                    Location = new Point(itemsPanel.ClientSize.Width - 40, y - 3)
                };
                btnRemoveAll.Click += (s, e) =>
                {
                    int id = (int)((Button)s).Tag;
                    cart.RemoveItem(id);
                    RefreshCart();
                };
                itemsPanel.Controls.Add(btnRemoveAll);

                itemsPanel.Controls.Add(lbl);
                itemsPanel.Controls.Add(btnRemoveOne);
                itemsPanel.Controls.Add(btnRemoveAll);
                y += 30;
            }

            Label totalLabel = new Label()
            {
                Text = $"Összesen: {cart.TotalPrice()} Ft",
                Location = new Point(10, 10),  // A végösszeg felirat feljebb helyezése
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            cButtons btnOrder = new cButtons()
            {
                Text = "Rendelés",
                Width = 120,
                Height = 35,
                Location = new Point(10, 40)  // A rendelés gomb feljebb helyezése
            };

            btnOrder.Click += (s, e) =>
            {
                // Az OrderingConfirmed esemény meghívása
                //OrderingConfirmed?.Invoke(currentTableId);

                //MessageBox.Show("Rendelés leadva!");
                //cart.Clear();
                //RefreshCart();

                Order();
            };

            //btnOrder.Click += (s, e) =>
            //{
            //    MessageBox.Show("Rendelés leadva!");
            //    cart.Clear();
            //    RefreshCart();
            //};

            cButtons btnClear = new cButtons()
            {
                Text = "Kosár törlése",
                Width = 120,
                Height = 35,
                Location = new Point(140, 40)
            };

            btnClear.Click += (s, e) =>
            {
                cart.Clear();
                RefreshCart();
            };

            Button btnCancelCart = new Button()
            {
                Text = "Mégse",
                Width = 120,
                Height = 35,
                Location = new Point(270, 40),
                DialogResult = DialogResult.Cancel,
            };

            btnCancelCart.Click += (s, e) =>
            {
                CanceledOrder?.Invoke();
            };

            bottomPanel.Controls.Add(totalLabel);
            bottomPanel.Controls.Add(btnOrder);
            bottomPanel.Controls.Add(btnClear);
            bottomPanel.Controls.Add(btnCancelCart);
        }

        public static DataTable ingridients;
        public static DataTable drinks;

        //ITALOK EGYEDIVE ALAKITASA
        private void editCocktailIngredients(object sender, EventArgs e, int drinkId)
        {
            DataTable cocktailTable = DatabaseHelper.GetData(
                "SELECT id FROM cocktails WHERE drink_id=" + drinkId);

            if (cocktailTable.Rows.Count == 0)
                return;

            int cocktailId = Convert.ToInt32(cocktailTable.Rows[0]["id"]);

            DataTable ingredientsInCocktail = DatabaseHelper.GetData("SELECT * FROM cocktail_ingredients WHERE cocktail_id=" + cocktailId);

            DataTable drinksInCocktail = DatabaseHelper.GetData("SELECT * FROM cocktail_drinks WHERE cocktail_id=" + cocktailId);

            ingridients = new DataTable();
            drinks = new DataTable();

            ingridients.Columns.Add("ID", typeof(int));
            ingridients.Columns.Add("name", typeof(string));
            ingridients.Columns.Add("quantity", typeof(int));
            ingridients.Columns.Add("quantity_type", typeof(string));
            ingridients.Columns.Add("price", typeof(int));

            drinks.Columns.Add("ID", typeof(int));
            drinks.Columns.Add("name", typeof(string));
            drinks.Columns.Add("quantity", typeof(int));
            drinks.Columns.Add("quantity_type", typeof(string));
            drinks.Columns.Add("price", typeof(int));

            foreach (DataRow row in ingredientsInCocktail.Rows)
            {
                DataTable ingredientDetails =
                    DatabaseHelper.GetData("SELECT * FROM ingredients WHERE id=" + row["ingredient_id"]);

                if (ingredientDetails.Rows.Count == 0)
                    continue;

                ingridients.Rows.Add(
                    ingredientDetails.Rows[0]["id"],
                    ingredientDetails.Rows[0]["name"],
                    row["quantity"],
                    "", // NINCS quantity_type oszlop → üres string
                    ingredientDetails.Rows[0]["price"]);

            }

            foreach (DataRow row in drinksInCocktail.Rows)
            {
                DataTable drinkDetails =
                    DatabaseHelper.GetData("SELECT * FROM drinks WHERE id=" + row["drink_id"]);

                if (drinkDetails.Rows.Count == 0)
                    continue;

                drinks.Rows.Add(
                    drinkDetails.Rows[0]["id"],
                    drinkDetails.Rows[0]["name"],
                    row["quantity"],
                    "", // NINCS quantity_type oszlop → üres string
                    drinkDetails.Rows[0]["mixing_price"]);

            }

            DataTable allIngredients = DatabaseHelper.GetData("SELECT * FROM ingredients");

            foreach (DataRow row in allIngredients.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                bool exists = false;

                foreach (DataRow existing in ingridients.Rows)
                {
                    if (Convert.ToInt32(existing["ID"]) == id)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    ingridients.Rows.Add(
                        row["id"],
                        row["name"],
                        0,
                        "", // quantity_type nincs → üres
                        row["price"]);

                }
            }

            DataTable allDrinks = DatabaseHelper.GetData("SELECT * FROM drinks");

            foreach (DataRow row in allDrinks.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                bool exists = false;

                foreach (DataRow existing in drinks.Rows)
                {
                    if (Convert.ToInt32(existing["ID"]) == id)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    drinks.Rows.Add(
                        row["id"],
                        row["name"],
                        0,
                        "", // quantity_type nincs → üres
                        row["mixing_price"]);

                }
            }

            CocktailEditPageForm frm = new CocktailEditPageForm(ingridients, drinks);
            frm.ShowDialog();
        }


        //(EXTRA NELKULI) RENDELES FELTOLTESE ADATBAZISBA

        private int CurrentSUM(int openBillId)
        {
            string sql = $"SELECT totalSum FROM open_bills WHERE id = {openBillId}";
            DataTable dt = DatabaseHelper.GetData(sql);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["totalSum"]);
            }

            return 0;
        }

        private int GetOrCreateOpenBillId()
        {
            string sql = $"SELECT id FROM open_bills WHERE box_id = {currentTableId}";

            if (DatabaseQuestions.IsDataExists(sql))
            {
                DataTable dt = DatabaseHelper.GetData(sql);
                return Convert.ToInt32(dt.Rows[0]["id"]);
            }
            else
            {
                string insert = $"INSERT INTO open_bills (box_id,totalSum) VALUES ({currentTableId},0)";
                DatabaseHelper.ExecuteNonQuery(insert);

                string getId = $"SELECT id FROM open_bills WHERE box_id = {currentTableId}";
                DataTable dt = DatabaseHelper.GetData(getId);
                return Convert.ToInt32(dt.Rows[0]["id"]);
            }
        }

        private void Order()
        {
            int openBillId = GetOrCreateOpenBillId();
            int boxSum = CurrentSUM(openBillId);

            foreach (var item in cart.Items)
            {
                int totalItemPrice = item.Price * item.Quantity;

                string sql = $"INSERT INTO orders (bills_id, item_id, quantity, subtotal) " +
                             $"VALUES ({openBillId}, {item.Id}, {item.Quantity}, {totalItemPrice})";

                DatabaseHelper.ExecuteNonQuery(sql);
            }

            int totalPrice = cart.TotalPrice();
            boxSum += totalPrice;

            string updateQuery = $"UPDATE open_bills SET totalSum = {boxSum} WHERE id = {openBillId}";
            DatabaseHelper.ExecuteUpdate(updateQuery);

            OrderingConfirmed?.Invoke(currentTableId);

            MessageBox.Show($"A rendelés összértéke: {totalPrice} Ft\nAz aktuális összeg: {boxSum} Ft");

            cart.Clear();
            RefreshCart();
        }
    }
}
