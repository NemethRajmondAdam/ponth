using ponth.CostumeControls;
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
            this.MaximizeBox = true;
            var uc = new ucLanguageSelect();

            uc.OnLanguageSelected += (langIndex) =>
            {
                LanguageManager.ToggleLanguage(langIndex);
                LanguageManager.ApplyCulture();


            };

            uc.OnLanguageChanged += (langIndex) =>
            {
                LanguageManager.ToggleLanguage(langIndex);
                LanguageManager.ApplyCulture();

                // Az aktuális form újratöltése
                this.Controls.Clear();
                this.InitializeComponent();
                UpdateLanguageButton();
            };

            uc.OnCancel += () =>
            {
                panelLanguageSelect.Controls.Clear();
                panelLanguageSelect.Visible = false;
            };

            panelLanguageSelect.Controls.Clear();
            panelLanguageSelect.Controls.Add(uc);
            panelLanguageSelect.Size = new Size(816, 489);
            uc.Dock = DockStyle.Fill;
            panelLanguageSelect.Visible = true;
        }

        private void UpdateLanguageButton()
        {
            btn_ChangeLang.Text = $"-{LanguageManager.CurrentLanguageName}-";
            //d.MakeRoundedBtn(btn_ChangeLang, 20);
            panelLanguageSelect.Visible = false;
            this.MaximizeBox = true;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            btn_ChangeLang.Text = $"-{LanguageManager.CurrentLanguageName}-";
            //sidebar.Height = this.Height;
            this.MaximizeBox = true;
            panelLanguageSelect.Visible = false;
            

        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderingForm frm = new OrderingForm();
            frm.ShowDialog();
        }

        bool sidebarExpand;

        private void sidebarTimer_Tick(object sender, EventArgs e)
        {


            if (sidebarExpand)
            {
                sidebar.Width -= 10;
                if (sidebar.Width == sidebar.MinimumSize.Width)
                {
                    sidebarExpand = false;
                    sidebarTimer.Stop();
                }
            }
            else 
            {
                sidebar.Width += 10;
                if (sidebar.Width == sidebar.MaximumSize.Width)
                {
                    sidebarExpand = true;
                    sidebarTimer.Stop();
                }
            }
        }

        private void sidebarButton_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();

        }

        private void btn_Menu_Click(object sender, EventArgs e)
        {
            OrderingForm frm = new OrderingForm();
            frm.ShowDialog();
        }

        private void btn_Order_Click(object sender, EventArgs e)
        {
            OrderingForm frm = new OrderingForm();
            frm.ShowDialog();
        }

        private void btn_Paying_Click(object sender, EventArgs e)
        {
            PayingMethodForm frm = new PayingMethodForm(); //elotte meg hogy jelenitsuk az opciot hogy melyik asztalnak szeretne fizetni (illetve tovabb fejlesztesnel hogy melyik szamlat fizetne)
            frm.ShowDialog();
        }
    }
}
