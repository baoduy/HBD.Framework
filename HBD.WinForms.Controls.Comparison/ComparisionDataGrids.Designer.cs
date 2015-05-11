using HBD.WinForms.Controls.Comparison.Core;
namespace HBD.WinForms.Controls.Comparison
{
    partial class ComparisionDataGrids
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
            this.grOption = new System.Windows.Forms.GroupBox();
            this.rd_ShowNotFound = new System.Windows.Forms.RadioButton();
            this.rd_ShowIdentical = new System.Windows.Forms.RadioButton();
            this.rd_ShowComparedData = new System.Windows.Forms.RadioButton();
            this.rd_ShowOriginal = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lb_A = new System.Windows.Forms.Label();
            this.lb_B = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.doubleDataGrids = new HBD.WinForms.Controls.Comparison.DoubleDataGrids();
            this.grOption.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grOption
            // 
            this.grOption.Controls.Add(this.rd_ShowNotFound);
            this.grOption.Controls.Add(this.rd_ShowIdentical);
            this.grOption.Controls.Add(this.rd_ShowComparedData);
            this.grOption.Controls.Add(this.rd_ShowOriginal);
            this.grOption.Dock = System.Windows.Forms.DockStyle.Top;
            this.grOption.Enabled = false;
            this.grOption.Location = new System.Drawing.Point(0, 0);
            this.grOption.Name = "grOption";
            this.grOption.Size = new System.Drawing.Size(605, 44);
            this.grOption.TabIndex = 0;
            this.grOption.TabStop = false;
            this.grOption.Text = "Options";
            // 
            // rd_ShowNotFound
            // 
            this.rd_ShowNotFound.AutoSize = true;
            this.rd_ShowNotFound.Location = new System.Drawing.Point(282, 19);
            this.rd_ShowNotFound.Name = "rd_ShowNotFound";
            this.rd_ShowNotFound.Size = new System.Drawing.Size(127, 17);
            this.rd_ShowNotFound.TabIndex = 6;
            this.rd_ShowNotFound.Text = "Show not found items";
            this.toolTip1.SetToolTip(this.rd_ShowNotFound, "Show not found items only");
            this.rd_ShowNotFound.UseVisualStyleBackColor = true;
            this.rd_ShowNotFound.CheckedChanged += new System.EventHandler(this.rd_CheckedChanged);
            // 
            // rd_ShowIdentical
            // 
            this.rd_ShowIdentical.AutoSize = true;
            this.rd_ShowIdentical.Location = new System.Drawing.Point(415, 19);
            this.rd_ShowIdentical.Name = "rd_ShowIdentical";
            this.rd_ShowIdentical.Size = new System.Drawing.Size(123, 17);
            this.rd_ShowIdentical.TabIndex = 2;
            this.rd_ShowIdentical.Text = "Show Identical Items";
            this.toolTip1.SetToolTip(this.rd_ShowIdentical, "Show identical items only");
            this.rd_ShowIdentical.UseVisualStyleBackColor = true;
            this.rd_ShowIdentical.CheckedChanged += new System.EventHandler(this.rd_CheckedChanged);
            // 
            // rd_ShowComparedData
            // 
            this.rd_ShowComparedData.AutoSize = true;
            this.rd_ShowComparedData.Checked = true;
            this.rd_ShowComparedData.Location = new System.Drawing.Point(138, 19);
            this.rd_ShowComparedData.Name = "rd_ShowComparedData";
            this.rd_ShowComparedData.Size = new System.Drawing.Size(138, 17);
            this.rd_ShowComparedData.TabIndex = 1;
            this.rd_ShowComparedData.TabStop = true;
            this.rd_ShowComparedData.Text = "Show Comparison Items";
            this.toolTip1.SetToolTip(this.rd_ShowComparedData, "Show difference and not found items");
            this.rd_ShowComparedData.UseVisualStyleBackColor = true;
            this.rd_ShowComparedData.CheckedChanged += new System.EventHandler(this.rd_CheckedChanged);
            // 
            // rd_ShowOriginal
            // 
            this.rd_ShowOriginal.AutoSize = true;
            this.rd_ShowOriginal.Location = new System.Drawing.Point(16, 19);
            this.rd_ShowOriginal.Name = "rd_ShowOriginal";
            this.rd_ShowOriginal.Size = new System.Drawing.Size(115, 17);
            this.rd_ShowOriginal.TabIndex = 0;
            this.rd_ShowOriginal.Text = "Show original items";
            this.toolTip1.SetToolTip(this.rd_ShowOriginal, "Show original items from source");
            this.rd_ShowOriginal.UseVisualStyleBackColor = true;
            this.rd_ShowOriginal.CheckedChanged += new System.EventHandler(this.rd_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lb_A, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lb_B, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 44);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(605, 24);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // lb_A
            // 
            this.lb_A.AutoSize = true;
            this.lb_A.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_A.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_A.ForeColor = System.Drawing.Color.Blue;
            this.lb_A.Location = new System.Drawing.Point(3, 0);
            this.lb_A.Name = "lb_A";
            this.lb_A.Size = new System.Drawing.Size(296, 24);
            this.lb_A.TabIndex = 0;
            this.lb_A.Text = "Table A";
            this.lb_A.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_B
            // 
            this.lb_B.AutoSize = true;
            this.lb_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_B.ForeColor = System.Drawing.Color.Blue;
            this.lb_B.Location = new System.Drawing.Point(305, 0);
            this.lb_B.Name = "lb_B";
            this.lb_B.Size = new System.Drawing.Size(297, 24);
            this.lb_B.TabIndex = 1;
            this.lb_B.Text = "Table B";
            this.lb_B.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // doubleDataGrids
            // 
            this.doubleDataGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doubleDataGrids.Location = new System.Drawing.Point(0, 68);
            this.doubleDataGrids.Name = "doubleDataGrids";
            this.doubleDataGrids.Size = new System.Drawing.Size(605, 551);
            this.doubleDataGrids.TabIndex = 1;
            this.doubleDataGrids.SelectionChanged += new System.EventHandler<HBD.WinForms.Controls.Comparison.Core.DataGridSelectionChangedEventArgs>(this.doubleDataGrids_SelectionChanged);
            // 
            // ComparisionDataGrids
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.doubleDataGrids);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.grOption);
            this.Name = "ComparisionDataGrids";
            this.Size = new System.Drawing.Size(605, 619);
            this.grOption.ResumeLayout(false);
            this.grOption.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grOption;
        private DoubleDataGrids doubleDataGrids;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lb_A;
        private System.Windows.Forms.Label lb_B;
        private System.Windows.Forms.RadioButton rd_ShowComparedData;
        private System.Windows.Forms.RadioButton rd_ShowOriginal;
        private System.Windows.Forms.RadioButton rd_ShowIdentical;
        private System.Windows.Forms.RadioButton rd_ShowNotFound;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
