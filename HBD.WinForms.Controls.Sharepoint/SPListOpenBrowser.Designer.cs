namespace HBD.WinForms.Controls.Sharepoint
{
    partial class SPListOpenBrowser
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
                this.DisposeAdapter();
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.txt_URL = new System.Windows.Forms.TextBox();
            this.lb_Sheets = new System.Windows.Forms.Label();
            this.cb_ListNames = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbView = new System.Windows.Forms.Label();
            this.cb_Views = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 6;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.txt_URL, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.lb_Sheets, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.cb_ListNames, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.lbView, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.cb_Views, 5, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(672, 25);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // txt_URL
            // 
            this.txt_URL.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txt_URL.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.txt_URL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_URL.Location = new System.Drawing.Point(44, 3);
            this.txt_URL.Multiline = true;
            this.txt_URL.Name = "txt_URL";
            this.txt_URL.Size = new System.Drawing.Size(289, 19);
            this.txt_URL.TabIndex = 0;
            this.txt_URL.TextChanged += new System.EventHandler(this.txt_URL_TextChanged);
            this.txt_URL.Leave += new System.EventHandler(this.txt_URL_Leave);
            // 
            // lb_Sheets
            // 
            this.lb_Sheets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Sheets.Location = new System.Drawing.Point(339, 0);
            this.lb_Sheets.Name = "lb_Sheets";
            this.lb_Sheets.Size = new System.Drawing.Size(35, 25);
            this.lb_Sheets.TabIndex = 2;
            this.lb_Sheets.Text = "List:";
            this.lb_Sheets.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cb_ListNames
            // 
            this.cb_ListNames.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cb_ListNames.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cb_ListNames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_ListNames.FormattingEnabled = true;
            this.cb_ListNames.Location = new System.Drawing.Point(380, 2);
            this.cb_ListNames.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this.cb_ListNames.Name = "cb_ListNames";
            this.cb_ListNames.Size = new System.Drawing.Size(119, 21);
            this.cb_ListNames.TabIndex = 1;
            this.cb_ListNames.SelectedIndexChanged += new System.EventHandler(this.cb_ListNames_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "URL:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbView
            // 
            this.lbView.AutoSize = true;
            this.lbView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbView.Location = new System.Drawing.Point(505, 0);
            this.lbView.Name = "lbView";
            this.lbView.Size = new System.Drawing.Size(38, 25);
            this.lbView.TabIndex = 9;
            this.lbView.Text = "Views:";
            this.lbView.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbView.Visible = false;
            // 
            // cb_Views
            // 
            this.cb_Views.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cb_Views.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cb_Views.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_Views.FormattingEnabled = true;
            this.cb_Views.Location = new System.Drawing.Point(549, 2);
            this.cb_Views.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this.cb_Views.Name = "cb_Views";
            this.cb_Views.Size = new System.Drawing.Size(120, 21);
            this.cb_Views.TabIndex = 2;
            this.cb_Views.Visible = false;
            this.cb_Views.SelectedIndexChanged += new System.EventHandler(this.cb_Views_SelectedIndexChanged);
            // 
            // SPListOpenBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "SPListOpenBrowser";
            this.Size = new System.Drawing.Size(672, 25);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ComboBox cb_ListNames;
        private System.Windows.Forms.Label lb_Sheets;
        private System.Windows.Forms.TextBox txt_URL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbView;
        private System.Windows.Forms.ComboBox cb_Views;
    }
}
