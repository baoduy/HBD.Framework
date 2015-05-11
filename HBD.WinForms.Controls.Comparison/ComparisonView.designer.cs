
using HBD.WinForms.Controls.Comparison.Events;
using HBD.WinForms.Controls.Comparison.Core;

namespace HBD.WinForms.Controls.Comparison
{
    partial class ComparisonView
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComparisonView));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabInputData = new System.Windows.Forms.TabPage();
            this.listColumnComparision = new HBD.WinForms.Controls.Comparison.ListColumnComparision();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btCompare = new System.Windows.Forms.Button();
            this.multiComparisonBrowser = new HBD.WinForms.Controls.Comparison.MultiBrowserComparisonControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lb_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabResult = new System.Windows.Forms.TabPage();
            this.comparisionDataGrids1 = new HBD.WinForms.Controls.Comparison.ComparisionDataGrids();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripLabel();
            this.ts_Source = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.ts_Destination = new System.Windows.Forms.ToolStripComboBox();
            this.ts_AutoLoadColumns = new System.Windows.Forms.ToolStripButton();
            this.ts_progressbar = new System.Windows.Forms.ToolStripProgressBar();
            this.tabControl1.SuspendLayout();
            this.tabInputData.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabResult.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabInputData);
            this.tabControl1.Controls.Add(this.tabResult);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(792, 755);
            this.tabControl1.TabIndex = 0;
            // 
            // tabInputData
            // 
            this.tabInputData.Controls.Add(this.listColumnComparision);
            this.tabInputData.Controls.Add(this.groupBox2);
            this.tabInputData.Controls.Add(this.multiComparisonBrowser);
            this.tabInputData.Controls.Add(this.statusStrip1);
            this.tabInputData.Location = new System.Drawing.Point(4, 22);
            this.tabInputData.Name = "tabInputData";
            this.tabInputData.Padding = new System.Windows.Forms.Padding(3);
            this.tabInputData.Size = new System.Drawing.Size(784, 729);
            this.tabInputData.TabIndex = 0;
            this.tabInputData.Text = "Input Data";
            this.tabInputData.UseVisualStyleBackColor = true;
            // 
            // listColumnComparision
            // 
            this.listColumnComparision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listColumnComparision.Location = new System.Drawing.Point(3, 85);
            this.listColumnComparision.Name = "listColumnComparision";
            this.listColumnComparision.Size = new System.Drawing.Size(778, 571);
            this.listColumnComparision.TabIndex = 5;
            this.listColumnComparision.TitleA = "Sheet A";
            this.listColumnComparision.TitleB = "Sheet B";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btCompare);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(3, 656);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(778, 48);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // btCompare
            // 
            this.btCompare.Dock = System.Windows.Forms.DockStyle.Right;
            this.btCompare.Enabled = false;
            this.btCompare.Location = new System.Drawing.Point(625, 16);
            this.btCompare.Name = "btCompare";
            this.btCompare.Size = new System.Drawing.Size(150, 29);
            this.btCompare.TabIndex = 4;
            this.btCompare.Text = "Compare";
            this.btCompare.UseVisualStyleBackColor = true;
            this.btCompare.Click += new System.EventHandler(this.btCompare_Click);
            // 
            // multiComparisonBrowser
            // 
            this.multiComparisonBrowser.Dock = System.Windows.Forms.DockStyle.Top;
            this.multiComparisonBrowser.Location = new System.Drawing.Point(3, 3);
            this.multiComparisonBrowser.Name = "multiComparisonBrowser";
            this.multiComparisonBrowser.Size = new System.Drawing.Size(778, 82);
            this.multiComparisonBrowser.TabIndex = 3;
            this.multiComparisonBrowser.TabStop = false;
            this.multiComparisonBrowser.Title = "Files Comparison";
            this.multiComparisonBrowser.SelectionChanged += new System.EventHandler<HBD.WinForms.Controls.Comparison.Events.FileSelectedEventArgs>(this.MultiCompareBrowser_SelectionChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.ts_progressbar,
            this.lb_Status});
            this.statusStrip1.Location = new System.Drawing.Point(3, 704);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(778, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(42, 17);
            this.toolStripStatusLabel1.Text = "Status:";
            // 
            // lb_Status
            // 
            this.lb_Status.ForeColor = System.Drawing.Color.Blue;
            this.lb_Status.Name = "lb_Status";
            this.lb_Status.Size = new System.Drawing.Size(39, 17);
            this.lb_Status.Text = "Ready";
            // 
            // tabResult
            // 
            this.tabResult.Controls.Add(this.comparisionDataGrids1);
            this.tabResult.Location = new System.Drawing.Point(4, 22);
            this.tabResult.Name = "tabResult";
            this.tabResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabResult.Size = new System.Drawing.Size(784, 729);
            this.tabResult.TabIndex = 1;
            this.tabResult.Text = "Result";
            this.tabResult.UseVisualStyleBackColor = true;
            // 
            // comparisionDataGrids1
            // 
            this.comparisionDataGrids1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comparisionDataGrids1.Location = new System.Drawing.Point(3, 3);
            this.comparisionDataGrids1.Name = "comparisionDataGrids1";
            this.comparisionDataGrids1.Size = new System.Drawing.Size(778, 723);
            this.comparisionDataGrids1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.ts_Source,
            this.toolStripLabel1,
            this.ts_Destination,
            this.ts_AutoLoadColumns});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(792, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(59, 22);
            this.toolStripButton2.Text = "Compare:";
            // 
            // ts_Source
            // 
            this.ts_Source.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ts_Source.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ts_Source.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ts_Source.Name = "ts_Source";
            this.ts_Source.Size = new System.Drawing.Size(120, 25);
            this.ts_Source.SelectedIndexChanged += new System.EventHandler(this.ts_Source_SelectedIndexChanged);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(30, 22);
            this.toolStripLabel1.Text = "with";
            // 
            // ts_Destination
            // 
            this.ts_Destination.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ts_Destination.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ts_Destination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ts_Destination.Name = "ts_Destination";
            this.ts_Destination.Size = new System.Drawing.Size(120, 25);
            this.ts_Destination.SelectedIndexChanged += new System.EventHandler(this.ts_Destination_SelectedIndexChanged);
            // 
            // ts_AutoLoadColumns
            // 
            this.ts_AutoLoadColumns.Image = ((System.Drawing.Image)(resources.GetObject("ts_AutoLoadColumns.Image")));
            this.ts_AutoLoadColumns.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ts_AutoLoadColumns.Name = "ts_AutoLoadColumns";
            this.ts_AutoLoadColumns.Size = new System.Drawing.Size(177, 22);
            this.ts_AutoLoadColumns.Text = "Generate Compare Columns";
            this.ts_AutoLoadColumns.Click += new System.EventHandler(this.ts_AutoLoadColumns_Click);
            // 
            // ts_progressbar
            // 
            this.ts_progressbar.Name = "ts_progressbar";
            this.ts_progressbar.Size = new System.Drawing.Size(100, 16);
            this.ts_progressbar.Visible = false;
            // 
            // ComparisonView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ComparisonView";
            this.Size = new System.Drawing.Size(792, 780);
            this.tabControl1.ResumeLayout(false);
            this.tabInputData.ResumeLayout(false);
            this.tabInputData.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabResult.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabInputData;
        private HBD.WinForms.Controls.Comparison.ListColumnComparision listColumnComparision;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btCompare;
        private MultiBrowserComparisonControl multiComparisonBrowser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabResult;
        private System.Windows.Forms.ToolTip toolTip;
        private ComparisionDataGrids comparisionDataGrids1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripButton2;
        private System.Windows.Forms.ToolStripComboBox ts_Source;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox ts_Destination;
        private System.Windows.Forms.ToolStripButton ts_AutoLoadColumns;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lb_Status;
        private System.Windows.Forms.ToolStripProgressBar ts_progressbar;
    }
}