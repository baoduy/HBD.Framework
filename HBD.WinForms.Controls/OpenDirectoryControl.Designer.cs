namespace HBD.WinForms.Controls
{
    partial class OpenDirectoryControl
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
            this.txt_FolderPath = new System.Windows.Forms.TextBox();
            this.lb_Title = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.bt_Browse, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.txt_FolderPath, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.lb_Title, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(579, 25);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // bt_Browse
            // 
            this.bt_Browse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bt_Browse.Location = new System.Drawing.Point(504, 1);
            this.bt_Browse.Margin = new System.Windows.Forms.Padding(1);
            this.bt_Browse.Name = "bt_Browse";
            this.bt_Browse.Size = new System.Drawing.Size(74, 23);
            this.bt_Browse.TabIndex = 1;
            this.bt_Browse.Text = "Browse";
            this.bt_Browse.UseVisualStyleBackColor = true;
            this.bt_Browse.Click += new System.EventHandler(this.bt_Browse_Click);
            // 
            // txt_FolderPath
            // 
            this.txt_FolderPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_FolderPath.Location = new System.Drawing.Point(47, 2);
            this.txt_FolderPath.Margin = new System.Windows.Forms.Padding(2);
            this.txt_FolderPath.Name = "txt_FolderPath";
            this.txt_FolderPath.Size = new System.Drawing.Size(454, 20);
            this.txt_FolderPath.TabIndex = 0;
            this.txt_FolderPath.TextChanged += new System.EventHandler(this.txt_FolderPath_TextChanged);
            this.txt_FolderPath.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_FolderPath_KeyUp);
            this.txt_FolderPath.Leave += new System.EventHandler(this.txt_FolderPath_Leave);
            // 
            // lb_Title
            // 
            this.lb_Title.AutoSize = true;
            this.lb_Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Title.Location = new System.Drawing.Point(3, 0);
            this.lb_Title.Name = "lb_Title";
            this.lb_Title.Size = new System.Drawing.Size(39, 25);
            this.lb_Title.TabIndex = 5;
            this.lb_Title.Text = "Folder:";
            this.lb_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OpenDirectoryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "OpenDirectoryControl";
            this.Size = new System.Drawing.Size(579, 25);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button bt_Browse;
        private System.Windows.Forms.TextBox txt_FolderPath;
        private System.Windows.Forms.Label lb_Title;
    }
}
