namespace HBD.WinForms.Controls.Comparison
{
    partial class ListColumnComparision
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
            this.grPrimary = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbSheetB = new System.Windows.Forms.Label();
            this.lbSheetA = new System.Windows.Forms.Label();
            this.primaryKeyColumn = new HBD.WinForms.Controls.Comparison.FieldComparisonControl();
            this.gr_Option = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ch_ColumnByColumn = new System.Windows.Forms.RadioButton();
            this.ch_SpecifyColumn = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ch_RowByRow = new System.Windows.Forms.RadioButton();
            this.ch_PrimaryKey = new System.Windows.Forms.RadioButton();
            this.columnCollection = new HBD.WinForms.Controls.Comparison.FieldComparisonCollectionControl();
            this.grPrimary.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.gr_Option.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // grPrimary
            // 
            this.grPrimary.Controls.Add(this.tableLayoutPanel1);
            this.grPrimary.Controls.Add(this.primaryKeyColumn);
            this.grPrimary.Dock = System.Windows.Forms.DockStyle.Top;
            this.grPrimary.Location = new System.Drawing.Point(0, 80);
            this.grPrimary.Name = "grPrimary";
            this.grPrimary.Size = new System.Drawing.Size(510, 69);
            this.grPrimary.TabIndex = 1;
            this.grPrimary.TabStop = false;
            this.grPrimary.Text = "Primary Key Column";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanel1.Controls.Add(this.lbSheetB, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbSheetA, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(504, 27);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // lbSheetB
            // 
            this.lbSheetB.AutoSize = true;
            this.lbSheetB.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbSheetB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSheetB.ForeColor = System.Drawing.Color.Blue;
            this.lbSheetB.Location = new System.Drawing.Point(212, 0);
            this.lbSheetB.Name = "lbSheetB";
            this.lbSheetB.Size = new System.Drawing.Size(203, 13);
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
            this.lbSheetA.Size = new System.Drawing.Size(203, 13);
            this.lbSheetA.TabIndex = 0;
            this.lbSheetA.Text = "Sheet A";
            this.lbSheetA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // primaryKeyColumn
            // 
            this.primaryKeyColumn.AutoLoadChildrenControlData = false;
            this.primaryKeyColumn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.primaryKeyColumn.Location = new System.Drawing.Point(3, 40);
            this.primaryKeyColumn.Name = "primaryKeyColumn";
            this.primaryKeyColumn.Padding = new System.Windows.Forms.Padding(0, 0, 60, 0);
            this.primaryKeyColumn.Size = new System.Drawing.Size(504, 26);
            this.primaryKeyColumn.TabIndex = 3;
            // 
            // gr_Option
            // 
            this.gr_Option.Controls.Add(this.groupBox4);
            this.gr_Option.Controls.Add(this.groupBox3);
            this.gr_Option.Dock = System.Windows.Forms.DockStyle.Top;
            this.gr_Option.Location = new System.Drawing.Point(0, 0);
            this.gr_Option.Name = "gr_Option";
            this.gr_Option.Size = new System.Drawing.Size(510, 80);
            this.gr_Option.TabIndex = 3;
            this.gr_Option.TabStop = false;
            this.gr_Option.Text = "Option";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ch_ColumnByColumn);
            this.groupBox4.Controls.Add(this.ch_SpecifyColumn);
            this.groupBox4.Location = new System.Drawing.Point(298, 11);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(143, 62);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Column Type";
            // 
            // ch_ColumnByColumn
            // 
            this.ch_ColumnByColumn.AutoSize = true;
            this.ch_ColumnByColumn.Location = new System.Drawing.Point(6, 18);
            this.ch_ColumnByColumn.Name = "ch_ColumnByColumn";
            this.ch_ColumnByColumn.Size = new System.Drawing.Size(112, 17);
            this.ch_ColumnByColumn.TabIndex = 2;
            this.ch_ColumnByColumn.Text = "Column by Column";
            this.ch_ColumnByColumn.UseVisualStyleBackColor = true;
            // 
            // ch_SpecifyColumn
            // 
            this.ch_SpecifyColumn.AutoSize = true;
            this.ch_SpecifyColumn.Checked = true;
            this.ch_SpecifyColumn.Location = new System.Drawing.Point(6, 41);
            this.ch_SpecifyColumn.Name = "ch_SpecifyColumn";
            this.ch_SpecifyColumn.Size = new System.Drawing.Size(129, 17);
            this.ch_SpecifyColumn.TabIndex = 3;
            this.ch_SpecifyColumn.TabStop = true;
            this.ch_SpecifyColumn.Text = "Columns Specification";
            this.ch_SpecifyColumn.UseVisualStyleBackColor = true;
            this.ch_SpecifyColumn.CheckedChanged += new System.EventHandler(this.ch_SpecifyColumn_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ch_RowByRow);
            this.groupBox3.Controls.Add(this.ch_PrimaryKey);
            this.groupBox3.Location = new System.Drawing.Point(57, 10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(124, 64);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Row Type";
            // 
            // ch_RowByRow
            // 
            this.ch_RowByRow.AutoSize = true;
            this.ch_RowByRow.Location = new System.Drawing.Point(6, 19);
            this.ch_RowByRow.Name = "ch_RowByRow";
            this.ch_RowByRow.Size = new System.Drawing.Size(86, 17);
            this.ch_RowByRow.TabIndex = 0;
            this.ch_RowByRow.Text = "Row by Row";
            this.ch_RowByRow.UseVisualStyleBackColor = true;
            // 
            // ch_PrimaryKey
            // 
            this.ch_PrimaryKey.AutoSize = true;
            this.ch_PrimaryKey.Checked = true;
            this.ch_PrimaryKey.Location = new System.Drawing.Point(6, 42);
            this.ch_PrimaryKey.Name = "ch_PrimaryKey";
            this.ch_PrimaryKey.Size = new System.Drawing.Size(80, 17);
            this.ch_PrimaryKey.TabIndex = 1;
            this.ch_PrimaryKey.TabStop = true;
            this.ch_PrimaryKey.Text = "Primary Key";
            this.ch_PrimaryKey.UseVisualStyleBackColor = true;
            this.ch_PrimaryKey.CheckedChanged += new System.EventHandler(this.dataPrimaryKey_CheckedChanged);
            // 
            // columnCollection
            // 
            this.columnCollection.AutoLoadChildrenControlData = false;
            this.columnCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.columnCollection.Header = "Compare Columns";
            this.columnCollection.Location = new System.Drawing.Point(0, 149);
            this.columnCollection.Name = "columnCollection";
            this.columnCollection.TitleA = "Sheet A";
            this.columnCollection.TitleB = "Sheet B";
            this.columnCollection.ShowTitle = false;
            this.columnCollection.Size = new System.Drawing.Size(510, 443);
            this.columnCollection.TabIndex = 4;
            this.columnCollection.ColumnAdded += new System.EventHandler<HBD.WinForms.Controls.Comparison.Events.CompareFieldEventArgs>(this.columnCollection_ColumnAdded);
            // 
            // ListColumnComparision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.columnCollection);
            this.Controls.Add(this.grPrimary);
            this.Controls.Add(this.gr_Option);
            this.Name = "ListColumnComparision";
            this.Size = new System.Drawing.Size(510, 592);
            this.grPrimary.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.gr_Option.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grPrimary;
        private System.Windows.Forms.Label lbSheetA;
        private FieldComparisonControl primaryKeyColumn;
        private System.Windows.Forms.Label lbSheetB;
        private System.Windows.Forms.GroupBox gr_Option;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton ch_ColumnByColumn;
        private System.Windows.Forms.RadioButton ch_SpecifyColumn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton ch_RowByRow;
        private System.Windows.Forms.RadioButton ch_PrimaryKey;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private FieldComparisonCollectionControl columnCollection;
    }
}
