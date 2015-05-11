namespace HBD.WinForms.Controls
{
    partial class FileOpenBrowser
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
            this.bt_Browse = new System.Windows.Forms.Button();
            this.txt_FileName = new System.Windows.Forms.TextBox();
            this.lb_Title = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.bt_Browse, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.txt_FileName, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.lb_Title, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(672, 22);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // bt_Browse
            // 
            this.bt_Browse.AutoSize = true;
            this.bt_Browse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bt_Browse.Location = new System.Drawing.Point(619, 1);
            this.bt_Browse.Margin = new System.Windows.Forms.Padding(1);
            this.bt_Browse.Name = "bt_Browse";
            this.bt_Browse.Size = new System.Drawing.Size(52, 20);
            this.bt_Browse.TabIndex = 1;
            this.bt_Browse.Text = "Browse";
            this.bt_Browse.UseVisualStyleBackColor = true;
            this.bt_Browse.Click += new System.EventHandler(this.bt_Browse_Click);
            // 
            // txt_FileName
            // 
            this.txt_FileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_FileName.Location = new System.Drawing.Point(34, 2);
            this.txt_FileName.Margin = new System.Windows.Forms.Padding(2);
            this.txt_FileName.Name = "txt_FileName";
            this.txt_FileName.Size = new System.Drawing.Size(567, 20);
            this.txt_FileName.TabIndex = 0;
            this.txt_FileName.TextChanged += new System.EventHandler(this.txt_FileName_TextChanged);
            this.txt_FileName.DoubleClick += new System.EventHandler(this.txt_FileName_DoubleClick);
            this.txt_FileName.Leave += new System.EventHandler(this.txt_FileName_Leave);
            // 
            // lb_Title
            // 
            this.lb_Title.AutoSize = true;
            this.lb_Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Title.Location = new System.Drawing.Point(3, 0);
            this.lb_Title.Name = "lb_Title";
            this.lb_Title.Size = new System.Drawing.Size(26, 22);
            this.lb_Title.TabIndex = 5;
            this.lb_Title.Text = "File:";
            this.lb_Title.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FileOpenBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "FileOpenBrowser";
            this.Size = new System.Drawing.Size(672, 22);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_Browse;
        private System.Windows.Forms.TextBox txt_FileName;
        private System.Windows.Forms.Label lb_Title;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}
