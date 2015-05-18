namespace HBD.WinForms.Controls
{
    partial class ExcelOpenBrowser
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lb_Sheet = new System.Windows.Forms.Label();
            this.cb_Sheets = new System.Windows.Forms.ComboBox();
            this.fileOpenBrowser = new HBD.WinForms.Controls.FileOpenBrowser();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel.Controls.Add(this.lb_Sheet, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.cb_Sheets, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.fileOpenBrowser, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(672, 24);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // lb_Sheet
            // 
            this.lb_Sheet.AutoSize = true;
            this.lb_Sheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Sheet.Location = new System.Drawing.Point(504, 0);
            this.lb_Sheet.Margin = new System.Windows.Forms.Padding(0);
            this.lb_Sheet.Name = "lb_Sheet";
            this.lb_Sheet.Size = new System.Drawing.Size(43, 24);
            this.lb_Sheet.TabIndex = 6;
            this.lb_Sheet.Text = "Sheets:";
            this.lb_Sheet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cb_Sheets
            // 
            this.cb_Sheets.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cb_Sheets.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cb_Sheets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_Sheets.FormattingEnabled = true;
            this.cb_Sheets.Location = new System.Drawing.Point(548, 1);
            this.cb_Sheets.Margin = new System.Windows.Forms.Padding(1);
            this.cb_Sheets.Name = "cb_Sheets";
            this.cb_Sheets.Size = new System.Drawing.Size(123, 21);
            this.cb_Sheets.TabIndex = 1;
            this.cb_Sheets.SelectedIndexChanged += new System.EventHandler(this.cb_Sheets_SelectedIndexChanged);
            // 
            // fileOpenBrowser
            // 
            this.fileOpenBrowser.DefaultExt = "*.xlsx";
            this.fileOpenBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileOpenBrowser.Filter = "Excel|*.xlsx|Excel 97-2003|*.xls";
            this.fileOpenBrowser.Location = new System.Drawing.Point(0, 0);
            this.fileOpenBrowser.Margin = new System.Windows.Forms.Padding(0);
            this.fileOpenBrowser.Name = "fileOpenBrowser";
            this.fileOpenBrowser.Size = new System.Drawing.Size(504, 24);
            this.fileOpenBrowser.TabIndex = 0;
            this.fileOpenBrowser.Title = "File:";
            this.fileOpenBrowser.SelectChange += new System.EventHandler(this.fileOpenBrowser1_SelectChange);
            // 
            // ExcelOpenBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "ExcelOpenBrowser";
            this.Size = new System.Drawing.Size(672, 24);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_Sheet;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ComboBox cb_Sheets;
        private FileOpenBrowser fileOpenBrowser;
    }
}
