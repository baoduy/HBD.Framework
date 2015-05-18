namespace HBD.WinForms.Controls
{
    partial class DataGridViewSearchControl
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
            this.components = new System.ComponentModel.Container();
            this.txt_Keyword = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.bt_Back = new System.Windows.Forms.Button();
            this.bt_Search = new System.Windows.Forms.Button();
            this.bt_Stop = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_Keyword
            // 
            this.txt_Keyword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Keyword.Location = new System.Drawing.Point(1, 1);
            this.txt_Keyword.Margin = new System.Windows.Forms.Padding(1);
            this.txt_Keyword.Name = "txt_Keyword";
            this.txt_Keyword.Size = new System.Drawing.Size(233, 20);
            this.txt_Keyword.TabIndex = 0;
            this.toolTip.SetToolTip(this.txt_Keyword, "Input keyword and press enter to search");
            this.txt_Keyword.TextChanged += new System.EventHandler(this.txt_Keyword_TextChanged);
            this.txt_Keyword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_Keyword_KeyUp);
            // 
            // bt_Back
            // 
            this.bt_Back.BackColor = System.Drawing.SystemColors.Control;
            this.bt_Back.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bt_Back.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bt_Back.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_Back.ForeColor = System.Drawing.SystemColors.Control;
            this.bt_Back.Image = global::HBD.WinForms.Controls.Properties.Resources.previous;
            this.bt_Back.Location = new System.Drawing.Point(236, 1);
            this.bt_Back.Margin = new System.Windows.Forms.Padding(1);
            this.bt_Back.Name = "bt_Back";
            this.bt_Back.Size = new System.Drawing.Size(22, 20);
            this.bt_Back.TabIndex = 1;
            this.bt_Back.TabStop = false;
            this.toolTip.SetToolTip(this.bt_Back, "Start search");
            this.bt_Back.UseVisualStyleBackColor = false;
            this.bt_Back.Visible = false;
            this.bt_Back.Click += new System.EventHandler(this.bt_Back_Click);
            // 
            // bt_Search
            // 
            this.bt_Search.BackColor = System.Drawing.SystemColors.Control;
            this.bt_Search.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bt_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bt_Search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_Search.ForeColor = System.Drawing.SystemColors.Control;
            this.bt_Search.Image = global::HBD.WinForms.Controls.Properties.Resources.next;
            this.bt_Search.Location = new System.Drawing.Point(260, 1);
            this.bt_Search.Margin = new System.Windows.Forms.Padding(1);
            this.bt_Search.Name = "bt_Search";
            this.bt_Search.Size = new System.Drawing.Size(22, 20);
            this.bt_Search.TabIndex = 2;
            this.bt_Search.TabStop = false;
            this.toolTip.SetToolTip(this.bt_Search, "Start search");
            this.bt_Search.UseVisualStyleBackColor = false;
            this.bt_Search.Click += new System.EventHandler(this.bt_Search_Click);
            // 
            // bt_Stop
            // 
            this.bt_Stop.BackColor = System.Drawing.SystemColors.Control;
            this.bt_Stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bt_Stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_Stop.ForeColor = System.Drawing.SystemColors.Control;
            this.bt_Stop.Image = global::HBD.WinForms.Controls.Properties.Resources.stop;
            this.bt_Stop.Location = new System.Drawing.Point(284, 1);
            this.bt_Stop.Margin = new System.Windows.Forms.Padding(1);
            this.bt_Stop.Name = "bt_Stop";
            this.bt_Stop.Size = new System.Drawing.Size(22, 20);
            this.bt_Stop.TabIndex = 3;
            this.bt_Stop.TabStop = false;
            this.toolTip.SetToolTip(this.bt_Stop, "Stop searching");
            this.bt_Stop.UseVisualStyleBackColor = false;
            this.bt_Stop.Visible = false;
            this.bt_Stop.Click += new System.EventHandler(this.bt_Stop_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.txt_Keyword, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.bt_Stop, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.bt_Search, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.bt_Back, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(307, 22);
            this.tableLayoutPanel.TabIndex = 5;
            // 
            // DataGridViewSearchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "DataGridViewSearchControl";
            this.Size = new System.Drawing.Size(307, 22);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Keyword;
        private System.Windows.Forms.Button bt_Search;
        private System.Windows.Forms.Button bt_Stop;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button bt_Back;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;

    }
}
