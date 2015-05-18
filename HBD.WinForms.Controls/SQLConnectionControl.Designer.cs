namespace HBD.WinForms.Controls
{
    partial class SQLConnectionControl
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
                this.DisposeConnection();
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.bt_Test = new System.Windows.Forms.Button();
            this.bt_Connect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_ServerName = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ch_SQL = new System.Windows.Forms.RadioButton();
            this.ch_Window = new System.Windows.Forms.RadioButton();
            this.loginControl = new HBD.WinForms.Controls.LoginControl();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_DBName = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.Controls.Add(this.bt_Test, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.bt_Connect, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txt_ServerName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.loginControl, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.cb_DBName, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(382, 332);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // bt_Test
            // 
            this.bt_Test.AutoSize = true;
            this.bt_Test.Dock = System.Windows.Forms.DockStyle.Left;
            this.bt_Test.Enabled = false;
            this.bt_Test.Location = new System.Drawing.Point(3, 305);
            this.bt_Test.Name = "bt_Test";
            this.bt_Test.Size = new System.Drawing.Size(80, 24);
            this.bt_Test.TabIndex = 3;
            this.bt_Test.Text = "Test";
            this.bt_Test.UseVisualStyleBackColor = true;
            this.bt_Test.Click += new System.EventHandler(this.bt_Test_Click);
            // 
            // bt_Connect
            // 
            this.bt_Connect.Dock = System.Windows.Forms.DockStyle.Right;
            this.bt_Connect.Enabled = false;
            this.bt_Connect.Location = new System.Drawing.Point(264, 305);
            this.bt_Connect.Name = "bt_Connect";
            this.bt_Connect.Size = new System.Drawing.Size(100, 24);
            this.bt_Connect.TabIndex = 4;
            this.bt_Connect.Text = "Connect";
            this.bt_Connect.UseVisualStyleBackColor = true;
            this.bt_Connect.Click += new System.EventHandler(this.bt_Connect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "Authentication:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_ServerName
            // 
            this.txt_ServerName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_ServerName.Location = new System.Drawing.Point(89, 3);
            this.txt_ServerName.Name = "txt_ServerName";
            this.txt_ServerName.Size = new System.Drawing.Size(275, 20);
            this.txt_ServerName.TabIndex = 0;
            this.txt_ServerName.TextChanged += new System.EventHandler(this.txt_ServerName_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ch_SQL);
            this.panel1.Controls.Add(this.ch_Window);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(89, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(275, 20);
            this.panel1.TabIndex = 4;
            // 
            // ch_SQL
            // 
            this.ch_SQL.AutoSize = true;
            this.ch_SQL.Dock = System.Windows.Forms.DockStyle.Left;
            this.ch_SQL.Location = new System.Drawing.Point(64, 0);
            this.ch_SQL.Name = "ch_SQL";
            this.ch_SQL.Size = new System.Drawing.Size(80, 20);
            this.ch_SQL.TabIndex = 1;
            this.ch_SQL.Text = "SQL Server";
            this.ch_SQL.UseVisualStyleBackColor = true;
            this.ch_SQL.CheckedChanged += new System.EventHandler(this.ch_Window_CheckedChanged);
            // 
            // ch_Window
            // 
            this.ch_Window.AutoSize = true;
            this.ch_Window.Checked = true;
            this.ch_Window.Dock = System.Windows.Forms.DockStyle.Left;
            this.ch_Window.Location = new System.Drawing.Point(0, 0);
            this.ch_Window.Name = "ch_Window";
            this.ch_Window.Size = new System.Drawing.Size(64, 20);
            this.ch_Window.TabIndex = 0;
            this.ch_Window.TabStop = true;
            this.ch_Window.Text = "Window";
            this.ch_Window.UseVisualStyleBackColor = true;
            this.ch_Window.CheckedChanged += new System.EventHandler(this.ch_Window_CheckedChanged);
            // 
            // loginControl
            // 
            this.loginControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loginControl.Enabled = false;
            this.loginControl.Location = new System.Drawing.Point(89, 55);
            this.loginControl.LoginText = "Login";
            this.loginControl.Name = "loginControl";
            this.loginControl.ShowLoginButton = false;
            this.loginControl.ShowRememberPassword = false;
            this.loginControl.Size = new System.Drawing.Size(275, 54);
            this.loginControl.TabIndex = 1;
            this.loginControl.ValueChanges += new System.EventHandler(this.loginControl_ValueChanges);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 27);
            this.label3.TabIndex = 8;
            this.label3.Text = "Database:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cb_DBName
            // 
            this.cb_DBName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cb_DBName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cb_DBName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_DBName.Enabled = false;
            this.cb_DBName.FormattingEnabled = true;
            this.cb_DBName.Location = new System.Drawing.Point(89, 115);
            this.cb_DBName.Name = "cb_DBName";
            this.cb_DBName.Size = new System.Drawing.Size(275, 21);
            this.cb_DBName.TabIndex = 2;
            this.cb_DBName.DropDown += new System.EventHandler(this.cb_DBName_DropDown);
            this.cb_DBName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cb_DBName_KeyUp);
            // 
            // SQLConnectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SQLConnectionControl";
            this.Size = new System.Drawing.Size(382, 332);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_ServerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton ch_Window;
        private System.Windows.Forms.RadioButton ch_SQL;
        private LoginControl loginControl;
        private System.Windows.Forms.Button bt_Test;
        private System.Windows.Forms.Button bt_Connect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_DBName;
    }
}
