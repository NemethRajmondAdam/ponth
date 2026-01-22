namespace ponth
{
    partial class LanguageSelectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LanguageSelectForm));
            this.panelMain = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnES = new System.Windows.Forms.Button();
            this.btnFR = new System.Windows.Forms.Button();
            this.btnDE = new System.Windows.Forms.Button();
            this.btnEN = new System.Windows.Forms.Button();
            this.btnHUN = new System.Windows.Forms.Button();
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
            this.panelMain.Size = new System.Drawing.Size(800, 450);
            this.panelMain.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(554, 387);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(109, 51);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Mentés";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(679, 387);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(109, 51);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Mégse";
            this.btnCancel.UseVisualStyleBackColor = true;
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
            this.btnES.Click += new System.EventHandler(this.LanguageButton_Click);
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
            this.btnFR.Click += new System.EventHandler(this.LanguageButton_Click);
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
            this.btnDE.Click += new System.EventHandler(this.LanguageButton_Click);
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
            this.btnEN.Click += new System.EventHandler(this.LanguageButton_Click);
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
            this.btnHUN.Click += new System.EventHandler(this.LanguageButton_Click);
            // 
            // LanguageSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.panelMain);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LanguageSelectForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nyelvi beállítás";
            this.Load += new System.EventHandler(this.LanguageSelectForm_Load);
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnES;
        private System.Windows.Forms.Button btnFR;
        private System.Windows.Forms.Button btnDE;
        private System.Windows.Forms.Button btnEN;
        private System.Windows.Forms.Button btnHUN;
        private System.Windows.Forms.Button btnSave;
    }
}