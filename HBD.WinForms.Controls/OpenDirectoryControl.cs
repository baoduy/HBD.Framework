using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Core;
using HBD.WinForms.Controls.Core;

namespace HBD.WinForms.Controls
{
    [DefaultProperty("SelectedPath"), DefaultEvent("SelectChange")]
    public partial class OpenDirectoryControl : HBDControl
    {
        public OpenDirectoryControl()
        {
            InitializeComponent();
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue("")]
        public string SelectedPath
        {
            get { return this.txt_FolderPath.Text; }
            set
            {
                if (!PathExtension.IsPathExisted(value)) return;
                if (!PathExtension.IsDirectory(value)) return;

                this.txt_FolderPath.Text = value;
            }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true)]
        public bool ShowTitle
        {
            get { return this.lb_Title.Visible; }
            set { this.lb_Title.Visible = value; }
        }

        bool _SelectChangeFired = false;
        public event EventHandler SelectChange = null;
        protected virtual void OnSelectChange(EventArgs e)
        {
            if (this.SelectChange != null && !_SelectChangeFired)
                this.SelectChange(this, e);
            _SelectChangeFired = true;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Height = this.txt_FolderPath.Height + 2;
        }

        public override bool ValidateData()
        {
            if (!base.ValidateData())
                return false;

            return this.ValidateControls(this.txt_FolderPath);
        }

        private void bt_Browse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.txt_FolderPath.Text = dialog.SelectedPath;
                    this._SelectChangeFired = false;
                    this.OnSelectChange(EventArgs.Empty);
                }
            }
        }

        private void txt_FolderPath_Leave(object sender, EventArgs e)
        {
            this.OnSelectChange(e);
        }

        private void txt_FolderPath_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.OnSelectChange(e);
        }

        private void txt_FolderPath_TextChanged(object sender, EventArgs e)
        {
            this._SelectChangeFired = false;
        }
    }
}
