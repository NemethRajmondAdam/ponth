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

        //NYELVVÁLTÁS
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

                LanguageManager.ToggleLanguage(langIndex);
                LanguageManager.ApplyCulture();
                ApplyResourcesToControl(this);
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

        private void ApplyResourcesToControl(Control control)
        {
            var resources = new ComponentResourceManager(typeof(FormMain));
            resources.ApplyResources(control, control.Name);

            foreach (Control child in control.Controls)
            {
                ApplyResourcesToControl(child);
            }
        }


        private void UpdateLanguageButton()
        {
            btn_ChangeLang.Text = $"-{LanguageManager.CurrentLanguageName}-";
            //d.MakeRoundedBtn(btn_ChangeLang, 20);
            panelLanguageSelect.Visible = false;
            this.MaximizeBox = true;
            this.WindowState = FormWindowState.Maximized;
            //bele kell rakni hogy a switch olyan allapotba alljon ahogyan kell neki
            panelResize();

        }

        //MAINFORM
        private void FormMain_Load(object sender, EventArgs e)
        {
            btn_ChangeLang.Text = $"-{LanguageManager.CurrentLanguageName}-";

            this.MaximizeBox = true;
            panelLanguageSelect.Visible = false;

            panel1.BringToFront();
            sidebar.BringToFront();
            tableViewSwitch.BringToFront();

            ucMainPanel.Size = pictureBox1.Size;
            ucMainPanel.Location = pictureBox1.Location;

            panelResize();

            btn_ChangeLang.Enabled = false;

            // aktuális legnagyobb ID lekérése
            object result = DatabaseHelper.ExecuteScalar("SELECT IFNULL(MAX(id),0) FROM orders");
            lastOrderId = Convert.ToInt32(result);

            orderTimer.Interval = 5000;
            orderTimer.Tick += OrderTimer_Tick;
            orderTimer.Start();
        }

        //SIDEBAR

        private bool sidebarExpanded = true;
        private bool sidebarAnimating = false;

        private async void ToggleSidebar()
        {
            if (sidebarAnimating) return;

            sidebarAnimating = true;

            int startWidth = sidebar.Width;
            int targetWidth = sidebarExpanded ? sidebar.MinimumSize.Width : sidebar.MaximumSize.Width;

            int animationDuration = 200; // ms
            int frameTime = 10;
            int steps = animationDuration / frameTime;

            for (int i = 0; i <= steps; i++)
            {
                double progress = (double)i / steps;

                // Smooth easing (easeInOut)
                double eased = progress < 0.5
                    ? 2 * progress * progress
                    : 1 - Math.Pow(-2 * progress + 2, 2) / 2;

                int newWidth = (int)(startWidth + (targetWidth - startWidth) * eased);

                this.SuspendLayout();
                sidebar.Width = newWidth;
                this.ResumeLayout();

                await Task.Delay(frameTime);
            }

            sidebar.Width = targetWidth;

            sidebarExpanded = !sidebarExpanded;
            sidebarAnimating = false;
        }


        private void sidebarButton_Click(object sender, EventArgs e)
        {
            ToggleSidebar();

        }


        //VIEWS

        bool tableView = false;
        bool systemView = false;

        private void tableViewSwitch_CheckedChanged(object sender, EventArgs e)
        {
            tableView = tableViewSwitch.Checked;

            if (tableView)
            {
                OpenTableView();
            }
            else
            {
                ucMainPanel.Panel2.Controls.Clear();
            }

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

            //POP UP (Ha szukseges)
            if (popUpNeeded)
            {
                popUp.Location = new Point((ucMainPanel.Panel1.Width - popUp.Width) / 2, (ucMainPanel.Panel1.Height - popUp.Height) / 2);
            }


        }

        private void homePage()
        {
            systemView = false;
            ucMainPanel.Panel1.Controls.Clear();
            panelResize();
        }

        private void btn_Home_Click(object sender, EventArgs e)
        {
            homePage();
        }

        //RENDELES

        Panel popUp;
        bool popUpNeeded = false;

        private void btn_Order_Click(object sender, EventArgs e)
        {

            ucMainPanel.Panel1.Controls.Clear();

            popUp = new Panel();
            popUp.Name = "panelPopUp";
            popUp.Size = new Size(644, 288);
            popUp.BackColor = Color.White;
            popUp.BorderStyle = BorderStyle.FixedSingle;

            ucMainPanel.Panel1.Controls.Add(popUp);

            popUp.Location = new Point(
                (ucMainPanel.Panel1.ClientSize.Width - popUp.Width) / 2,
                (ucMainPanel.Panel1.ClientSize.Height - popUp.Height) / 2
            );

            popUpNeeded = true;
 
            ucOrderDetails uc = new ucOrderDetails();
            uc.Dock = DockStyle.Fill;
            uc.OrderingConfirmed += Uc_OrderingConfirmed;

            uc.CancelRequested += () =>
            {
                if (popUp != null && popUp.Parent != null)
                {
                    ucMainPanel.Panel1.Controls.Remove(popUp);
                    popUp.Dispose();
                    popUp = null;
                    popUpNeeded=false;
                    homePage();
                }
            };

            popUp.Controls.Add(uc);

            systemView = true;
            panelResize();
        }

        private void Uc_OrderingConfirmed(int tableId)
        {
            popUpNeeded = false;
            ucMainPanel.Panel1.Controls.Clear();

            // UC létrehozása
            ucOrdering uc = new ucOrdering(tableId);
            uc.Dock = DockStyle.Fill;

            // Esemény kezelése
            uc.OrderingConfirmed += (id) =>
            {
                MessageBox.Show($"Rendelés leadva az {id} asztalra!");
                ucMainPanel.Panel1.Controls.Clear();
                homePage();
            };

            uc.CanceledOrder += () =>
            {

                homePage();

            };

            ucMainPanel.Panel1.Controls.Add(uc);
        }

        //fizetes
        //1253; 700

        private void btn_Paying_Click(object sender, EventArgs e)
        {
            ucPaying uc = new ucPaying();
            uc.BackColor = Color.White;
            uc.BorderStyle = BorderStyle.FixedSingle;
            uc.Location = new Point(100, 100);
            uc.Size = new Size(1253, 700);

            ucMainPanel.Panel1.Controls.Add(uc);


            uc.CancelRequested += () =>
            {
                ucMainPanel.Panel1.Controls.Remove(uc);
                uc.Dispose();
                homePage();
            };

            uc.PaymentDone += () =>
            {
                ucMainPanel.Panel1.Controls.Remove(uc);
                uc.Dispose();
                homePage();
            };

            systemView = true;
            panelResize();
        }

        //TableManagement

        private ucTables ucTables = new ucTables();

        System.Windows.Forms.Timer orderTimer = new System.Windows.Forms.Timer();
        int lastOrderId = 0;

        private void OrderTimer_Tick(object sender, EventArgs e)
        {
            string query = "SELECT IFNULL(MAX(id),0) FROM orders";
            object result = DatabaseHelper.ExecuteScalar(query);

            if (result == null) return;

            int latestId = Convert.ToInt32(result);

            if (latestId > lastOrderId)
            {
                lastOrderId = latestId;

                if (tableView)
                {
                    ucTables.UpdateTableStatus(); // frissítjük a gombokat
                }
                else
                {
                    ShowNewOrderNotification();
                }
            }
        }

        private void ShowNewOrderNotification()
        {
            lb_orderAlert.BringToFront();
            lb_orderAlert.Text = "";
            lb_orderAlert.BackColor = Color.Red;
            lb_orderAlert.Visible = true;
        }

        private void OpenTableView()
        {
            tableView = true;
            lb_orderAlert.Visible = false;

            ucMainPanel.Panel2.Controls.Clear();
            ucTables.Dock = DockStyle.Fill;
            ucMainPanel.Panel2.Controls.Add(ucTables);

            ucTables.LoadTables();
        }

        //Foglalasok

        private void btn_Reservation_Click(object sender, EventArgs e)
        {
            ucReservations uc = new ucReservations();
            uc.Dock = DockStyle.Fill;

            ucMainPanel.Panel1.Controls.Clear();
            ucMainPanel.Panel1.Controls.Add(uc);

            systemView = true;
            panelResize();



        }

    } 

}
