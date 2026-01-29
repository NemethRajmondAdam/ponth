namespace ponth
{
    partial class ucLanguageSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucLanguageSelect));
            this.panelMain = new System.Windows.Forms.Panel();
            this.btnES = new System.Windows.Forms.Button();
            this.btnFR = new System.Windows.Forms.Button();
            this.btnDE = new System.Windows.Forms.Button();
            this.btnEN = new System.Windows.Forms.Button();
            this.btnHUN = new System.Windows.Forms.Button();
            this.btnSave = new ponth.CostumeControls.cButtons();
            this.btnCancel = new ponth.CostumeControls.cButtons();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelMain.BackgroundImage")));
            this.panelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelMain.Controls.Add(this.btnSave);
            this.panelMain.Controls.Add(this.btnCancel);
            this.panelMain.Controls.Add(this.btnES);
            this.panelMain.Controls.Add(this.btnFR);
            this.panelMain.Controls.Add(this.btnDE);
            this.panelMain.Controls.Add(this.btnEN);
            this.panelMain.Controls.Add(this.btnHUN);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(786, 469);
            this.panelMain.TabIndex = 1;
            // 
            // btnES
            // 
            this.btnES.Image = global::ponth.Properties.Resources.ES_Selected;
            this.btnES.Location = new System.Drawing.Point(201, 294);
            this.btnES.Name = "btnES";
            this.btnES.Size = new System.Drawing.Size(188, 56);
            this.btnES.TabIndex = 4;
            this.btnES.TabStop = false;
            this.btnES.Tag = "4";
            this.btnES.Text = "ES";
            this.btnES.UseVisualStyleBackColor = true;
            // 
            // btnFR
            // 
            this.btnFR.Image = global::ponth.Properties.Resources.FR_Selected;
            this.btnFR.Location = new System.Drawing.Point(403, 194);
            this.btnFR.Name = "btnFR";
            this.btnFR.Size = new System.Drawing.Size(188, 56);
            this.btnFR.TabIndex = 3;
            this.btnFR.TabStop = false;
            this.btnFR.Tag = "3";
            this.btnFR.Text = "FR";
            this.btnFR.UseVisualStyleBackColor = true;
            // 
            // btnDE
            // 
            this.btnDE.Image = global::ponth.Properties.Resources.DE_Selected;
            this.btnDE.Location = new System.Drawing.Point(201, 194);
            this.btnDE.Name = "btnDE";
            this.btnDE.Size = new System.Drawing.Size(188, 56);
            this.btnDE.TabIndex = 2;
            this.btnDE.TabStop = false;
            this.btnDE.Tag = "2";
            this.btnDE.Text = "DE";
            this.btnDE.UseVisualStyleBackColor = true;
            // 
            // btnEN
            // 
            this.btnEN.Image = global::ponth.Properties.Resources.EN_Selected;
            this.btnEN.Location = new System.Drawing.Point(403, 93);
            this.btnEN.Name = "btnEN";
            this.btnEN.Size = new System.Drawing.Size(188, 56);
            this.btnEN.TabIndex = 1;
            this.btnEN.TabStop = false;
            this.btnEN.Tag = "1";
            this.btnEN.Text = "EN";
            this.btnEN.UseVisualStyleBackColor = true;
            // 
            // btnHUN
            // 
            this.btnHUN.BackColor = System.Drawing.SystemColors.Control;
            this.btnHUN.Image = global::ponth.Properties.Resources.HU_Unselected;
            this.btnHUN.Location = new System.Drawing.Point(201, 93);
            this.btnHUN.Name = "btnHUN";
            this.btnHUN.Size = new System.Drawing.Size(188, 56);
            this.btnHUN.TabIndex = 0;
            this.btnHUN.TabStop = false;
            this.btnHUN.Tag = "0";
            this.btnHUN.Text = "HUN";
            this.btnHUN.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.BackColor = System.Drawing.Color.GhostWhite;
            this.btnSave.BackgroundColor = System.Drawing.Color.GhostWhite;
            this.btnSave.BorderColor = System.Drawing.Color.LimeGreen;
            this.btnSave.BorderRadius = 20;
            this.btnSave.BorderSize = 1;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Honeydew;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGreen;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.LimeGreen;
            this.btnSave.Location = new System.Drawing.Point(540, 426);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(109, 40);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Mentés";
            this.btnSave.TextColor = System.Drawing.Color.LimeGreen;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.BackColor = System.Drawing.Color.GhostWhite;
            this.btnCancel.BackgroundColor = System.Drawing.Color.GhostWhite;
            this.btnCancel.BorderColor = System.Drawing.Color.Crimson;
            this.btnCancel.BorderRadius = 20;
            this.btnCancel.BorderSize = 1;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Honeydew;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightCoral;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.Red;
            this.btnCancel.Location = new System.Drawing.Point(665, 426);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(109, 40);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Mégse";
            this.btnCancel.TextColor = System.Drawing.Color.Red;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ucLanguageSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Name = "ucLanguageSelect";
            this.Size = new System.Drawing.Size(786, 469);
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEN;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button btnES;
        private System.Windows.Forms.Button btnFR;
        private System.Windows.Forms.Button btnDE;
        private System.Windows.Forms.Button btnHUN;
        private CostumeControls.cButtons btnCancel;
        private CostumeControls.cButtons btnSave;
    }
}
