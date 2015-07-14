using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using HBD.Framework.Extension;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using HBD.WinForms.Controls.Comparison.Events;
using HBD.Framework.Data.Comparison;
using HBD.WinForms.Controls.Comparison.Helpers;
using HBD.WinForms.Controls.Comparison.Core;
using HBD.WinForms.Controls.Core;
using HBD.Framework.Core;

namespace HBD.WinForms.Controls.Comparison
{
    public partial class ComparisonView : HBDViewBase, IMultiBrowserComparisonControl
    {
        public ComparisonView()
        {
            InitializeComponent();
        }

        DataExporter _dataExporter = null;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataExporter DataExporter
        {
            get
            {
                if (this._dataExporter == null)
                    this._dataExporter = new DataExporter();
                return _dataExporter;
            }
        }

        CompareInfo _currentCompareTable;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CompareInfo CurrentCompareTable
        {
            get { return _currentCompareTable; }
        }

        private CompareResult _currentResult = null;

        protected virtual void Compare(DataTable tableA, DataTable tableB, FieldComparison primaryField, FieldComparisonCollection comparisonFields)
        {
            if ( comparisonFields.Count <= 0 )
                throw new ArgumentNullException();

            var sortFiled = primaryField != null && !primaryField.IsEmpty() ? primaryField : comparisonFields[0];

            this._currentCompareTable = new CompareInfo()
            {
                TableA = tableA.Sort(SortDirection.Ascending, sortFiled.FieldA),
                TableB = tableB.Sort(SortDirection.Ascending, sortFiled.FieldB),
                PrimaryField = primaryField,
                CompareFields = comparisonFields,
            };

            this.ts_progressbar.Visible = true;
            var worker = new BackgroundWorker() { WorkerReportsProgress = true };
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerAsync();

        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            this._currentResult = DataCompareManager.Compare(this._currentCompareTable, sender as BackgroundWorker);
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.lb_Status.Text = e.UserState as string;
            if ( e.ProgressPercentage > 100 )
                this.ts_progressbar.Value = 100;
            else this.ts_progressbar.Value = e.ProgressPercentage;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ShowResult(this._currentResult);
            this.ts_progressbar.Visible = false;
        }

        protected virtual void ShowResult(CompareResult result)
        {
            this.tabControl1.SelectedTab = this.tabResult;
            this.comparisionDataGrids1.DataSource = result;
            this.btCompare.Enabled = true;
            this.DisableWithWaitCursor(false);
        }

        private void btCompare_Click(object sender, EventArgs e)
        {
            this.DisableWithWaitCursor(true);
            this.btCompare.Enabled = false;

            this.Compare(this.multiComparisonBrowser.TableA,
                this.multiComparisonBrowser.TableB,
                this.listColumnComparision.PrimaryKey,
                this.listColumnComparision.CompareFields);
        }

        private void btExportToXLS_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "*.xlsx";
            saveFile.Filter = "Excel|*.xlsx";
            saveFile.FileName = string.Format("Export_{0:yyyyMMdd}", DateTime.Now);

            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (System.IO.File.Exists(saveFile.FileName))
                    System.IO.File.Delete(saveFile.FileName);

                this.DataExporter.ExportToXLS(this._currentResult, saveFile.FileName);

                if (MessageBox.Show("Export Sucessful. Do you want open it?", "Export to Excel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    Process.Start(saveFile.FileName);
            }
        }

        private void MultiCompareBrowser_SelectionChanged(object sender, FileSelectedEventArgs e)
        {
            if (e.FileType == SelectedFileType.FileA)
            {
                if (string.IsNullOrEmpty(this.multiComparisonBrowser.OriginalSourceA))
                    return;

                this.listColumnComparision.TitleA = this.multiComparisonBrowser.TableA.TableName;
                this.listColumnComparision.DataSourceA = this.multiComparisonBrowser.TableA.Columns;
            }
            else
            {
                if (string.IsNullOrEmpty(this.multiComparisonBrowser.OriginalSourceB))
                    return;

                this.listColumnComparision.TitleB = this.multiComparisonBrowser.TableB.TableName;
                this.listColumnComparision.DataSourceB = this.multiComparisonBrowser.TableB.Columns;
            }

            this.listColumnComparision.ClearControl();
            this.btCompare.Enabled = this.listColumnComparision.DataSourceA != null && this.listColumnComparision.DataSourceB != null;
        }

