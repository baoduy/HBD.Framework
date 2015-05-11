namespace HBD.WinForms.Controls.Sharepoint
{
    partial class SPListView
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lb_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.spContentDetailsControl = new HBD.WinForms.Controls.Sharepoint.SPContentDetailsControl();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lb_Status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 550);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1087, 22);
            this.statusStrip1.TabIndex = 2;
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
            this.lb_Status.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lb_Status.Name = "lb_Status";
            this.lb_Status.Size = new System.Drawing.Size(39, 17);
            this.lb_Status.Text = "Ready";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // spContentDetailsControl
            // 
            this.spContentDetailsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spContentDetailsControl.Location = new System.Drawing.Point(0, 0);
            this.spContentDetailsControl.Name = "spContentDetailsControl";
            this.spContentDetailsControl.OpenBrowserType = HBD.WinForms.Controls.OpenBrowserType.Custom;
            this.spContentDetailsControl.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.spContentDetailsControl.Size = new System.Drawing.Size(1087, 550);
            this.spContentDetailsControl.TabIndex = 3;
            this.spContentDetailsControl.SearchStatusChanged += this.spContentDetailsControl_SearchStatusChanged;
            this.spContentDetailsControl.CellValidating += this.spContentDetailsControl_CellValidating;
            // 
            // SPListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spContentDetailsControl);
            this.Controls.Add(this.statusStrip1);
            this.Name = "SPListView";
            this.Size = new System.Drawing.Size(1087, 572);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lb_Status;
        private System.Windows.Forms.Timer timer;
        private SPContentDetailsControl spContentDetailsControl;
    }
}
