namespace TestWindowsForms
{
    partial class Form2
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
            this.sqlConnectionControl1 = new HBD.WinForms.Controls.SQLConnectionControl();
            this.SuspendLayout();
            // 
            // sqlConnectionControl1
            // 
            this.sqlConnectionControl1.Location = new System.Drawing.Point(126, 21);
            this.sqlConnectionControl1.Name = "sqlConnectionControl1";
            this.sqlConnectionControl1.Size = new System.Drawing.Size(403, 284);
            this.sqlConnectionControl1.TabIndex = 0;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 511);
            this.Controls.Add(this.sqlConnectionControl1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);

        }

        #endregion

        private HBD.WinForms.Controls.SQLConnectionControl sqlConnectionControl1;


    }
}