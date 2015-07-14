namespace HBD.WinForms.Controls.Comparison
{
    partial class MultiBrowserComparisonControl
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
            this.gr_Parent = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.multiBrowserA = new HBD.WinForms.Controls.MultiBrowserControl();
            this.multiBrowserB = new HBD.WinForms.Controls.MultiBrowserControl();
            this.gr_Parent.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // gr_Parent
            // 
            this.gr_Parent.Controls.Add(this.tableLayoutPanel);
            this.gr_Parent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gr_Parent.Location = new System.Drawing.Point(0, 0);
            this.gr_Parent.Name = "gr_Parent";
            this.gr_Parent.Size = new System.Drawing.Size(577, 82);
            this.gr_Parent.TabIndex = 0;
            this.gr_Parent.TabStop = false;
            this.gr_Parent.Text = "Comparison Sources";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.multiBrowserA, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.multiBrowserB, 0, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(571, 63);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(3, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "   - Compare With:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // multiBrowserA
            // 
            this.multiBrowserA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.multiBrowserA.Location = new System.Drawing.Point(0, 0);
            this.multiBrowserA.Margin = new System.Windows.Forms.Padding(0);
            this.multiBrowserA.Name = "multiBrowserA";
            this.multiBrowserA.Size = new System.Drawing.Size(571, 24);
            this.multiBrowserA.TabIndex = 3;
            this.multiBrowserA.SelectChange += new System.EventHandler(this.MultiBrowserComparisonControl_SelectChange);
            // 
            // multiBrowserB
            // 
            this.multiBrowserB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.multiBrowserB.Location = new System.Drawing.Point(0, 39);
            this.multiBrowserB.Margin = new System.Windows.Forms.Padding(0);
            this.multiBrowserB.Name = "multiBrowserB";
            this.multiBrowserB.Size = new System.Drawing.Size(571, 24);
            this.multiBrowserB.TabIndex = 4;
            this.multiBrowserB.SelectChange += new System.EventHandler(this.MultiBrowserComparisonControl_SelectChange);
            // 
            // MultiBrowserComparisonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gr_Parent);
            this.Name = "MultiBrowserComparisonControl";
            this.Size = new System.Drawing.Size(577, 82);
            this.gr_Parent.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gr_Parent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label label1;
        private MultiBrowserControl multiBrowserA;
        private MultiBrowserControl multiBrowserB;
    }
}
