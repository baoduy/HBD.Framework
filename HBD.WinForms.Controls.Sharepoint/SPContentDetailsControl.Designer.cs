namespace HBD.WinForms.Controls.Sharepoint
{
    partial class SPContentDetailsControl
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
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ct_Details = new System.Windows.Forms.ToolStripMenuItem();
            this.ct_EditValue = new System.Windows.Forms.ToolStripMenuItem();
            this.ct_HideSelectedRows = new System.Windows.Forms.ToolStripMenuItem();
            this.ct_ShowHiddenRows = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ct_Details,
            this.ct_EditValue,
            this.ct_HideSelectedRows,
            this.ct_ShowHiddenRows});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(186, 92);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            this.contextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_ItemClicked);
            // 
            // ct_Details
            // 
            this.ct_Details.Image = global::HBD.WinForms.Controls.Sharepoint.Properties.Resources.view;
            this.ct_Details.Name = "ct_Details";
            this.ct_Details.Size = new System.Drawing.Size(185, 22);
            this.ct_Details.Text = "View Details";
            // 
            // ct_EditValue
            // 
            this.ct_EditValue.Image = global::HBD.WinForms.Controls.Sharepoint.Properties.Resources.edit;
            this.ct_EditValue.Name = "ct_EditValue";
            this.ct_EditValue.Size = new System.Drawing.Size(185, 22);
            this.ct_EditValue.Text = "Edit Value";
            // 
            // ct_HideSelectedRows
            // 
            this.ct_HideSelectedRows.Name = "ct_HideSelectedRows";
            this.ct_HideSelectedRows.Size = new System.Drawing.Size(185, 22);
            this.ct_HideSelectedRows.Text = "Hide Selected Row(s)";
            // 
            // ct_ShowHiddenRows
            // 
            this.ct_ShowHiddenRows.Enabled = false;
            this.ct_ShowHiddenRows.Name = "ct_ShowHiddenRows";
            this.ct_ShowHiddenRows.Size = new System.Drawing.Size(185, 22);
            this.ct_ShowHiddenRows.Text = "Show Hidden Row(s)";
            // 
            // SPContentDetailsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SPContentDetailsControl";
            this.Size = new System.Drawing.Size(1087, 572);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem ct_Details;
        private System.Windows.Forms.ToolStripMenuItem ct_EditValue;
        private System.Windows.Forms.ToolStripMenuItem ct_HideSelectedRows;
        private System.Windows.Forms.ToolStripMenuItem ct_ShowHiddenRows;
    }
}
