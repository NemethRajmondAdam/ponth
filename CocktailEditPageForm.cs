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
    public partial class CocktailEditPageForm : FormBase
    {
        private DataTable ingridients;
        private DataTable drinks;

        public CocktailEditPageForm(DataTable ing, DataTable dr)
        {
            InitializeComponent();
            this.ingridients = ing;
            this.drinks = dr;
        }

        private void CocktailEditPageForm_Load(object sender, EventArgs e)
        {
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
                UpdateQuantity(itemId, quantity, lblQuantity, lblPrice, price, isDrink);
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
                UpdateQuantity(itemId, -quantity, lblQuantity, lblPrice, price, isDrink);
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
        }
        private void DisplayItemsAsCards()
        {
            flowLayoutPanelIngridients.Controls.Clear();

            Panel drinkHeader = CreateHeader("Italok");
            flowLayoutPanelIngridients.Controls.Add(drinkHeader);

            foreach (DataRow row in drinks.Rows)
            {
                int quantity = row["quantity"] != DBNull.Value ? Convert.ToInt32(row["quantity"]) : 0;
                int price = row["price"] != DBNull.Value ? Convert.ToInt32(row["price"]) : 0;
                string name = row["name"].ToString();
                string quantityType = row["quantity_type"].ToString();
                int id = row["ID"] != DBNull.Value ? Convert.ToInt32(row["ID"]) : 0;

                Panel drinkCard = CreateItemCard(id, name, price, quantity, quantityType, true);
                flowLayoutPanelIngridients.Controls.Add(drinkCard);
            }

            Panel ingredientHeader = CreateHeader("Összetevők");
            flowLayoutPanelIngridients.Controls.Add(ingredientHeader);

            foreach (DataRow row in ingridients.Rows)
            {
                int quantity = row["quantity"] != DBNull.Value ? Convert.ToInt32(row["quantity"]) : 0;

                int price = row["price"] != DBNull.Value ? Convert.ToInt32(row["price"]) : 0;
                string name = row["name"].ToString();
                string quantityType = row["quantity_type"].ToString();
                int id = row["ID"] != DBNull.Value ? Convert.ToInt32(row["ID"]) : 0;


                Panel ingredientCard = CreateItemCard(id, name, price, quantity, quantityType, false);
                flowLayoutPanelIngridients.Controls.Add(ingredientCard);
            }
        }











    }
}
