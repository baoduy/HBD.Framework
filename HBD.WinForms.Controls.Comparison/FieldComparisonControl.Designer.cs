namespace HBD.WinForms.Controls.Comparison
{
    partial class FieldComparisonControl
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxA = new System.Windows.Forms.ComboBox();
            this.comboBoxB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chEnable = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel.Controls.Add(this.comboBoxA, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxB, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.chEnable, 3, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(462, 29);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // comboBoxA
            // 
            this.comboBoxA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxA.FormattingEnabled = true;
            this.comboBoxA.Location = new System.Drawing.Point(3, 3);
            this.comboBoxA.Name = "comboBoxA";
            this.comboBoxA.Size = new System.Drawing.Size(200, 21);
            this.comboBoxA.TabIndex = 0;
            this.comboBoxA.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxB
            // 
            this.comboBoxB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxB.FormattingEnabled = true;
            this.comboBoxB.Location = new System.Drawing.Point(235, 3);
            this.comboBoxB.Name = "comboBoxB";
            this.comboBoxB.Size = new System.Drawing.Size(200, 21);
            this.comboBoxB.TabIndex = 1;
            this.comboBoxB.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(209, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "=";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chEnable
            // 
            this.chEnable.AutoSize = true;
            this.chEnable.Checked = true;
            this.chEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chEnable.Location = new System.Drawing.Point(441, 3);
            this.chEnable.Name = "chEnable";
            this.chEnable.Size = new System.Drawing.Size(18, 23);
            this.chEnable.TabIndex = 4;
            this.chEnable.UseVisualStyleBackColor = true;
            this.chEnable.Visible = false;
            this.chEnable.CheckedChanged += new System.EventHandler(this.chEnable_CheckedChanged);
            // 
            // FieldComparisonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "FieldComparisonControl";
            this.Size = new System.Drawing.Size(462, 29);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ComboBox comboBoxA;
        private System.Windows.Forms.ComboBox comboBoxB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chEnable;
    }
}