        List<object> _list = null;
        private object[] GetBrowserNames()
        {
            if (_list != null)
                return _list.ToArray();

            _list = new List<object>();
            _list.Add(new HBDKeyValuePair<string, object>("Excel", OpenBrowserType.ExcelOpenBrowser));
            _list.Add(new HBDKeyValuePair<string, object>("CSV", OpenBrowserType.CSVOpenBrowser));
            _list.Add(new HBDKeyValuePair<string, object>("XML", OpenBrowserType.XMLOpenBrowser));

            return _list.ToArray();
        }

        private void ts_Source_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ts_Source.SelectedItem == null)
                return;
            var type = ((HBDKeyValuePair<string, object>)this.ts_Source.SelectedItem).Value;
            this.OpenBrowserAType = (OpenBrowserType)type;
        }

        private void ts_Destination_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ts_Destination.SelectedItem == null)
                return;
            var type = ((HBDKeyValuePair<string, object>)this.ts_Destination.SelectedItem).Value;
            this.OpenBrowserBType = (OpenBrowserType)type;
        }

        private void ts_AutoLoadColumns_Click(object sender, EventArgs e)
        {
            this.DisableWithWaitCursor(true);

            if (this.listColumnComparision.DataSourceA == null
                || this.listColumnComparision.DataSourceB == null)
                return;

            this.ts_AutoLoadColumns.Enabled = false;
            this.btCompare.Enabled = false;
            this.listColumnComparision.SuspendLayout();
            this.SuspendLayout();

            this.listColumnComparision.ClearControl();
            this.listColumnComparision.PopulateComparisonByColumnNames();

            this.listColumnComparision.ResumeLayout(false);
            this.ResumeLayout(false);
            this.ts_AutoLoadColumns.Enabled = true;
            this.btCompare.Enabled = true;
            this.DisableWithWaitCursor(false);
        }

        public override void CreateChildrenControl()
        {
            base.CreateChildrenControl();

            if (this.ts_Source.Items.Count == 0)
            {
                this.ts_Source.Items.AddRange(this.GetBrowserNames());
                this.ts_Source.SelectedIndex = 0;
            }

            if (this.ts_Destination.Items.Count == 0)
            {
                this.ts_Destination.Items.AddRange(this.GetBrowserNames());
                this.ts_Destination.SelectedIndex = 0;
            }
        }

        public override void LoadControlData()
        {
            base.LoadControlData();
            this.multiComparisonBrowser.LoadControlData();
            this.listColumnComparision.LoadControlData();
        }

        #region IMultiBrowserComparisonControl
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(OpenBrowserType.ExcelOpenBrowser)]
        public OpenBrowserType OpenBrowserAType
        {
            get { return this.multiComparisonBrowser.OpenBrowserAType; }
            set { this.multiComparisonBrowser.OpenBrowserAType = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(OpenBrowserType.ExcelOpenBrowser)]
        public OpenBrowserType OpenBrowserBType
        {
            get { return this.multiComparisonBrowser.OpenBrowserBType; }
            set { this.multiComparisonBrowser.OpenBrowserBType = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Type CustomBrowserControlAType
        {
            get { return this.multiComparisonBrowser.CustomBrowserControlAType; }
            set { this.multiComparisonBrowser.CustomBrowserControlAType = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Type CustomBrowserControlBType
        {
            get { return this.multiComparisonBrowser.CustomBrowserControlBType; }
            set { this.multiComparisonBrowser.CustomBrowserControlBType = value; }
        }
        #endregion

        #region IHBDViewBase
        #endregion

        protected override IList<ControlStates.ControlState> GetListControlState()
        {
            var list = base.GetListControlState();

            list.Add(this.GetControlState(this.ts_Source));
            list.Add(this.GetControlState(this.ts_Destination));
            list.Add(this.GetControlState(this.multiComparisonBrowser));
            list.Add(this.GetControlState(this.listColumnComparision));

            return list;
        }
    }
}
