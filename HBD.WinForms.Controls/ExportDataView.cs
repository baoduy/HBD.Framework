using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.WinForms.Controls.Core;
using HBD.Framework.Core;
using HBD.Framework.Data.XML;
using HBD.Framework.Data.Excel;
using HBD.Framework.Data;
using HBD.Framework.Data.CSV;
using System.Collections.Specialized;
using HBD.Framework.Extension.WinForms;

namespace HBD.WinForms.Controls
{
    [DefaultEvent("SelectChange")]
    public partial class ExportDataView : HBDViewBase
    {
        public ExportDataView()
        {
            InitializeComponent();
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

        public override void CreateChildrenControl()
        {
            base.CreateChildrenControl();
            this.viewDataControl1.CreateChildrenControl();
        }

        public override void LoadControlData()
        {
            base.LoadControlData();

            if (this.ts_Source.Items.Count == 0)
            {
                this.ts_Source.Items.AddRange(GetBrowserNames());
                this.ts_Source.SelectedIndex = 0;
            }

            this.viewDataControl1.LoadControlData();
        }

        private void ts_Source_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ts_Source.SelectedItem == null)
                return;
            var type = ((HBDKeyValuePair<string, object>)this.ts_Source.SelectedItem).Value;
            this.viewDataControl1.OpenBrowserType = (OpenBrowserType)type;
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.Enabled = false;

            if (e.ClickedItem == this.ts_ToExcel
                || e.ClickedItem == this.ts_CSV
                || e.ClickedItem == this.ts_ToXML)
            {
                this.ExportData(e.ClickedItem);
            }

            this.Enabled = true;
        }

        private string GetFilter(ToolStripItem item)
        {
            if (item == this.ts_ToExcel)
                return "Excel|*.xml";
            if (item == this.ts_ToXML)
                return "XML|*.xml";
            return "CSV|*.csv";
        }

        private FileDataConverterBase GetFileDataConverterBase(ToolStripItem item, string fileName)
        {
            if (item == this.ts_ToExcel)
                return new ExcelAdapter(fileName);
            if (item == this.ts_ToXML)
                return new XMLAdapter(fileName);
            return new CSVAdapter(fileName);
        }

        public void ExportData(ToolStripItem item)
        {
            var data = this.viewDataControl1.GetDataTable();
            if (data == null || data.Rows.Count == 0)
                MessageBox.Show("There is no data to export", "Export Data To File", MessageBoxButtons.OK, MessageBoxIcon.Error);

            using (var saveDialog = new SaveFileDialog() { FileName = this.viewDataControl1.SourceName, Filter = GetFilter(item) })
            {
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var adapter = GetFileDataConverterBase(item, saveDialog.FileName))
                    {
                        adapter.WriteFile(data);
                    }

                    MessageBox.Show(string.Format("The {0} had been exported to file {1}",
                        this.viewDataControl1.SourceName, saveDialog.FileName), "Export Data To File", MessageBoxButtons.OK);
                }
            }
        }

        #region IHBDViewBase

        protected override IList<ControlStates.ControlState> GetListControlState()
        {
            var listControlState = base.GetListControlState();

            listControlState.Add(this.GetControlState(this.ts_Source));
            listControlState.Add(this.GetControlState(viewDataControl1));

            return listControlState;
        }
        #endregion
    }
}
