using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Data.Excel;
using System.IO;
using HBD.WinForms.Controls.Core;
using HBD.Framework.Core;
using HBD.WinForms.Controls.Attributes;

namespace HBD.WinForms.Controls
{
    [DefaultProperty("SourcePath"), DefaultEvent("SelectChange")]
    public partial class FileOpenBrowser : HBDControl, IOpenBrowserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue("File")]
        public string Title
        {
            get { return this.lb_Title.Text; }
            set { this.lb_Title.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue("Browse")]
        public string BrowseText
        {
            get { return this.bt_Browse.Text; }
            set { this.bt_Browse.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(null)]
        public string Filter { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(null)]
        public string DefaultExt { get; set; }

        #region IOpenBrowser
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        [ControlPropertyState]
        public string SourcePath
        {
            get { return this.txt_FileName.Text; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                var ext = Path.GetExtension(value);

                if (string.IsNullOrEmpty(ext))
                    return;
                if (!string.IsNullOrEmpty(this.Filter)
                    && !this.Filter.Contains(ext))
                    return;
                if (!PathExtension.IsPathExisted(value))
                    return;

                this.txt_FileName.Text = value;
                this.OnSelectChange(EventArgs.Empty);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        [ControlPropertyState]
        public string SourceName { get; set; }

        bool _SelectChangeFired = false;
        public event EventHandler SelectChange = null;
        protected virtual void OnSelectChange(EventArgs e)
        {
            if (this.SelectChange != null && !_SelectChangeFired)
                this.SelectChange(this, e);
            _SelectChangeFired = true;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public FileOpenBrowser()
        {
            InitializeComponent();
        }

        public override bool ValidateData()
        {
            return this.ValidateControls(this.txt_FileName);
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.txt_FileName.Focus();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Height = this.txt_FileName.Height + 2;
        }
        private void txt_FileName_DoubleClick(object sender, EventArgs e)
        {
            this.bt_Browse_Click(sender, e);
        }

        private void bt_Browse_Click(object sender, EventArgs e)
        {
            using (var openDialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                DefaultExt = this.DefaultExt,
                Filter = this.Filter
            })
            {
                if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _SelectChangeFired = false;
                    this.txt_FileName.Text = openDialog.FileName;
                    this.OnSelectChange(e);
                }
            }
        }

        private void txt_FileName_Leave(object sender, EventArgs e)
        {
            this.OnSelectChange(e);
        }

        private void txt_FileName_TextChanged(object sender, EventArgs e)
        {
            _SelectChangeFired = false;
        }
    }
}
