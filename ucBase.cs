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
    public partial class ucBase : UserControl
    {
        public ucBase()
        {
            InitializeComponent();
        }

        protected virtual void task_btn_Ok()
        {
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            task_btn_Ok();
        }
    }
}
