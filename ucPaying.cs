using ponth.CostumeControls;
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
    public partial class ucPaying : UserControl
    {
        public ucPaying()
        {
            InitializeComponent();
        }

        DataTable tables;

        private void GetTables()
        {
            
        }

        private void btnDropdown_Click(object sender, EventArgs e)
        {
            List<string> items = new List<string>()
            {
                "Vodka", "Rum", "Tequila", "Gin", "Whiskey","asd","sddsd","valami"
            };

            cDropDown popup = new cDropDown(items);

            // A gomb alá pozicionáljuk
            var location = btnDropdown.PointToScreen(new Point(0, btnDropdown.Height));
            popup.Location = location;

            popup.ItemSelected += (value) =>
            {
                btnDropdown.Text = value;
            };

            popup.Show();
        }

    }
}
