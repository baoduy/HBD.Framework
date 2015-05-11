namespace TestWindowsForms
{
    partial class Form1
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.addRemoveButtonToolStrip = new HBD.WinForms.Controls.AddRemoveButtonToolStrip();
            this.listControlCollection1 = new HBD.WinForms.Controls.ListControlCollection();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRemoveButtonToolStrip});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(792, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // addRemoveButtonToolStrip
            // 
            this.addRemoveButtonToolStrip.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.addRemoveButtonToolStrip.CollectionControl = this.listControlCollection1;
            this.addRemoveButtonToolStrip.Name = "addRemoveButtonToolStrip";
            this.addRemoveButtonToolStrip.Size = new System.Drawing.Size(100, 22);
            this.addRemoveButtonToolStrip.Text = "addRemoveButtonToolStrip1";
            // 
            // listControlCollection1
            // 
            this.listControlCollection1.ChildrenControlType = null;
            this.listControlCollection1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listControlCollection1.Location = new System.Drawing.Point(0, 25);
            this.listControlCollection1.Name = "listControlCollection1";
            this.listControlCollection1.ShowAddRemoveButton = false;
            this.listControlCollection1.Size = new System.Drawing.Size(792, 543);
            this.listControlCollection1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 568);
            this.Controls.Add(this.listControlCollection1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HBD.WinForms.Controls.ListControlCollection listControlCollection1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private HBD.WinForms.Controls.AddRemoveButtonToolStrip addRemoveButtonToolStrip;
    }
}

