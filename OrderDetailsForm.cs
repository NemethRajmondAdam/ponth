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
    public partial class OrderDetailsForm : FormBase
    {
        public OrderDetailsForm()
        {
            LanguageManager.ApplyCulture();

            InitializeComponent();
        }

        private void OrderDetailForm_Load(object sender, EventArgs e)
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

        private void chckBox_Account_CheckedChanged(object sender, EventArgs e) //Tovabb fejlesztesi lehetoseg
        {
            txtBox_AccountHolder.Enabled = chckBox_Account.Checked;
        }

        protected override void task_btn_Ok() //Ellenorzi az asztalID (egyenlore csak a 0-at [nincs adatbazis adat]) ha nem megfelelo nem enged tovabb a rendeleshez
        {

            if (chckBox_OrderingToTable.Checked)
            {
                try
                {
                    if (numUpDown_TableID.Value == 0)
                    {
                        numUpDown_TableID.Focus();
                        throw new Exception("Nem letezo asztal");

                    }
                }
                catch (Exception err)
                {

                    MessageBox.Show(err.Message, "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //rendeles leadasa metodus
                OrderingForm frm = new OrderingForm();
                frm.ShowDialog();
            }
            
        }
    }
}
