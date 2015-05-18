namespace TestWinForms
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
            this.comparisionDataGrids1 = new HBD.WinForms.Controls.Comparison.ComparisionDataGrids();
            this.SuspendLayout();
            // 
            // comparisionDataGrids1
            // 
            this.comparisionDataGrids1.DataSource = null;
            this.comparisionDataGrids1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comparisionDataGrids1.Location = new System.Drawing.Point(0, 0);
            this.comparisionDataGrids1.Name = "comparisionDataGrids1";
            this.comparisionDataGrids1.Size = new System.Drawing.Size(844, 592);
            this.comparisionDataGrids1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 592);
            this.Controls.Add(this.comparisionDataGrids1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private HBD.WinForms.Controls.Comparison.ComparisionDataGrids comparisionDataGrids1;
    }
}

