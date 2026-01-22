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
    public partial class TableViewForm : Form
    {
        public TableViewForm()
        {
            InitializeComponent();
        }

        protected bool visible;
        
        private void TableViewForm_Load(object sender, EventArgs e)
        {

            this.Opacity = 1;
            visible = true;

        }

        private void opacity_btn_Click(object sender, EventArgs e)
        {
            if (visible)
            {
                this.Opacity = 0.3;
                visible = false;
            }
            else
            {
                this.Opacity = 1;
                visible = true;
            }


        }
    }
}
