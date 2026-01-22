using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ponth
{
    public partial class OrderingForm : Form
    {
        public OrderingForm()
        {
            InitializeComponent();
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            LoadDrinks();
            RefreshCart();
        }

        Cart cart = new Cart();

        private void AddToCart(int id, string name, int price, string type)
        {
            cart.AddItem(id, name, price, type);
            RefreshCart();
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

                y += 30;
            }

            //gombospanel
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

            Button btnOrderCart = new Button()
            {
                Text = "Rendelés",
                Width = panelWidth / 2 - 10,
                Height = 30,
                Location = new Point(0, 35)
            };
            btnOrderCart.Click += (s, e) =>
            {
                MessageBox.Show("Rendelés leadva!");
                cart.Clear();

            };
            bottomPanel.Controls.Add(btnOrderCart);

            Button btnCancelCart = new Button()
            {
                Text = "Mégse",
                Width = panelWidth / 2 - 10,
                Height = 30,
                Location = new Point(panelWidth / 2, 35),
                DialogResult = DialogResult.Cancel,
            };
            bottomPanel.Controls.Add(btnCancelCart);

            Button btnClearCart = new Button()
            {
                Text = "Összes törlése",
                Width = panelWidth - 20,
                Height = 25,
                Location = new Point(10, 5)
            };
            btnClearCart.Click += (s, e) =>
            {
                cart.Clear();
                RefreshCart();
            };
            bottomPanel.Controls.Add(btnClearCart);
        }
        private DataTable dt;

        private void LoadDrinks()
        {
            DrinksPanel.Controls.Clear();

            string currentType = null;

            dt = DatabaseHelper.GetData("SELECT * FROM drinks ORDER BY type");

            foreach (DataRow row in dt.Rows)
            {

                int id = Convert.ToInt32(row["id"]);
                string name = row["Name"].ToString();
                int price = Convert.ToInt32(row["price"]);
                string type = row["Type"].ToString();

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
            Panel header = new Panel();
            header.Width = DrinksPanel.ClientSize.Width - 40;
            header.Height = 40;
            header.Margin = new Padding(5, 20, 5, 5);
            header.BackColor = Color.FromArgb(230, 230, 230);

            Label lbl = new Label();
            lbl.Text = type.ToUpper();
            lbl.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lbl.Location = new Point(10, 5);
            lbl.AutoSize = true;

            header.Controls.Add(lbl);
            return header;
        }

        private Panel CreateDrinkCard(int id, string name, int price, string type)
        {
            Panel card = new Panel();
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Width = DrinksPanel.ClientSize.Width / 3 - 30;
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
                Button b = s as Button;
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
                    editCocktailIngredients(s,e,drinkId);
                };


                card.Controls.Add(linkEdit);
            }

            return card;
        }

        public static DataTable ingridients;
        public static DataTable drinks;
        

        private void editCocktailIngredients(object sender, EventArgs e,int drinkId)
        {
            //MessageBox.Show(drinkId.ToString());


            DataTable drinksInCocktails = DatabaseHelper.GetData("SELECT * FROM cocktail_drinks WHERE cocktail_id=(SELECT id FROM cocktails WHERE drink_id="+drinkId+")");
            DataTable ingridientsInCocktails = DatabaseHelper.GetData("SELECT * FROM cocktail_ingredients WHERE cocktail_id=(SELECT id FROM cocktails WHERE drink_id="+drinkId+")");

            ingridients = new DataTable();
            drinks = new DataTable();

            ingridients.Columns.Add("ID", typeof(int));
            ingridients.Columns.Add("name", typeof(string));
            ingridients.Columns.Add("quantity", typeof(int));
            ingridients.Columns.Add("quantity_type", typeof(string));
            ingridients.Columns.Add("price", typeof(int));

            drinks.Columns.Add("ID", typeof(int));
            drinks.Columns.Add("type", typeof(string));
            drinks.Columns.Add("name", typeof(string));
            drinks.Columns.Add("quantity", typeof(int));
            drinks.Columns.Add("quantity_type", typeof(string));
            drinks.Columns.Add("price", typeof(int));

            foreach (DataRow row in ingridientsInCocktails.Rows)
            {

                DataTable ingridientDetails = DatabaseHelper.GetData("SELECT * FROM ingredients WHERE id="+row["ingredient_id"]);

                //MessageBox.Show(ingridientDetails.Rows[0]["name"].ToString());
                //break;

                DataTable svQuantity = DatabaseHelper.GetData("SELECT * FROM quantities WHERE id=" + ingridientDetails.Rows[0]["quantity_id"]);

                //MessageBox.Show((ingridientDetails.Rows[0]["id"], ingridientDetails.Rows[0]["name"].ToString(), row["quantity"], svQuantity.Rows[0]["quantity"].ToString(), ingridientDetails.Rows[0]["price"]).ToString());
                //break;

                ingridients.Rows.Add(ingridientDetails.Rows[0]["id"], ingridientDetails.Rows[0]["name"].ToString(), row["quantity"], svQuantity.Rows[0]["quantity"].ToString(), ingridientDetails.Rows[0]["price"]);

            }


            string sql = $"SELECT d.id, d.type, d.name, d.mixing_price, cd.quantity,q.quantity  AS quantity_type FROM cocktails AS c INNER JOIN cocktail_drinks AS cd ON cd.cocktail_id = c.id INNER JOIN drinks AS d ON d.id = cd.drink_id INNER JOIN quantities AS q ON q.id = d.mixing_quantity_id WHERE c.drink_id = {drinkId}";
            DataTable drinkDetails = DatabaseHelper.GetData(sql);

            foreach (DataRow row in drinkDetails.Rows)
            {

                drinks.Rows.Add(row["id"], row["type"], row["name"], row["quantity"], row["quantity_type"], row["mixing_price"]);
                //drinks.Rows.Add(row["id"], row["type"], row["name"]);

            }

            CocktailEditPageForm frm = new CocktailEditPageForm(ingridients,drinks);
            frm.ShowDialog();

        }


    }
}
