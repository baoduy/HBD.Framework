namespace HBD.WinForms.Controls.Comparison
{
    partial class DoubleDataGrids
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dataGridA = new HBD.WinForms.Controls.HBDDataGridView();
            this.dataGridB = new HBD.WinForms.Controls.HBDDataGridView();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridB)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dataGridA);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.dataGridB);
            this.splitContainer.Size = new System.Drawing.Size(600, 549);
            this.splitContainer.SplitterDistance = 300;
            this.splitContainer.TabIndex = 0;
            // 
            // dataGridA
            // 
            this.dataGridA.AllowUserToAddRows = false;
            this.dataGridA.AllowUserToDeleteRows = false;
            this.dataGridA.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridA.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridA.ColumnSortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridA.Location = new System.Drawing.Point(0, 0);
            this.dataGridA.Name = "dataGridA";
            this.dataGridA.ReadOnly = true;
            this.dataGridA.Size = new System.Drawing.Size(300, 549);
            this.dataGridA.TabIndex = 0;
            this.dataGridA.SelectionModeChanged += new System.EventHandler(this.dataGridA_SelectionModeChanged);
            this.dataGridA.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGrid_ColumnAdded);
            this.dataGridA.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGrid_Scroll);
            this.dataGridA.SelectionChanged += new System.EventHandler(this.dataGrid_SelectionChanged);
            // 
            // dataGridB
            // 
            this.dataGridB.AllowUserToAddRows = false;
            this.dataGridB.AllowUserToDeleteRows = false;
            this.dataGridB.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridB.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridB.ColumnSortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridB.Location = new System.Drawing.Point(0, 0);
            this.dataGridB.Name = "dataGridB";
            this.dataGridB.ReadOnly = true;
            this.dataGridB.Size = new System.Drawing.Size(296, 549);
            this.dataGridB.TabIndex = 0;
            this.dataGridB.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGrid_ColumnAdded);
            this.dataGridB.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGrid_Scroll);
            this.dataGridB.SelectionChanged += new System.EventHandler(this.dataGrid_SelectionChanged);
            // 
            // DoubleDataGrids
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Name = "DoubleDataGrids";
            this.Size = new System.Drawing.Size(600, 549);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private HBDDataGridView dataGridA;
        private HBDDataGridView dataGridB;
    }
}
