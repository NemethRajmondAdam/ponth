namespace ponth
{
    partial class ucBase
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_Ok = new ponth.CostumeControls.cButtons();
            this.btn_Cancel = new ponth.CostumeControls.cButtons();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_Ok);
            this.panel1.Controls.Add(this.btn_Cancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 215);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(575, 72);
            this.panel1.TabIndex = 1;
            // 
            // btn_Ok
            // 
            this.btn_Ok.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_Ok.BackColor = System.Drawing.Color.GhostWhite;
            this.btn_Ok.BackgroundColor = System.Drawing.Color.GhostWhite;
            this.btn_Ok.BorderColor = System.Drawing.Color.LimeGreen;
            this.btn_Ok.BorderRadius = 20;
            this.btn_Ok.BorderSize = 1;
            this.btn_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_Ok.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btn_Ok.FlatAppearance.BorderSize = 0;
            this.btn_Ok.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Honeydew;
            this.btn_Ok.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGreen;
            this.btn_Ok.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Ok.ForeColor = System.Drawing.Color.LimeGreen;
            this.btn_Ok.Location = new System.Drawing.Point(334, 17);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(109, 40);
            this.btn_Ok.TabIndex = 10;
            this.btn_Ok.Text = "Save";
            this.btn_Ok.TextColor = System.Drawing.Color.LimeGreen;
            this.btn_Ok.UseVisualStyleBackColor = false;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_Cancel.BackColor = System.Drawing.Color.GhostWhite;
            this.btn_Cancel.BackgroundColor = System.Drawing.Color.GhostWhite;
            this.btn_Cancel.BorderColor = System.Drawing.Color.Crimson;
            this.btn_Cancel.BorderRadius = 20;
            this.btn_Cancel.BorderSize = 1;
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btn_Cancel.FlatAppearance.BorderSize = 0;
            this.btn_Cancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Honeydew;
            this.btn_Cancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightCoral;
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.ForeColor = System.Drawing.Color.Red;
            this.btn_Cancel.Location = new System.Drawing.Point(459, 17);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(109, 40);
            this.btn_Cancel.TabIndex = 9;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.TextColor = System.Drawing.Color.Red;
            this.btn_Cancel.UseVisualStyleBackColor = false;
            // 
            // ucBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "ucBase";
            this.Size = new System.Drawing.Size(575, 287);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private CostumeControls.cButtons btn_Ok;
        private CostumeControls.cButtons btn_Cancel;
    }
}
