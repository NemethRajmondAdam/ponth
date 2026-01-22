namespace ponth
{
    partial class OrderDetailsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderDetailsForm));
            this.panelMain = new System.Windows.Forms.Panel();
            this.txtBox_AccountHolder = new System.Windows.Forms.TextBox();
            this.chckBox_Account = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numUpDown_TableID = new System.Windows.Forms.NumericUpDown();
            this.chckBox_OrderingToTable = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_TableID)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            resources.ApplyResources(this.panelMain, "panelMain");
            this.panelMain.Controls.Add(this.txtBox_AccountHolder);
            this.panelMain.Controls.Add(this.chckBox_Account);
            this.panelMain.Controls.Add(this.label2);
            this.panelMain.Controls.Add(this.numUpDown_TableID);
            this.panelMain.Controls.Add(this.chckBox_OrderingToTable);
            this.panelMain.Controls.Add(this.label1);
            this.panelMain.Name = "panelMain";
            // 
            // txtBox_AccountHolder
            // 
            resources.ApplyResources(this.txtBox_AccountHolder, "txtBox_AccountHolder");
            this.txtBox_AccountHolder.Name = "txtBox_AccountHolder";
            // 
            // chckBox_Account
            // 
            resources.ApplyResources(this.chckBox_Account, "chckBox_Account");
            this.chckBox_Account.Name = "chckBox_Account";
            this.chckBox_Account.UseVisualStyleBackColor = true;
            this.chckBox_Account.CheckedChanged += new System.EventHandler(this.chckBox_Account_CheckedChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // numUpDown_TableID
            // 
            resources.ApplyResources(this.numUpDown_TableID, "numUpDown_TableID");
            this.numUpDown_TableID.Name = "numUpDown_TableID";
            // 
            // chckBox_OrderingToTable
            // 
            resources.ApplyResources(this.chckBox_OrderingToTable, "chckBox_OrderingToTable");
            this.chckBox_OrderingToTable.Name = "chckBox_OrderingToTable";
            this.chckBox_OrderingToTable.UseVisualStyleBackColor = true;
            this.chckBox_OrderingToTable.CheckedChanged += new System.EventHandler(this.chckBox_OrderingToTable_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // OrderDetailsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Name = "OrderDetailsForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.Load += new System.EventHandler(this.OrderDetailForm_Load);
            this.Controls.SetChildIndex(this.panelMain, 0);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_TableID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numUpDown_TableID;
        private System.Windows.Forms.CheckBox chckBox_OrderingToTable;
        private System.Windows.Forms.TextBox txtBox_AccountHolder;
        private System.Windows.Forms.CheckBox chckBox_Account;
    }
}