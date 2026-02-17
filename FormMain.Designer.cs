namespace ponth
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_ChangeLang = new ponth.CostumeControls.cButtons();
            this.panelLanguageSelect = new System.Windows.Forms.Panel();
            this.sidebar = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.sidebarButton = new System.Windows.Forms.PictureBox();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.btn_Menu = new System.Windows.Forms.Button();
            this.panelOrder = new System.Windows.Forms.Panel();
            this.btn_Order = new System.Windows.Forms.Button();
            this.panelPaying = new System.Windows.Forms.Panel();
            this.btn_Paying = new System.Windows.Forms.Button();
            this.panelHome = new System.Windows.Forms.Panel();
            this.btn_Home = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.sidebarTimer = new System.Windows.Forms.Timer(this.components);
            this.tableViewSwitch = new ponth.CostumeControls.cToogleSwitch();
            this.ucMainPanel = new System.Windows.Forms.SplitContainer();
            this.panel1.SuspendLayout();
            this.sidebar.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sidebarButton)).BeginInit();
            this.panelMenu.SuspendLayout();
            this.panelOrder.SuspendLayout();
            this.panelPaying.SuspendLayout();
            this.panelHome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucMainPanel)).BeginInit();
            this.ucMainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btn_ChangeLang);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btn_ChangeLang
            // 
            resources.ApplyResources(this.btn_ChangeLang, "btn_ChangeLang");
            this.btn_ChangeLang.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_ChangeLang.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btn_ChangeLang.BorderColor = System.Drawing.Color.Black;
            this.btn_ChangeLang.BorderRadius = 20;
            this.btn_ChangeLang.BorderSize = 1;
            this.btn_ChangeLang.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btn_ChangeLang.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_ChangeLang.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_ChangeLang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            this.btn_ChangeLang.Name = "btn_ChangeLang";
            this.btn_ChangeLang.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            this.btn_ChangeLang.UseVisualStyleBackColor = false;
            this.btn_ChangeLang.Click += new System.EventHandler(this.btn_ChangeLang_Click);
            // 
            // panelLanguageSelect
            // 
            resources.ApplyResources(this.panelLanguageSelect, "panelLanguageSelect");
            this.panelLanguageSelect.Name = "panelLanguageSelect";
            // 
            // sidebar
            // 
            this.sidebar.BackColor = System.Drawing.Color.LightGray;
            this.sidebar.Controls.Add(this.panel2);
            this.sidebar.Controls.Add(this.panelMenu);
            this.sidebar.Controls.Add(this.panelOrder);
            this.sidebar.Controls.Add(this.panelPaying);
            this.sidebar.Controls.Add(this.panelHome);
            resources.ApplyResources(this.sidebar, "sidebar");
            this.sidebar.Name = "sidebar";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.sidebarButton);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // sidebarButton
            // 
            this.sidebarButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sidebarButton.Image = global::ponth.Properties.Resources.menu__1_;
            resources.ApplyResources(this.sidebarButton, "sidebarButton");
            this.sidebarButton.Name = "sidebarButton";
            this.sidebarButton.TabStop = false;
            this.sidebarButton.Click += new System.EventHandler(this.sidebarButton_Click);
            // 
            // panelMenu
            // 
            resources.ApplyResources(this.panelMenu, "panelMenu");
            this.panelMenu.Controls.Add(this.btn_Menu);
            this.panelMenu.Name = "panelMenu";
            // 
            // btn_Menu
            // 
            this.btn_Menu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            resources.ApplyResources(this.btn_Menu, "btn_Menu");
            this.btn_Menu.ForeColor = System.Drawing.Color.Black;
            this.btn_Menu.Image = global::ponth.Properties.Resources.rsz_menu;
            this.btn_Menu.Name = "btn_Menu";
            this.btn_Menu.UseVisualStyleBackColor = false;
            this.btn_Menu.Click += new System.EventHandler(this.btn_Menu_Click);
            // 
            // panelOrder
            // 
            resources.ApplyResources(this.panelOrder, "panelOrder");
            this.panelOrder.Controls.Add(this.btn_Order);
            this.panelOrder.Name = "panelOrder";
            // 
            // btn_Order
            // 
            this.btn_Order.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            resources.ApplyResources(this.btn_Order, "btn_Order");
            this.btn_Order.Image = global::ponth.Properties.Resources.rsz_menu;
            this.btn_Order.Name = "btn_Order";
            this.btn_Order.UseVisualStyleBackColor = false;
            this.btn_Order.Click += new System.EventHandler(this.btn_Order_Click);
            // 
            // panelPaying
            // 
            resources.ApplyResources(this.panelPaying, "panelPaying");
            this.panelPaying.Controls.Add(this.btn_Paying);
            this.panelPaying.Name = "panelPaying";
            // 
            // btn_Paying
            // 
            this.btn_Paying.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            resources.ApplyResources(this.btn_Paying, "btn_Paying");
            this.btn_Paying.Image = global::ponth.Properties.Resources.rsz_menu;
            this.btn_Paying.Name = "btn_Paying";
            this.btn_Paying.UseVisualStyleBackColor = false;
            this.btn_Paying.Click += new System.EventHandler(this.btn_Paying_Click);
            // 
            // panelHome
            // 
            resources.ApplyResources(this.panelHome, "panelHome");
            this.panelHome.Controls.Add(this.btn_Home);
            this.panelHome.Name = "panelHome";
            // 
            // btn_Home
            // 
            this.btn_Home.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            resources.ApplyResources(this.btn_Home, "btn_Home");
            this.btn_Home.Image = global::ponth.Properties.Resources.rsz_menu;
            this.btn_Home.Name = "btn_Home";
            this.btn_Home.UseVisualStyleBackColor = false;
            this.btn_Home.Click += new System.EventHandler(this.btn_Home_Click);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // sidebarTimer
            // 
            this.sidebarTimer.Interval = 10;
            this.sidebarTimer.Tick += new System.EventHandler(this.sidebarTimer_Tick);
            // 
            // tableViewSwitch
            // 
            resources.ApplyResources(this.tableViewSwitch, "tableViewSwitch");
            this.tableViewSwitch.Name = "tableViewSwitch";
            this.tableViewSwitch.OffBackColor = System.Drawing.Color.Gray;
            this.tableViewSwitch.OffToggleColor = System.Drawing.Color.Gainsboro;
            this.tableViewSwitch.OnBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            this.tableViewSwitch.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.tableViewSwitch.UseVisualStyleBackColor = true;
            this.tableViewSwitch.CheckedChanged += new System.EventHandler(this.tableViewSwitch_CheckedChanged);
            // 
            // ucMainPanel
            // 
            resources.ApplyResources(this.ucMainPanel, "ucMainPanel");
            this.ucMainPanel.Name = "ucMainPanel";
            // 
            // ucMainPanel.Panel1
            // 
            this.ucMainPanel.Panel1.BackColor = System.Drawing.Color.Transparent;
            // 
            // ucMainPanel.Panel2
            // 
            this.ucMainPanel.Panel2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucMainPanel);
            this.Controls.Add(this.tableViewSwitch);
            this.Controls.Add(this.sidebar);
            this.Controls.Add(this.panelLanguageSelect);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "FormMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.panel1.ResumeLayout(false);
            this.sidebar.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sidebarButton)).EndInit();
            this.panelMenu.ResumeLayout(false);
            this.panelOrder.ResumeLayout(false);
            this.panelPaying.ResumeLayout(false);
            this.panelHome.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucMainPanel)).EndInit();
            this.ucMainPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelLanguageSelect;
        private CostumeControls.cButtons btn_ChangeLang;
        private System.Windows.Forms.FlowLayoutPanel sidebar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Button btn_Menu;
        private System.Windows.Forms.Panel panelOrder;
        private System.Windows.Forms.Button btn_Order;
        private System.Windows.Forms.Panel panelPaying;
        private System.Windows.Forms.Button btn_Paying;
        private System.Windows.Forms.Panel panelHome;
        private System.Windows.Forms.Button btn_Home;
        private System.Windows.Forms.PictureBox sidebarButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer sidebarTimer;
        private CostumeControls.cToogleSwitch tableViewSwitch;
        private System.Windows.Forms.SplitContainer ucMainPanel;
    }
}

