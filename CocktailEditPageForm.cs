using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ponth
{
    public partial class CocktailEditPageForm : Form
    {
        private DataTable ingridients;
        private DataTable drinks;

        // Tracking quantities for extras calculation
        private Dictionary<string, int> originalQuantities = new Dictionary<string, int>();
        private Dictionary<string, int> currentQuantities = new Dictionary<string, int>();
        private Dictionary<string, string> itemNames = new Dictionary<string, string>();
        private Dictionary<string, int> itemPrices = new Dictionary<string, int>();

        private HashSet<int> cocktailDrinkIds = new HashSet<int>();
        private HashSet<int> cocktailIngredientIds = new HashSet<int>();

        public List<CartExtra> CollectedExtras { get; private set; } = new List<CartExtra>();

        public CocktailEditPageForm(DataTable ing, DataTable dr)
        {
            InitializeComponent();
            this.ingridients = ing;
            this.drinks = dr;
        }

        private void CocktailEditPageForm_Load(object sender, EventArgs e)
        {
            // Add bottom panel with "Add to cart" button
            Panel bottomPanel = new Panel()
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            Button btnAddToCart = new Button()
            {
                Text = "Add to cart",
                Width = 220,
                Height = 40,
                Location = new Point(10, 10),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
            };
            btnAddToCart.Click += btnAddToCart_Click;

            Button btnCancel = new Button()
            {
                Text = "Cancel",
                Width = 120,
                Height = 40,
                Location = new Point(240, 10),
                BackColor = Color.FromArgb(180, 180, 180),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 11),
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += (s, ev) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            bottomPanel.Controls.Add(btnAddToCart);
            bottomPanel.Controls.Add(btnCancel);
            this.Controls.Add(bottomPanel);

            // Ensure the flow panel fills the remaining space above the bottom panel
            flowLayoutPanelIngridients.BringToFront();

            DisplayItemsAsCards();
        }

        private Panel CreateHeader(string type)
        {
            Panel header = new Panel();
            header.Width = flowLayoutPanelIngridients.ClientSize.Width - 40;
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

        private Panel CreateItemCard(int id, string name, int price, int quantity, string quantityType, bool isDrink)
        {

            Panel card = new Panel()
            {
                BorderStyle = BorderStyle.FixedSingle,
                Width = flowLayoutPanelIngridients.ClientSize.Width / 3 - 30,
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
                Text = $"{price * quantity} Ft",
                Font = new Font("Segoe UI", 12),
                Location = new Point(10, 45),
                AutoSize = true
            };

            Label lblQuantity = new Label()
            {
                Text = $"{quantity} {quantityType}",
                Font = new Font("Segoe UI", 12),
                Location = new Point(10, 75),
                AutoSize = true
            };

            Button btnIncrease = new Button()
            {
                Text = "+",
                Location = new Point(10, 115),
                Width = 50,
                Height = 35,
                Tag = id
            };
            btnIncrease.Click += (s, e) =>
            {
                Button btn = s as Button;
                int itemId = (int)btn.Tag;
                if (quantity != 0)
                {
                    UpdateQuantity(itemId, quantity, lblQuantity, lblPrice, price, isDrink);
                }
                else 
                {
                    UpdateQuantity(itemId, 1, lblQuantity, lblPrice, price, isDrink);
                }
                
            };

            Button btnDecrease = new Button()
            {
                Text = "-",
                Location = new Point(70, 115),
                Width = 50,
                Height = 35,
                Tag = id
            };
            btnDecrease.Click += (s, e) =>
            {
                Button btn = s as Button;
                int itemId = (int)btn.Tag;
                if (quantity != 0)
                {
                    UpdateQuantity(itemId, -quantity, lblQuantity, lblPrice, price, isDrink);
                }
                else
                {
                    UpdateQuantity(itemId, -1, lblQuantity, lblPrice, price, isDrink);
                }
                
            };

            card.Controls.Add(lblName);
            card.Controls.Add(lblPrice);
            card.Controls.Add(lblQuantity);
            card.Controls.Add(btnIncrease);
            card.Controls.Add(btnDecrease);

            return card;
        }


        private void UpdateQuantity(int itemId, int change, Label lblQuantity, Label lblPrice, int price, bool isDrink)
        {
            int currentQuantity = int.Parse(lblQuantity.Text.Split(' ')[0]);
            int newQuantity = currentQuantity + change;

            if (newQuantity < 0) newQuantity = 0;

            int newPrice = price * newQuantity;

            lblQuantity.Text = $"{newQuantity} {lblQuantity.Text.Split(' ')[1]}";
            lblPrice.Text = $"{newPrice} Ft";

            // Update tracking dictionary
            string key = (isDrink ? "drink_" : "ingredient_") + itemId;
            currentQuantities[key] = newQuantity;
        }

        private void DisplayItemsAsCards()
        {
            flowLayoutPanelIngridients.Controls.Clear();
            originalQuantities.Clear();
            currentQuantities.Clear();
            itemNames.Clear();
            itemPrices.Clear();
            cocktailDrinkIds.Clear();
            cocktailIngredientIds.Clear();

            // === "TARTALMAZZA" (Contains) section ===
            Panel containsHeader = CreateHeader("Contains");
            flowLayoutPanelIngridients.Controls.Add(containsHeader);
            
            foreach (DataRow row in drinks.Rows)
            {
                int quantity = row["quantity"] != DBNull.Value ? Convert.ToInt32(row["quantity"]) : 0;
                if (quantity <= 0) continue;
                int price = Convert.ToInt32(row["price"]);
                string name = row["name"].ToString();
                string quantityType = row["quantity_type"].ToString();
                int id = Convert.ToInt32(row["ID"]);

                string key = "drink_" + id;
                originalQuantities[key] = quantity;
                currentQuantities[key] = quantity;
                itemNames[key] = name;
                itemPrices[key] = price;
                cocktailDrinkIds.Add(id);

                Panel drinkCard = CreateItemCard(id, name, price, quantity, quantityType, true);
                flowLayoutPanelIngridients.Controls.Add(drinkCard);
            }
            
            foreach (DataRow row in ingridients.Rows)
            {
                int quantity = row["quantity"] != DBNull.Value ? Convert.ToInt32(row["quantity"]) : 0;
                if (quantity <= 0) continue;
                int price = Convert.ToInt32(row["price"]);
                string name = row["name"].ToString();
                string quantityType = row["quantity_type"].ToString();
                int id = Convert.ToInt32(row["ID"]);

                string key = "ingredient_" + id;
                originalQuantities[key] = quantity;
                currentQuantities[key] = quantity;
                itemNames[key] = name;
                itemPrices[key] = price;
                cocktailIngredientIds.Add(id);

                Panel ingredientCard = CreateItemCard(id, name, price, quantity, quantityType, false);
                flowLayoutPanelIngridients.Controls.Add(ingredientCard);
            }

            // === "ITALOK" (Drinks) section — only drinks NOT already shown above ===
            Panel drinkHeader = CreateHeader("Drinks");
            flowLayoutPanelIngridients.Controls.Add(drinkHeader);

            foreach (DataRow row in drinks.Rows)
            {
                int id = row["ID"] != DBNull.Value ? Convert.ToInt32(row["ID"]) : 0;
                if (cocktailDrinkIds.Contains(id)) continue; // skip already shown

                int quantity = row["quantity"] != DBNull.Value ? Convert.ToInt32(row["quantity"]) : 0;
                int price = row["price"] != DBNull.Value ? Convert.ToInt32(row["price"]) : 0;
                string name = row["name"].ToString();
                string quantityType = row["quantity_type"].ToString();

                string key = "drink_" + id;
                originalQuantities[key] = quantity;
                currentQuantities[key] = quantity;
                itemNames[key] = name;
                itemPrices[key] = price;

                Panel drinkCard = CreateItemCard(id, name, price, quantity, quantityType, true);
                flowLayoutPanelIngridients.Controls.Add(drinkCard);
            }

            // === "ÖSSZETEVŐK" (Ingredients) section — only ingredients NOT already shown ===
            Panel ingredientHeader = CreateHeader("Ingredients");
            flowLayoutPanelIngridients.Controls.Add(ingredientHeader);

            foreach (DataRow row in ingridients.Rows)
            {
                int id = row["ID"] != DBNull.Value ? Convert.ToInt32(row["ID"]) : 0;
                if (cocktailIngredientIds.Contains(id)) continue; // skip already shown

                int quantity = row["quantity"] != DBNull.Value ? Convert.ToInt32(row["quantity"]) : 0;
                int price = row["price"] != DBNull.Value ? Convert.ToInt32(row["price"]) : 0;
                string name = row["name"].ToString();
                string quantityType = row["quantity_type"].ToString();

                string key = "ingredient_" + id;
                originalQuantities[key] = quantity;
                currentQuantities[key] = quantity;
                itemNames[key] = name;
                itemPrices[key] = price;

                Panel ingredientCard = CreateItemCard(id, name, price, quantity, quantityType, false);
                flowLayoutPanelIngridients.Controls.Add(ingredientCard);
            }
        }

        // Collect extras by comparing current quantities with originals
        private void CollectExtras()
        {
            CollectedExtras.Clear();

            foreach (var key in currentQuantities.Keys)
            {
                int current = currentQuantities[key];
                int original = originalQuantities.ContainsKey(key) ? originalQuantities[key] : 0;

                if (current > original)
                {
                    int extraQty = current - original;
                    string type = key.StartsWith("drink_") ? "Drink" : "Ingredient";
                    int itemId = int.Parse(key.Split('_')[1]);

                    CollectedExtras.Add(new CartExtra
                    {
                        ItemId = itemId,
                        Name = itemNames[key],
                        Type = type,
                        Quantity = extraQty,
                        Price = itemPrices[key]
                    });
                }
            }
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            CollectExtras();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
