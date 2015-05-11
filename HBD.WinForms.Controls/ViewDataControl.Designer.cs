namespace HBD.WinForms.Controls
{
    partial class ViewDataControl
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
            this.hbdDataGridView1 = new HBD.WinForms.Controls.HBDDataGridView();
            this.multiBrowserControl = new HBD.WinForms.Controls.MultiBrowserControl();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewSearchControl = new HBD.WinForms.Controls.DataGridViewSearchControl();
            this.bt_Filter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.hbdDataGridView1)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // hbdDataGridView1
            // 
            this.hbdDataGridView1.AllowUserToAddRows = false;
            this.hbdDataGridView1.AllowUserToOrderColumns = true;
            this.hbdDataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.hbdDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.hbdDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbdDataGridView1.Location = new System.Drawing.Point(0, 24);
            this.hbdDataGridView1.Name = "hbdDataGridView1";
            this.hbdDataGridView1.ReadOnly = true;
            this.hbdDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.hbdDataGridView1.Size = new System.Drawing.Size(895, 258);
            this.hbdDataGridView1.TabIndex = 1;
            // 
            // multiBrowserControl
            // 
            this.multiBrowserControl.AutoLoadChildrenControlData = false;
            this.multiBrowserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.multiBrowserControl.Location = new System.Drawing.Point(0, 0);
            this.multiBrowserControl.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.multiBrowserControl.Name = "multiBrowserControl";
            this.multiBrowserControl.Size = new System.Drawing.Size(662, 24);
            this.multiBrowserControl.TabIndex = 0;
            this.multiBrowserControl.SelectChange += new System.EventHandler(this.multiBrowserControl_SelectChange);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.Controls.Add(this.multiBrowserControl, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.dataGridViewSearchControl, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.bt_Filter, 2, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(895, 24);
            this.tableLayoutPanel.TabIndex = 2;
            // 
            // dataGridViewSearchControl
            // 
            this.dataGridViewSearchControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSearchControl.Enabled = false;
            this.dataGridViewSearchControl.Location = new System.Drawing.Point(668, 1);
            this.dataGridViewSearchControl.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.dataGridViewSearchControl.MinimumSize = new System.Drawing.Size(200, 20);
            this.dataGridViewSearchControl.Name = "dataGridViewSearchControl";
            this.dataGridViewSearchControl.SearchableControl = this.hbdDataGridView1;
            this.dataGridViewSearchControl.Size = new System.Drawing.Size(200, 22);
            this.dataGridViewSearchControl.TabIndex = 1;
            // 
            // bt_Filter
            // 
            this.bt_Filter.BackColor = System.Drawing.SystemColors.Control;
            this.bt_Filter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bt_Filter.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bt_Filter.ForeColor = System.Drawing.SystemColors.Control;
            this.bt_Filter.Image = global::HBD.WinForms.Controls.Properties.Resources.filter;
            this.bt_Filter.Location = new System.Drawing.Point(868, 1);
            this.bt_Filter.Margin = new System.Windows.Forms.Padding(3, 1, 1, 1);
            this.bt_Filter.Name = "bt_Filter";
            this.bt_Filter.Size = new System.Drawing.Size(26, 22);
            this.bt_Filter.TabIndex = 2;
            this.bt_Filter.UseVisualStyleBackColor = false;
            this.bt_Filter.Click += new System.EventHandler(this.bt_Filter_Click);
            // 
            // ViewDataControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hbdDataGridView1);
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "ViewDataControl";
            this.Size = new System.Drawing.Size(895, 282);
            ((System.ComponentModel.ISupportInitialize)(this.hbdDataGridView1)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MultiBrowserControl multiBrowserControl;
        private HBDDataGridView hbdDataGridView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private DataGridViewSearchControl dataGridViewSearchControl;
        private System.Windows.Forms.Button bt_Filter;
    }
}
