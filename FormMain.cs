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

            panelLanguageSelect.BringToFront();
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
            //bele kell rakni hogy a switch olyan allapotba alljon ahogyan kell neki
            panelResize();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            btn_ChangeLang.Text = $"-{LanguageManager.CurrentLanguageName}-";
            //sidebar.Height = this.Height;
            this.MaximizeBox = true;
            panelLanguageSelect.Visible = false;
            sidebar.BringToFront();
            tableViewSwitch.BringToFront();
            ucMainPanel.Size = pictureBox1.Size;
            ucMainPanel.Location = pictureBox1.Location;
            panelResize();
            

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
            /*OrderingForm frm = new OrderingForm();
            frm.ShowDialog();*/
            systemView = true;
            panelResize();
        }

        private void btn_Order_Click(object sender, EventArgs e)
        {
            OrderingForm frm = new OrderingForm();
            frm.ShowDialog();
            systemView = true;
        }

        private void btn_Paying_Click(object sender, EventArgs e)
        {
            PayingMethodForm frm = new PayingMethodForm(); //elotte meg hogy jelenitsuk az opciot hogy melyik asztalnak szeretne fizetni (illetve tovabb fejlesztesnel hogy melyik szamlat fizetne)
            frm.ShowDialog();
            systemView = true;
        }

        bool tableView = false;
        bool systemView = false;

        private void tableViewSwitch_CheckedChanged(object sender, EventArgs e)
        {
            tableView = !tableView;
            panelResize();
            
        }

        private void panelResize()
        {
            // ha semmi nem kell
            if (!systemView && !tableView)
            {
                ucMainPanel.Visible = false;

                return;
            }

            sidebar.BringToFront();
            tableViewSwitch.BringToFront();

            ucMainPanel.Size = pictureBox1.Size;
            ucMainPanel.Location = pictureBox1.Location;

            ucMainPanel.Visible = true;

            int halfWidth = ucMainPanel.Width / 2;

            if (systemView && tableView)
            {
                // 50–50, mindkettő látszik
                ucMainPanel.Panel1Collapsed = false;
                ucMainPanel.Panel2Collapsed = false;
                ucMainPanel.SplitterDistance = halfWidth;
            }
            else if (systemView && !tableView)
            {
                // csak bal oldal
                ucMainPanel.Panel1Collapsed = false;
                ucMainPanel.Panel2Collapsed = true;
            }
            else if (!systemView && tableView)
            {
                // CSAK jobb oldal, max 50%
                ucMainPanel.Panel1Collapsed = true;
                ucMainPanel.Panel2Collapsed = false;

                ucMainPanel.Size = new Size(pictureBox1.Height,ucMainPanel.Width / 2);
                ucMainPanel.Location = new Point(pictureBox1.Location.X + halfWidth + 50, pictureBox1.Location.Y-2);

            }
        }

        private void btn_Home_Click(object sender, EventArgs e)
        {
            systemView = false;
            panelResize();
        }
    }
}
