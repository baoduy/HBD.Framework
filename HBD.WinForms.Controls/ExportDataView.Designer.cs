namespace HBD.WinForms.Controls
{
    partial class ExportDataView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportDataView));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripLabel();
            this.ts_Source = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.ts_ToExcel = new System.Windows.Forms.ToolStripButton();
            this.ts_ToXML = new System.Windows.Forms.ToolStripButton();
            this.ts_CSV = new System.Windows.Forms.ToolStripButton();
            this.viewDataControl1 = new HBD.WinForms.Controls.ViewDataControl();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.ts_Source,
            this.toolStripLabel1,
            this.ts_ToExcel,
            this.ts_ToXML,
            this.ts_CSV});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(662, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(54, 22);
            this.toolStripButton2.Text = "File Type:";
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
            this.toolStripLabel1.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel1.Text = "Export:";
            // 
            // ts_ToExcel
            // 
            this.ts_ToExcel.Image = ((System.Drawing.Image)(resources.GetObject("ts_ToExcel.Image")));
            this.ts_ToExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ts_ToExcel.Name = "ts_ToExcel";
            this.ts_ToExcel.Size = new System.Drawing.Size(67, 22);
            this.ts_ToExcel.Text = "To Excel";
            // 
            // ts_ToXML
            // 
            this.ts_ToXML.Image = ((System.Drawing.Image)(resources.GetObject("ts_ToXML.Image")));
            this.ts_ToXML.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ts_ToXML.Name = "ts_ToXML";
            this.ts_ToXML.Size = new System.Drawing.Size(61, 22);
            this.ts_ToXML.Text = "To XML";
            // 
            // ts_CSV
            // 
            this.ts_CSV.Image = ((System.Drawing.Image)(resources.GetObject("ts_CSV.Image")));
            this.ts_CSV.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ts_CSV.Name = "ts_CSV";
            this.ts_CSV.Size = new System.Drawing.Size(61, 22);
            this.ts_CSV.Text = "To CSV";
            // 
            // viewDataControl1
            // 
            this.viewDataControl1.AutoLoadChildrenControlData = false;
            this.viewDataControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewDataControl1.Location = new System.Drawing.Point(0, 25);
            this.viewDataControl1.Name = "viewDataControl1";
            this.viewDataControl1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.viewDataControl1.Size = new System.Drawing.Size(662, 220);
            this.viewDataControl1.TabIndex = 3;
            // 
            // ExportDataView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.viewDataControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ExportDataView";
            this.Size = new System.Drawing.Size(662, 245);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripButton2;
        private System.Windows.Forms.ToolStripComboBox ts_Source;
        private ViewDataControl viewDataControl1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton ts_ToExcel;
        private System.Windows.Forms.ToolStripButton ts_ToXML;
        private System.Windows.Forms.ToolStripButton ts_CSV;
    }
}
