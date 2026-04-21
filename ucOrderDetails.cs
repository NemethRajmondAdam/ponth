using System;
using System.Data;
using System.Windows.Forms;

namespace ponth
{
    public partial class ucOrderDetails : UserControl
    {
        //int paraméteres event
        public event Action<int> OrderingConfirmed;

        //bezarasi metodus
        public event Action CancelRequested;


        public ucOrderDetails()
        {
            InitializeComponent();
            this.Load += ucOrderDetails_Load;
        }

        private void ucOrderDetails_Load(object sender, EventArgs e)
        {
            numUpDown_TableID.Value = 0;
            numUpDown_TableID.Enabled = false;
            txtBox_AccountHolder.Enabled = false;
            chckBox_Account.Enabled = true;
        }

        private void chckBox_OrderingToTable_CheckedChanged(object sender, EventArgs e)
        {
            if (chckBox_OrderingToTable.Checked)
            {
                numUpDown_TableID.Enabled = true;
                txtBox_AccountHolder.Enabled = false;
                chckBox_Account.Enabled = false;

            }
            else
            {
                numUpDown_TableID.Enabled = false;
                chckBox_Account.Enabled = true;
            }
        }

        private void chckBox_Account_CheckedChanged(object sender, EventArgs e)
        {
            txtBox_AccountHolder.Enabled = chckBox_Account.Checked;
        }


        private int numberOfTable()
        {
            string sql = "SELECT id FROM boxes ORDER BY id";
            DataTable dt = DatabaseHelper.GetData(sql);

            return dt.Rows.Count;
        }


        // OK gomb
        private void btnOk_Click(object sender, EventArgs e)
        {
            int tableId = 0;

            if (chckBox_OrderingToTable.Checked)
            {
                if (numUpDown_TableID.Value == 0 || numUpDown_TableID.Value > numberOfTable())
                {
                    numUpDown_TableID.Focus();
                    MessageBox.Show("The table doesn't exist",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                tableId = (int)numUpDown_TableID.Value;
            }

            // int paraméter átadása
            OrderingConfirmed?.Invoke(tableId);
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            CancelRequested?.Invoke();
        }
    }
}
