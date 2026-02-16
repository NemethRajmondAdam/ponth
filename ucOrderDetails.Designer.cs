namespace ponth
{
    partial class ucOrderDetails
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
            this.txtBox_AccountHolder = new System.Windows.Forms.TextBox();
            this.chckBox_Account = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numUpDown_TableID = new System.Windows.Forms.NumericUpDown();
            this.chckBox_OrderingToTable = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_Ok = new ponth.CostumeControls.cButtons();
            this.btn_Cancel = new ponth.CostumeControls.cButtons();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_TableID)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panel1);
            this.panelMain.Controls.Add(this.txtBox_AccountHolder);
            this.panelMain.Controls.Add(this.chckBox_Account);
            this.panelMain.Controls.Add(this.label2);
            this.panelMain.Controls.Add(this.numUpDown_TableID);
            this.panelMain.Controls.Add(this.chckBox_OrderingToTable);
            this.panelMain.Controls.Add(this.label1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(680, 264);
            this.panelMain.TabIndex = 2;
            // 
            // txtBox_AccountHolder
            // 
            this.txtBox_AccountHolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBox_AccountHolder.Location = new System.Drawing.Point(468, 73);
            this.txtBox_AccountHolder.Name = "txtBox_AccountHolder";
            this.txtBox_AccountHolder.Size = new System.Drawing.Size(174, 20);
            this.txtBox_AccountHolder.TabIndex = 5;
            this.txtBox_AccountHolder.Text = "Nev";
            this.txtBox_AccountHolder.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chckBox_Account
            // 
            this.chckBox_Account.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chckBox_Account.AutoSize = true;
            this.chckBox_Account.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chckBox_Account.Location = new System.Drawing.Point(361, 48);
            this.chckBox_Account.Name = "chckBox_Account";
            this.chckBox_Account.Size = new System.Drawing.Size(88, 17);
            this.chckBox_Account.TabIndex = 4;
            this.chckBox_Account.Text = "Szamlat nyit?";
            this.chckBox_Account.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(35, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Asztal sorszama";
            // 
            // numUpDown_TableID
            // 
            this.numUpDown_TableID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numUpDown_TableID.Location = new System.Drawing.Point(149, 50);
            this.numUpDown_TableID.Name = "numUpDown_TableID";
            this.numUpDown_TableID.Size = new System.Drawing.Size(194, 20);
            this.numUpDown_TableID.TabIndex = 2;
            // 
            // chckBox_OrderingToTable
            // 
            this.chckBox_OrderingToTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chckBox_OrderingToTable.AutoSize = true;
            this.chckBox_OrderingToTable.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chckBox_OrderingToTable.Location = new System.Drawing.Point(12, 12);
            this.chckBox_OrderingToTable.Name = "chckBox_OrderingToTable";
            this.chckBox_OrderingToTable.Size = new System.Drawing.Size(111, 17);
            this.chckBox_OrderingToTable.TabIndex = 1;
            this.chckBox_OrderingToTable.Text = "Asztalhoz rendeli?";
            this.chckBox_OrderingToTable.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(58, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_Ok);
            this.panel1.Controls.Add(this.btn_Cancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 198);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(680, 66);
            this.panel1.TabIndex = 6;
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
            this.btn_Ok.Location = new System.Drawing.Point(439, 14);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(109, 40);
            this.btn_Ok.TabIndex = 10;
            this.btn_Ok.Text = "Mentés";
            this.btn_Ok.TextColor = System.Drawing.Color.LimeGreen;
            this.btn_Ok.UseVisualStyleBackColor = false;
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
            this.btn_Cancel.Location = new System.Drawing.Point(564, 14);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(109, 40);
            this.btn_Cancel.TabIndex = 9;
            this.btn_Cancel.Text = "Mégse";
            this.btn_Cancel.TextColor = System.Drawing.Color.Red;
            this.btn_Cancel.UseVisualStyleBackColor = false;
            // 
            // ucOrderDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Name = "ucOrderDetails";
            this.Size = new System.Drawing.Size(680, 264);
            this.Load += new System.EventHandler(this.ucOrderDetails_Load);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_TableID)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.TextBox txtBox_AccountHolder;
        private System.Windows.Forms.CheckBox chckBox_Account;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numUpDown_TableID;
        private System.Windows.Forms.CheckBox chckBox_OrderingToTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private CostumeControls.cButtons btn_Ok;
        private CostumeControls.cButtons btn_Cancel;
    }
}
