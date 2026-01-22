namespace ponth
{
    partial class OrderingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderingForm));
            this.panelMain = new System.Windows.Forms.Panel();
            this.tablePanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.DrinksPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.CartPanel = new System.Windows.Forms.Panel();
            this.panelMain.SuspendLayout();
            this.tablePanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.tablePanelMain);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(4);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1200, 623);
            this.panelMain.TabIndex = 0;
            // 
            // tablePanelMain
            // 
            this.tablePanelMain.ColumnCount = 2;
            this.tablePanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.99607F));
            this.tablePanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.00393F));
            this.tablePanelMain.Controls.Add(this.DrinksPanel, 0, 0);
            this.tablePanelMain.Controls.Add(this.CartPanel, 1, 0);
            this.tablePanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanelMain.Location = new System.Drawing.Point(0, 0);
            this.tablePanelMain.Name = "tablePanelMain";
            this.tablePanelMain.RowCount = 1;
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanelMain.Size = new System.Drawing.Size(1200, 623);
            this.tablePanelMain.TabIndex = 3;
            // 
            // DrinksPanel
            // 
            this.DrinksPanel.AutoScroll = true;
            this.DrinksPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DrinksPanel.Location = new System.Drawing.Point(3, 3);
            this.DrinksPanel.Name = "DrinksPanel";
            this.DrinksPanel.Size = new System.Drawing.Size(713, 617);
            this.DrinksPanel.TabIndex = 0;
            // 
            // CartPanel
            // 
            this.CartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CartPanel.Location = new System.Drawing.Point(722, 3);
            this.CartPanel.Name = "CartPanel";
            this.CartPanel.Size = new System.Drawing.Size(475, 617);
            this.CartPanel.TabIndex = 1;
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 623);
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Itallap";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MenuForm_Load);
            this.panelMain.ResumeLayout(false);
            this.tablePanelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.TableLayoutPanel tablePanelMain;
        private System.Windows.Forms.FlowLayoutPanel DrinksPanel;
        private System.Windows.Forms.Panel CartPanel;
    }
}