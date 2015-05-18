namespace HBD.WinForms.Controls.Comparison
{
    partial class FieldComparisonCollectionControl
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
            this.grColumns = new System.Windows.Forms.GroupBox();
            this.tableCompareColumns = new HBD.WinForms.Controls.ListControlCollection();
            this.tableLabel = new System.Windows.Forms.TableLayoutPanel();
            this.lbSheetB = new System.Windows.Forms.Label();
            this.lbSheetA = new System.Windows.Forms.Label();
            this.grColumns.SuspendLayout();
            this.tableLabel.SuspendLayout();
            this.SuspendLayout();
            // 
            // grColumns
            // 
            this.grColumns.Controls.Add(this.tableCompareColumns);
            this.grColumns.Controls.Add(this.tableLabel);
            this.grColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grColumns.Location = new System.Drawing.Point(0, 0);
            this.grColumns.Name = "grColumns";
            this.grColumns.Size = new System.Drawing.Size(510, 592);
            this.grColumns.TabIndex = 2;
            this.grColumns.TabStop = false;
            this.grColumns.Text = "Compare Columns";
            // 
            // tableCompareColumns
            // 
            this.tableCompareColumns.ChildrenControlType = null;
            this.tableCompareColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCompareColumns.Location = new System.Drawing.Point(3, 32);
            this.tableCompareColumns.Name = "tableCompareColumns";
            this.tableCompareColumns.Size = new System.Drawing.Size(504, 557);
            this.tableCompareColumns.TabIndex = 7;
            this.tableCompareColumns.ChildrenControlAdded += new System.EventHandler<HBD.WinForms.Controls.Events.ListControlCollectionEventArgs>(this.listControlCollection_ChildrenControlAdded);
            this.tableCompareColumns.ChildrenControlRemoved += new System.EventHandler<HBD.WinForms.Controls.Events.ListControlCollectionEventArgs>(this.listControlCollection_ChildrenControlRemoved);
            // 
            // tableLabel
            // 
            this.tableLabel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLabel.ColumnCount = 3;
            this.tableLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLabel.Controls.Add(this.lbSheetB, 1, 0);
            this.tableLabel.Controls.Add(this.lbSheetA, 0, 0);
            this.tableLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLabel.Location = new System.Drawing.Point(3, 16);
            this.tableLabel.Name = "tableLabel";
            this.tableLabel.RowCount = 1;
            this.tableLabel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLabel.Size = new System.Drawing.Size(504, 16);
            this.tableLabel.TabIndex = 6;
            // 
            // lbSheetB
            // 
            this.lbSheetB.AutoSize = true;
            this.lbSheetB.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbSheetB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSheetB.ForeColor = System.Drawing.Color.Blue;
            this.lbSheetB.Location = new System.Drawing.Point(227, 0);
            this.lbSheetB.Name = "lbSheetB";
            this.lbSheetB.Size = new System.Drawing.Size(218, 13);
            this.lbSheetB.TabIndex = 4;
            this.lbSheetB.Text = "Sheet B";
            this.lbSheetB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbSheetA
            // 
            this.lbSheetA.AutoSize = true;
            this.lbSheetA.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbSheetA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSheetA.ForeColor = System.Drawing.Color.Blue;
            this.lbSheetA.Location = new System.Drawing.Point(3, 0);
            this.lbSheetA.Name = "lbSheetA";
            this.lbSheetA.Size = new System.Drawing.Size(218, 13);
            this.lbSheetA.TabIndex = 0;
            this.lbSheetA.Text = "Sheet A";
            this.lbSheetA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FieldComparisonCollectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grColumns);
            this.Name = "FieldComparisonCollectionControl";
            this.Size = new System.Drawing.Size(510, 592);
            this.grColumns.ResumeLayout(false);
            this.tableLabel.ResumeLayout(false);
            this.tableLabel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grColumns;
        private System.Windows.Forms.TableLayoutPanel tableLabel;
        private System.Windows.Forms.Label lbSheetB;
        private System.Windows.Forms.Label lbSheetA;
        private ListControlCollection tableCompareColumns;
    }
}
