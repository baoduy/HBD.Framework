namespace HBD.WinForms.Controls.Sharepoint
{
    partial class SPSiteContentView
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
            this.spAllSiteContent = new HBD.WinForms.Controls.Sharepoint.SPAllSiteContentTreeControl();
            this.spContentDetailsControl = new HBD.WinForms.Controls.Sharepoint.SPContentDetailsControl();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
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
            this.splitContainer.Panel1.Controls.Add(this.spAllSiteContent);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.spContentDetailsControl);
            this.splitContainer.Size = new System.Drawing.Size(942, 520);
            this.splitContainer.SplitterDistance = 140;
            this.splitContainer.TabIndex = 0;
            // 
            // spAllSiteContent
            // 
            this.spAllSiteContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spAllSiteContent.Location = new System.Drawing.Point(0, 0);
            this.spAllSiteContent.Name = "spAllSiteContent";
            this.spAllSiteContent.Size = new System.Drawing.Size(140, 520);
            this.spAllSiteContent.TabIndex = 0;
            this.spAllSiteContent.Selected += new System.EventHandler<HBD.WinForms.Controls.Sharepoint.Libraries.SPTreeNodeEventArgs>(this.spAllSiteContent_Selected);
            this.spAllSiteContent.SourceChanged += new System.EventHandler(this.spAllSiteContent_SourceChanged);
            // 
            // spContentDetailsControl
            // 
            this.spContentDetailsControl.AutoLoadChildrenControlData = false;
            this.spContentDetailsControl.BrowseControlVisible = false;
            this.spContentDetailsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spContentDetailsControl.Location = new System.Drawing.Point(0, 0);
            this.spContentDetailsControl.Name = "spContentDetailsControl";
            this.spContentDetailsControl.OpenBrowserType = HBD.WinForms.Controls.OpenBrowserType.Custom;
            this.spContentDetailsControl.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.spContentDetailsControl.Size = new System.Drawing.Size(798, 520);
            this.spContentDetailsControl.TabIndex = 0;
            // 
            // SPSiteContentView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Name = "SPSiteContentView";
            this.Size = new System.Drawing.Size(942, 520);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private SPAllSiteContentTreeControl spAllSiteContent;
        private SPContentDetailsControl spContentDetailsControl;
    }
}
