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

namespace HBD.WinForms.Controls
{
    [DefaultProperty("SourcePath"), DefaultEvent("SelectChange")]
    public partial class ExcelOpenBrowser : HBDControl, IOpenBrowserConvertableControl
    {
        public ExcelOpenBrowser()
        {
            InitializeComponent();
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue("Browse")]
        public string BrowseText
        {
            get { return this.fileOpenBrowser.BrowseText; }
            set { this.fileOpenBrowser.BrowseText = value; }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue("File")]
        public string Title
        {
            get { return this.fileOpenBrowser.Title; }
            set { this.fileOpenBrowser.Title = value; }
        }

        #region IOpenBrowserConvertable

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public string SourcePath
        {
            get { return this.fileOpenBrowser.SourcePath; }
            set { this.fileOpenBrowser.SourcePath = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public string SourceName
        {
            get { return this.fileOpenBrowser.SourceName; }
            set { this.fileOpenBrowser.SourceName = value; }
        }

        public event EventHandler SelectChange = null;
        protected void OnSelectChange(EventArgs e)
        {
            if (this.SelectChange != null)
                this.SelectChange(this, e);
        }

        public DataTable GetDataTable()
        {
            var path = this.fileOpenBrowser.SourcePath;
            Guard.PathExisted(path);

            this.Enabled = false;
            using (var adapter = new ExcelAdapter(path))
            {
                var data = adapter.ToDataTable();

                this.Enabled = true;
                return data;
            }
        }

        #endregion

        public override bool ValidateData()
        {
            if (!this.fileOpenBrowser.ValidateData())
                return false;

            return this.ValidateControls(this.cb_Sheets);
        }

        public override void LoadControlData()
        {
            base.LoadControlData();
            //Validate Control without show error messages.
            if (this.fileOpenBrowser.ValidateData(false))
                this.cb_Sheets.Text = this.SourceName;
        }

        private void fileOpenBrowser1_SelectChange(object sender, EventArgs e)
        {
            this.cb_Sheets.Items.Clear();

            if (this.fileOpenBrowser.ValidateData())
            {
                using (var adapter = new ExcelAdapter(this.SourcePath))
                {
                    this.cb_Sheets.Items.AddRange(adapter.SheetNames);

                    if (adapter.SheetNames.Contains(this.SourceName))
                        this.cb_Sheets.Text = this.SourceName;
                }
            }
        }

        private void cb_Sheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SourceName = this.cb_Sheets.Text;
            this.OnSelectChange(e);
        }
    }
}
