using System;
using System.Data;
using System.Windows.Forms;

namespace ponth
{
    public partial class ucReservations : UserControl
    {
        public ucReservations()
        {
            InitializeComponent();
            
            LoadData();
        }


        private void LoadData(string name = "")
        {
            string query = @"SELECT 
                        name AS 'Name',
                        phone AS 'Phone number',
                        reservation_date AS 'Date',
                        reservation_time AS 'Time',
                        duration_minutes AS 'Duration time',
                        boxes_id AS 'Table',
                        status AS 'Status'
                     FROM reservations";

            if (!string.IsNullOrEmpty(name))
            {
                query += $" WHERE name LIKE '%{name}%'";
            }

            dataGridView.DataSource = DatabaseHelper.GetData(query);

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            LoadData(txtBox_SearchBar.Text);
        }

        private void txtBox_SearchBar_TextChanged(object sender, EventArgs e)
        {
            LoadData(txtBox_SearchBar.Text);
        }
    }
}