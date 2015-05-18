namespace HBD.WinForms.Controls
{
    partial class DetailViewControl
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
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridSearchControl1 = new HBD.WinForms.Controls.DataGridViewSearchControl();
            ((System.ComponentModel.ISupportInitialize)(this.hbdDataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hbdDataGridView1
            // 
            this.hbdDataGridView1.AllowUserToAddRows = false;
            this.hbdDataGridView1.AllowUserToDeleteRows = false;
            this.hbdDataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.hbdDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.hbdDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Title,
            this.Value});
            this.hbdDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbdDataGridView1.Location = new System.Drawing.Point(0, 40);
            this.hbdDataGridView1.Name = "hbdDataGridView1";
            this.hbdDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.hbdDataGridView1.Size = new System.Drawing.Size(897, 459);
            this.hbdDataGridView1.TabIndex = 1;
            // 
            // Title
            // 
            this.Title.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Title.HeaderText = "Title";
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            this.Title.Width = 50;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridSearchControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0, 0, 4, 6);
            this.groupBox1.Size = new System.Drawing.Size(897, 40);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // dataGridSearchControl1
            // 
            this.dataGridSearchControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.dataGridSearchControl1.Enabled = false;
            this.dataGridSearchControl1.Location = new System.Drawing.Point(693, 13);
            this.dataGridSearchControl1.MinimumSize = new System.Drawing.Size(200, 20);
            this.dataGridSearchControl1.Name = "dataGridSearchControl1";
            this.dataGridSearchControl1.SearchableControl = this.hbdDataGridView1;
            this.dataGridSearchControl1.Size = new System.Drawing.Size(200, 21);
            this.dataGridSearchControl1.TabIndex = 0;
            // 
            // DetailViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.hbdDataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Name = "DetailViewControl";
            this.Size = new System.Drawing.Size(897, 499);
            ((System.ComponentModel.ISupportInitialize)(this.hbdDataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private HBDDataGridView hbdDataGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private DataGridViewSearchControl dataGridSearchControl1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;

    }
}
