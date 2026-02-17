using System;
using System.Windows.Forms;

namespace ponth
{
    public partial class ucOrderDetails : UserControl
    {
        //int paraméteres event
        public event Action<int> OrderingConfirmed;

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

        // OK gomb
        private void btnOk_Click(object sender, EventArgs e)
        {
            int tableId = 0;

            if (chckBox_OrderingToTable.Checked)
            {
                if (numUpDown_TableID.Value == 0)
                {
                    numUpDown_TableID.Focus();
                    MessageBox.Show("Nem letezo asztal",
                        "Figyelmeztetés",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                tableId = (int)numUpDown_TableID.Value;
            }

            // int paraméter átadása
            OrderingConfirmed?.Invoke(tableId);
        }
    }
}
