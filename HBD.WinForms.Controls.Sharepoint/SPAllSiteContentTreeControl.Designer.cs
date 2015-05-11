namespace HBD.WinForms.Controls.Sharepoint
{
    partial class SPAllSiteContentTreeControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPAllSiteContentTreeControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lb_URL = new System.Windows.Forms.ToolStripLabel();
            this.data_URL = new System.Windows.Forms.ToolStripTextBox();
            this.bt_Open = new System.Windows.Forms.ToolStripButton();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lb_URL,
            this.data_URL,
            this.bt_Open});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(302, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lb_URL
            // 
            this.lb_URL.Name = "lb_URL";
            this.lb_URL.Size = new System.Drawing.Size(31, 22);
            this.lb_URL.Text = "URL:";
            // 
            // data_URL
            // 
            this.data_URL.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.data_URL.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.data_URL.AutoSize = false;
            this.data_URL.AutoToolTip = true;
            this.data_URL.Name = "data_URL";
            this.data_URL.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.data_URL.Size = new System.Drawing.Size(100, 25);
            this.data_URL.TextChanged += new System.EventHandler(this.data_URL_TextChanged);
            // 
            // bt_Open
            // 
            this.bt_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_Open.Image = global::HBD.WinForms.Controls.Sharepoint.Properties.Resources.open;
            this.bt_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Open.Name = "bt_Open";
            this.bt_Open.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.bt_Open.Size = new System.Drawing.Size(23, 22);
            this.bt_Open.Text = "Open";
            this.bt_Open.Click += new System.EventHandler(this.bt_Open_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // treeView
            // 
            this.treeView.BackColor = System.Drawing.SystemColors.Control;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.Location = new System.Drawing.Point(0, 25);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(302, 489);
            this.treeView.TabIndex = 1;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "site.png");
            this.imageList.Images.SetKeyName(1, "list.png");
            this.imageList.Images.SetKeyName(2, "view.png");
            this.imageList.Images.SetKeyName(3, "group.png");
            this.imageList.Images.SetKeyName(4, "user.png");
            // 
            // SPAllSiteContentTreeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.toolStrip1);
            this.Name = "SPAllSiteContentTreeControl";
            this.Size = new System.Drawing.Size(302, 514);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel lb_URL;
        private System.Windows.Forms.ToolStripTextBox data_URL;
        private System.Windows.Forms.ToolStripButton bt_Open;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ImageList imageList;
    }
}
