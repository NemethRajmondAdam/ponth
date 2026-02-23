namespace ponth
{
    partial class ucPaying
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
            this.btnDropdown = new ponth.CostumeControls.cButtons();
            this.btnCash = new ponth.CostumeControls.cButtons();
            this.btnCard = new ponth.CostumeControls.cButtons();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.BackgroundImage = global::ponth.Properties.Resources.ponth_background;
            this.panelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelMain.Controls.Add(this.btnDropdown);
            this.panelMain.Controls.Add(this.btnCash);
            this.panelMain.Controls.Add(this.btnCard);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1253, 700);
            this.panelMain.TabIndex = 0;
            // 
            // btnDropdown
            // 
            this.btnDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDropdown.BackColor = System.Drawing.Color.GhostWhite;
            this.btnDropdown.BackgroundColor = System.Drawing.Color.GhostWhite;
            this.btnDropdown.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnDropdown.BorderRadius = 50;
            this.btnDropdown.BorderSize = 0;
            this.btnDropdown.FlatAppearance.BorderSize = 0;
            this.btnDropdown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDropdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnDropdown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            this.btnDropdown.Location = new System.Drawing.Point(510, 290);
            this.btnDropdown.Name = "btnDropdown";
            this.btnDropdown.Size = new System.Drawing.Size(227, 115);
            this.btnDropdown.TabIndex = 3;
            this.btnDropdown.Text = "Asztalszám";
            this.btnDropdown.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            this.btnDropdown.UseVisualStyleBackColor = false;
            this.btnDropdown.Click += new System.EventHandler(this.btnDropdown_Click);
            // 
            // btnCash
            // 
            this.btnCash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCash.BackColor = System.Drawing.Color.Transparent;
            this.btnCash.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCash.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            this.btnCash.BorderRadius = 50;
            this.btnCash.BorderSize = 10;
            this.btnCash.FlatAppearance.BorderSize = 0;
            this.btnCash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnCash.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            this.btnCash.Location = new System.Drawing.Point(899, 171);
            this.btnCash.Name = "btnCash";
            this.btnCash.Size = new System.Drawing.Size(276, 340);
            this.btnCash.TabIndex = 1;
            this.btnCash.Text = "Készpénz";
            this.btnCash.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            this.btnCash.UseVisualStyleBackColor = false;
            // 
            // btnCard
            // 
            this.btnCard.BackColor = System.Drawing.Color.Transparent;
            this.btnCard.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            this.btnCard.BorderRadius = 50;
            this.btnCard.BorderSize = 10;
            this.btnCard.FlatAppearance.BorderSize = 0;
            this.btnCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            this.btnCard.Location = new System.Drawing.Point(81, 171);
            this.btnCard.Name = "btnCard";
            this.btnCard.Size = new System.Drawing.Size(276, 340);
            this.btnCard.TabIndex = 0;
            this.btnCard.Text = "Kártya";
            this.btnCard.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            this.btnCard.UseVisualStyleBackColor = false;
            // 
            // ucPaying
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Name = "ucPaying";
            this.Size = new System.Drawing.Size(1253, 700);
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private CostumeControls.cButtons btnCash;
        private CostumeControls.cButtons btnCard;
        private CostumeControls.cButtons btnDropdown;
    }
}
