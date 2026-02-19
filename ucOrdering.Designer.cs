namespace ponth
{
    partial class ucOrdering
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
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
            this.panelMain.Size = new System.Drawing.Size(150, 150);
            this.panelMain.TabIndex = 1;
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
            this.tablePanelMain.Size = new System.Drawing.Size(150, 150);
            this.tablePanelMain.TabIndex = 3;
            // 
            // DrinksPanel
            // 
            this.DrinksPanel.AutoScroll = true;
            this.DrinksPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DrinksPanel.Location = new System.Drawing.Point(3, 3);
            this.DrinksPanel.Name = "DrinksPanel";
            this.DrinksPanel.Size = new System.Drawing.Size(83, 144);
            this.DrinksPanel.TabIndex = 0;
            // 
            // CartPanel
            // 
            this.CartPanel.AutoScroll = true;
            this.CartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CartPanel.Location = new System.Drawing.Point(92, 3);
            this.CartPanel.Name = "CartPanel";
            this.CartPanel.Size = new System.Drawing.Size(55, 144);
            this.CartPanel.TabIndex = 1;
            // 
            // ucOrdering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Name = "ucOrdering";
            this.Load += new System.EventHandler(this.ucOrdering_Load);
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
