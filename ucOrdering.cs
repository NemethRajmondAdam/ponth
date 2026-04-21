using ponth.CostumeControls;
using System;
using System.Collections.Generic;
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
                    Text = "There is no drink in the database.",
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
                Text = $"Type: {type}",
                Font = new Font("Segoe UI", 11, FontStyle.Italic),
                ForeColor = Color.DimGray,
                Location = new Point(10, 75),
                AutoSize = true
            };

            cButtons btnOrder = new cButtons()
            {
                Text = "Order",
                Location = new Point(10, 115),
                Tag = id,
                Width = 120,
                Height = 35,
                BackColor = Color.FromArgb(228, 197, 114),
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
                    Text = "Costumize",
                    Location = new Point(150, 125),
                    AutoSize = true,
                    Tag = id,
                    LinkColor = Color.FromArgb(228, 197, 114),
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
                Height = 150
            };

            CartPanel.Controls.Add(itemsPanel);
            CartPanel.Controls.Add(bottomPanel);

            int y = 40;

            foreach (var item in cart.Items)
            {
                int cartKey = cart.GetKey(item);

                // Main item label — show extras count if any
                string mainText = $"{item.Name} ({item.Quantity}x) - {item.TotalItemPrice} Ft";
                if (item.Extras.Count > 0)
                    mainText += $"  (+{item.Extras.Count} extra)";

                Label lbl = new Label()
                {
                    Text = mainText,
                    Location = new Point(10, y),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                };
                Button btnRemoveOne = new Button()
                {
                    Text = "-",
                    Tag = cartKey,
                    Size = new Size(35, 25),
                    Location = new Point(itemsPanel.ClientSize.Width - 80, y - 3)
                };
                btnRemoveOne.Click += (s, e) =>
                {
                    int key = (int)((Button)s).Tag;
                    cart.RemoveOne(key);
                    RefreshCart();
                };
                itemsPanel.Controls.Add(btnRemoveOne);

                Button btnRemoveAll = new Button()
                {
                    Text = "X",
                    Tag = cartKey,
                    Size = new Size(35, 25),
                    Location = new Point(itemsPanel.ClientSize.Width - 40, y - 3)
                };
                btnRemoveAll.Click += (s, e) =>
                {
                    int key = (int)((Button)s).Tag;
                    cart.RemoveItem(key);
                    RefreshCart();
                };
                itemsPanel.Controls.Add(btnRemoveAll);

                itemsPanel.Controls.Add(lbl);
                itemsPanel.Controls.Add(btnRemoveOne);
                itemsPanel.Controls.Add(btnRemoveAll);
                y += 30;

                // Show extras as indented sub-items
                foreach (var extra in item.Extras)
                {
                    Label lblExtra = new Label()
                    {
                        Text = $"  + {extra.Name} ({extra.Quantity}x, {extra.Type}) - {extra.Price * extra.Quantity} Ft",
                        Location = new Point(25, y),
                        AutoSize = true,
                        ForeColor = Color.DarkGreen,
                        Font = new Font("Segoe UI", 8, FontStyle.Italic)
                    };
                    itemsPanel.Controls.Add(lblExtra);
                    y += 22;
                }
            }

            Label totalLabel = new Label()
            {
                Text = $"Total: {cart.TotalPrice()} Ft",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            cButtons btnOrderCart = new cButtons()
            {
                Text = "Order",
                Width = 120,
                Height = 35,
                Location = new Point(10, 40),
                BackColor = Color.FromArgb(228, 197, 114),
            };

            btnOrderCart.Click += (s, e) =>
            {
                Order();
            };

            cButtons btnClear = new cButtons()
            {
                Text = "Delete cart",
                Width = 120,
                Height = 35,
                Location = new Point(140, 40),
                BackColor = Color.FromArgb(228, 197, 114),
            };

            btnClear.Click += (s, e) =>
            {
                cart.Clear();
                RefreshCart();
            };

            Button btnCancelCart = new Button()
            {
                Text = "Cancel",
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
            bottomPanel.Controls.Add(btnOrderCart);
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

            DataTable ingredientsInCocktail = DatabaseHelper.GetData(
                "SELECT * FROM cocktail_ingredients WHERE cocktail_id=" + cocktailId);

            DataTable drinksInCocktail = DatabaseHelper.GetData(
                "SELECT * FROM cocktail_drinks WHERE cocktail_id=" + cocktailId);

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

            //INGREDIENTS A KOKTÉLBAN
            foreach (DataRow row in ingredientsInCocktail.Rows)
            {
                DataTable ingredientDetails =
                    DatabaseHelper.GetData(
                        "SELECT i.*, q.quantity AS quantity_type " +
                        "FROM ingredients i " +
                        "LEFT JOIN quantities q ON i.quantity_id = q.id " +
                        "WHERE i.id=" + row["ingredient_id"]);

                if (ingredientDetails.Rows.Count == 0)
                    continue;

                ingridients.Rows.Add(
                    ingredientDetails.Rows[0]["id"],
                    ingredientDetails.Rows[0]["name"],
                    row["quantity"],
                    ingredientDetails.Rows[0]["quantity_type"],
                    ingredientDetails.Rows[0]["price"]);
            }

            //DRINKS A KOKTÉLBAN
            foreach (DataRow row in drinksInCocktail.Rows)
            {
                DataTable drinkDetails =
                    DatabaseHelper.GetData(
                        "SELECT d.*, q.quantity AS quantity_type " +
                        "FROM drinks d " +
                        "LEFT JOIN quantities q ON d.quantity_id = q.id " +
                        "WHERE d.id=" + row["drink_id"]);

                if (drinkDetails.Rows.Count == 0)
                    continue;

                drinks.Rows.Add(
                    drinkDetails.Rows[0]["id"],
                    drinkDetails.Rows[0]["name"],
                    row["quantity"],
                    drinkDetails.Rows[0]["quantity_type"],
                    drinkDetails.Rows[0]["mixing_price"]);
            }

            //ÖSSZES INGREDIENT
            DataTable allIngredients = DatabaseHelper.GetData(
                "SELECT i.*, q.quantity AS quantity_type " +
                "FROM ingredients i " +
                "LEFT JOIN quantities q ON i.quantity_id = q.id");

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
                        row["quantity_type"],
                        row["price"]);
                }
            }

            //ÖSSZES DRINK
            DataTable allDrinks = DatabaseHelper.GetData(
                "SELECT d.*, q.quantity AS quantity_type " +
                "FROM drinks d " +
                "LEFT JOIN quantities q ON d.quantity_id = q.id");

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
                        row["quantity_type"],
                        row["mixing_price"]);
                }
            }

            CocktailEditPageForm frm = new CocktailEditPageForm(ingridients, drinks);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Get drink info for the cart
                DataTable drinkInfo = DatabaseHelper.GetData(
                    $"SELECT name, price FROM drinks WHERE id = {drinkId}");

                if (drinkInfo.Rows.Count > 0)
                {
                    string drinkName = drinkInfo.Rows[0]["name"].ToString();
                    int drinkPrice = Convert.ToInt32(drinkInfo.Rows[0]["price"]);

                    cart.AddItemWithExtras(drinkId, drinkName, drinkPrice, "Cocktail", frm.CollectedExtras);
                    RefreshCart();
                }
            }
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
            string sql = $"SELECT id FROM open_bills " +
                         $"WHERE box_id = {currentTableId} AND user_id IS NULL";

            if (DatabaseQuestions.IsDataExists(sql))
            {
                DataTable dt = DatabaseHelper.GetData(sql);
                return Convert.ToInt32(dt.Rows[0]["id"]);
            }
            else
            {
                string insert =
                    $"INSERT INTO open_bills (box_id, totalSum, user_id) " +
                    $"VALUES ({currentTableId}, 0, NULL)";

                DatabaseHelper.ExecuteNonQuery(insert);

                string getId =
                    $"SELECT id FROM open_bills " +
                    $"WHERE box_id = {currentTableId} AND user_id IS NULL " +
                    $"ORDER BY id DESC LIMIT 1 ";

                DataTable dt = DatabaseHelper.GetData(getId);
                return Convert.ToInt32(dt.Rows[0]["id"]);
            }
        }

        private bool isItForAPerson()
        {
            string sql =
                $"SELECT user_id FROM open_bills WHERE box_id = {currentTableId}";

            if (DatabaseQuestions.IsDataExists(sql))
            {
                DataTable dt = DatabaseHelper.GetData(sql);

                return dt.Rows[0]["user_id"] != DBNull.Value;
            }

            return false;
        }

        private void Order()
        {
            int openBillId = GetOrCreateOpenBillId();
            int boxSum = CurrentSUM(openBillId);

            foreach (var item in cart.Items)
            {
                int totalItemPrice = item.TotalItemPrice;

                string sql =
                    $"INSERT INTO orders (bills_id, item_id, quantity, subtotal) " +
                    $"VALUES ({openBillId}, {item.Id}, {item.Quantity}, {totalItemPrice})";

                DatabaseHelper.ExecuteNonQuery(sql);

                // Save extras to orders_extra table
                if (item.Extras.Count > 0)
                {
                    // Get the newly inserted order's ID
                    object lastId = DatabaseHelper.ExecuteScalar("SELECT LAST_INSERT_ID()");
                    int orderId = Convert.ToInt32(lastId);

                    foreach (var extra in item.Extras)
                    {
                        int extraTotalPrice = extra.Price * extra.Quantity;
                        string extraSql =
                            $"INSERT INTO orders_extra (order_id, item_id, type, box_detail_id, quantity, price) " +
                            $"VALUES ({orderId}, {extra.ItemId}, '{extra.Type}', {currentTableId}, {extra.Quantity}, {extraTotalPrice})";

                        DatabaseHelper.ExecuteNonQuery(extraSql);
                    }
                }
            }

            int totalPrice = cart.TotalPrice();
            boxSum += totalPrice;

            string updateQuery =
                $"UPDATE open_bills SET totalSum = {boxSum} WHERE id = {openBillId}";

            DatabaseHelper.ExecuteUpdate(updateQuery);

            OrderingConfirmed?.Invoke(currentTableId);

            MessageBox.Show(
                $"Total: {totalPrice} Ft\n" +
                $"Current : {boxSum} Ft");

            cart.Clear();
            RefreshCart();
        }
    }
}
