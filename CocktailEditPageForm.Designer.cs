namespace ponth
{
    partial class CocktailEditPageForm
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
            this.flowLayoutPanelIngridients = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowLayoutPanelIngridients
            // 
            this.flowLayoutPanelIngridients.AutoScroll = true;
            this.flowLayoutPanelIngridients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelIngridients.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelIngridients.Name = "flowLayoutPanelIngridients";
            this.flowLayoutPanelIngridients.Size = new System.Drawing.Size(800, 450);
            this.flowLayoutPanelIngridients.TabIndex = 1;
            // 
            // CocktailEditPageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.flowLayoutPanelIngridients);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CocktailEditPageForm";
            this.Text = "CocktailEditPageForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.CocktailEditPageForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelIngridients;
    }
}