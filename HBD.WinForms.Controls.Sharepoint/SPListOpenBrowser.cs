using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Data.Sharepoint;
using HBD.WinForms.Controls.Core;
using HBD.WinForms.Controls.Attributes;
using HBD.Framework.Core;
using HBD.Framework.Extension.WinForms;
using HBD.Framework.Log;

namespace HBD.WinForms.Controls.Sharepoint
{
    [DefaultProperty("SourcePath"), DefaultEvent("SelectChange")]
    public partial class SPListOpenBrowser : HBDControl, IOpenBrowserConvertableControl
    {
        const string _none = "[None]";

        public SPListOpenBrowser()
        {
            InitializeComponent();
        }

        #region IOpenBrowserConvertable
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        [ControlPropertyState]
        public string SourcePath
        {
            get { return this.txt_URL.Text; }
            set { this.txt_URL.Text = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        [ControlPropertyState]
        public string SourceName { get; set; }

        string _viewTitle;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        [ControlPropertyState]
        public string ViewTitle
        {
            get { return _viewTitle; }
            set { _viewTitle = value; }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(false)]
        [ControlPropertyState]
        public bool ShowSPViewOption
        {
            get { return this.cb_Views.Visible; }
            set
            {
                this.cb_Views.Visible = value;
                this.lbView.Visible = value;
            }
        }

        volatile ISPAdapter _sPAdapter = null;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public virtual ISPAdapter SPAdapter
        {
            get
            {
                if (_sPAdapter == null && !string.IsNullOrEmpty(this.SourcePath))
                    _sPAdapter = SPAdapterManager.Open(this.SourcePath);
                return _sPAdapter;
            }
        }

        public DataTable GetDataTable()
        {
            if (!this.ValidateData())
                return null;

            this.DisableWithWaitCursor(true);

            var view = this.ViewTitle;
            if (view == _none)
                view = string.Empty;

            try
            {
                //Create the new adpter to get data as the SPList cannot work on multi thread.
                //using (var adapter = new SPServerAdapter(this.SourcePath))
                //{
                return this.SPAdapter.ToDataTable(this.SourceName, view);
                //}
            }
            catch (Exception ex)
            { LogManager.Write(ex); }
            finally
            { this.DisableWithWaitCursor(false); }

            return null;
        }
        #endregion

        private bool _selectChangeFired = false;
        public event EventHandler SelectChange = null;
        protected void OnSelectChange(EventArgs e)
        {
            if (this.SelectChange != null && !_selectChangeFired)
                this.SelectChange(this, e);
            _selectChangeFired = true;
        }

        public override bool ValidateData()
        {
            if (this.ShowSPViewOption)
                return this.ValidateControls(this.txt_URL, this.cb_ListNames, this.cb_Views);
            else return this.ValidateControls(this.txt_URL, this.cb_ListNames);
        }

        public override void LoadControlData()
        {
            base.LoadControlData();
            if (this.cb_ListNames.Items.Count == 0)
                this.LoadListNames();
        }

        private void bt_Browse_Click(object sender, EventArgs e)
        {
            _selectChangeFired = false;
            using (var openDialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                DefaultExt = "*.xlsx",
                Filter = "Excel|*.xlsx|Excel 97-2003|*.xls"
            })
            {
                if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.txt_URL.Text = openDialog.FileName;
                }
            }
        }

        private void txt_URL_TextChanged(object sender, EventArgs e)
        {
            //Reset SPAdapter as URL changed
            this.DisposeAdapter();
        }

        private void txt_URL_Leave(object sender, EventArgs e)
        {
            this.LoadListNames();
        }

        private void LoadListNames()
        {
            if (this._selectChangeFired)
                return;

            this.cb_ListNames.Items.Clear();
            if (string.IsNullOrEmpty(this.SourcePath))
                return;
            try
            {
                this.DisableWithWaitCursor(true);

                this.SafeThreadExecute<string[]>(
                        () => { return this.SPAdapter.ListTitles; },
                        (listNames) =>
                        {
                            if (listNames == null)
                            {
                                this.SetError(this.txt_URL, "The URL is not found.");
                                this.DisableWithWaitCursor(false);
                                return;
                            }

                            this.cb_ListNames.Items.AddRange(listNames);

                            if (!string.IsNullOrEmpty(this.SourceName)
                                && this.cb_ListNames.Items.Contains(this.SourceName))
                            {
                                this.cb_ListNames.Text = this.SourceName;
                            }
                            else this.DisableWithWaitCursor(false);

                            if (!this.ShowSPViewOption)
                                this.DisableWithWaitCursor(false);
                        });
            }
            catch (Exception ex)
            {
                this.SetError(this.txt_URL, ex.Message);
            }
        }

        private void cb_ListNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SourceName = this.cb_ListNames.Text;

            if (!this.ShowSPViewOption)
            {
                _selectChangeFired = false;
                this.OnSelectChange(e);
            }
            else LoadViewName();
        }

        private void LoadViewName()
        {
            this.cb_Views.Items.Clear();
            if (string.IsNullOrEmpty(this.SourceName))
                return;
            try
            {
                this.DisableWithWaitCursor(true);

                this.SafeThreadExecute<string[]>(
                        () => { return this.SPAdapter.GetViewTitles(this.SourceName); },
                        (views) =>
                        {
                            if (views == null)
                            {
                                this.SetError(this.cb_ListNames, "The SPList is not found.");
                                this.DisableWithWaitCursor(false);
                                return;
                            }

                            this.cb_Views.Items.Add(_none);
                            this.cb_Views.Items.AddRange(views);

                            if (!string.IsNullOrEmpty(this.ViewTitle)
                                && this.cb_Views.Items.Contains(this.ViewTitle))
                            {
                                this.cb_Views.Text = this.ViewTitle;
                            }

                            this.DisableWithWaitCursor(false);
                        });
            }
            catch (Exception ex)
            {
                this.SetError(this.txt_URL, ex.Message);
            }
        }

        private void cb_Views_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ViewTitle = this.cb_Views.Text;

            _selectChangeFired = false;
            this.OnSelectChange(e);
        }

        protected virtual void DisposeAdapter()
        {
            if (_sPAdapter != null)
            {
                this._sPAdapter.Dispose();
                this._sPAdapter = null;
            }
        }
    }
}
