using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ponth
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            
            InitializeComponent();
        }
        
        private Desing d = new Desing();

        private void orderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderDetailsForm frm = new OrderDetailsForm();
            frm.ShowDialog();
        }

        private void PayingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PayingMethodForm frm = new PayingMethodForm(); //elotte meg hogy jelenitsuk az opciot hogy melyik asztalnak szeretne fizetni (illetve tovabb fejlesztesnel hogy melyik szamlat fizetne)
            frm.ShowDialog();
        }

        private void btn_ChangeLang_Click(object sender, EventArgs e)
        {

            LanguageSelectForm frm = new LanguageSelectForm();
            frm.ShowDialog();

            // Az aktuális form újratöltése
            this.Controls.Clear();
            this.InitializeComponent();

            //MessageBox.Show(LanguageManager.CurrentLanguage);

            // Visszajelzés a gombon
            UpdateLanguageButton();
        }

        private void UpdateLanguageButton()
        {
            btn_ChangeLang.Text = $"-{LanguageManager.CurrentLanguageName}-";
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            d.MakeRoundedBtn(btn_ChangeLang, 20);

            btn_ChangeLang.Text = $"-{LanguageManager.CurrentLanguageName}-";
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderingForm frm = new OrderingForm();
            frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            btn_ChangeLang.Text = $"Nyelv: {LanguageManager.CurrentLanguageName}";
            LanguageManager.ApplyCulture();

        }
    }
}
