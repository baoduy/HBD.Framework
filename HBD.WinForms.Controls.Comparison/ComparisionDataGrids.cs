using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Data.Comparison;
using HBD.WinForms.Controls.Comparison.Core;
using HBD.WinForms.Controls.Core;

namespace HBD.WinForms.Controls.Comparison
{
    public partial class ComparisionDataGrids : HBDControl, IDoubleDataGrids
    {
        //private IList<int> FormatedCell { get; set; }

        CompareResult _dataSource;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public CompareResult DataSource
        {
            get { return _dataSource; }
            set
            {
                _dataSource = value;
                this.OnDataSourceChanged(EventArgs.Empty);
            }
        }

        public ComparisionDataGrids()
        {
            InitializeComponent();
        }

        #region IDoubleDataGrids
        public void BeginInit()
        {
            this.doubleDataGrids.BeginInit();
        }

        public void ClearSelection()
        {
            this.doubleDataGrids.ClearSelection();
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public HBDDataGridView DataGridA
        {
            get { return this.doubleDataGrids.DataGridA; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public HBDDataGridView DataGridB
        {
            get { return this.doubleDataGrids.DataGridB; }
        }

        public void EndInit()
        {
            this.doubleDataGrids.EndInit();
        }

        public void HideAllColumns()
        {
            this.doubleDataGrids.HideAllColumns();
        }

        public void HideAllRows()
        {
            this.doubleDataGrids.HideAllRows();
        }

        public void HidEmptyFormatRows()
        {
            this.doubleDataGrids.HidEmptyFormatRows();
        }

        public void ResumeBinding()
        {
            this.doubleDataGrids.ResumeBinding();
        }

        public void ShowAllColumns()
        {
            this.doubleDataGrids.ShowAllColumns();
        }

        public void ShowAllRows()
        {
            this.doubleDataGrids.ShowAllRows();
        }

        public void SortColumnHeaders()
        {
            this.doubleDataGrids.SortColumnHeaders();
        }

        public void SortColumnHeaders(FieldComparison excludeColumn)
        {
            this.doubleDataGrids.SortColumnHeaders(excludeColumn);
        }

        public void SortColumnHeadersByIndexOfFields(FieldComparisonCollection fields)
        {
            this.doubleDataGrids.SortColumnHeadersByIndexOfFields(fields);
        }

        public void SortRowsBy(FieldComparison field, ListSortDirection direction)
        {
            this.doubleDataGrids.SortRowsBy(field, direction);
        }

        public void SuspendBinding()
        {
            this.doubleDataGrids.SuspendBinding();
        }

        public event EventHandler<DataGridSelectionChangedEventArgs> SelectionChanged;

        private void doubleDataGrids_SelectionChanged(object sender, DataGridSelectionChangedEventArgs e)
        {
            if (this.SelectionChanged != null)
                this.SelectionChanged(this, e);
        }
        #endregion

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(true)]
        public bool OptionVisibled
        {
            get { return this.grOption.Visible; }
            set
            {
                this.grOption.Visible = value;
                this.tableLayoutPanel1.Visible = value;
            }
        }

        private void ChangeBackgroundColors(CompareResult result)
        {
            if (this.rd_ShowComparedData.Checked)
            {
                foreach (DifferenceCell diff in result.DifferenceCells)
                {
                    this.DataGridA.Rows[diff.RowIndex].Cells[diff.ColumnA].Style.BackColor = Constant.DifferenceColor;
                    this.DataGridB.Rows[diff.RowIndex].Cells[diff.ColumnB].Style.BackColor = Constant.DifferenceColor;
                }
            }

            foreach (int rowAIndex in result.TableANotFoundRowsIndexs)
            {
                this.DataGridA.Rows[rowAIndex].DefaultCellStyle.BackColor = Constant.NotFoundAColor;
                this.DataGridB.Rows[rowAIndex].DefaultCellStyle.BackColor = Constant.NotFoundAColor;
            }

            foreach (int rowBIndex in result.TableBNotFoundRowsIndexs)
            {
                this.DataGridB.Rows[rowBIndex].DefaultCellStyle.BackColor = Constant.NotFoundBColor;
                this.DataGridA.Rows[rowBIndex].DefaultCellStyle.BackColor = Constant.NotFoundBColor;
            }
        }

        public event EventHandler DataSourceChanged;
        protected virtual void OnDataSourceChanged(EventArgs e)
        {
            this.DisableWithWaitCursor(true);
            this.doubleDataGrids.SuspendLayout();
            this.SuspendLayout();

            if (this.DataSource != null)
            {
                this.lb_A.Text = this.DataSource.TableA.TableName;
                this.lb_B.Text = this.DataSource.TableB.TableName;

                if (!this.OptionVisibled)
                {
                    this.doubleDataGrids.DataSource = this.DataSource;
                    return;
                }

                this.grOption.Enabled = true;

                //Show Original data
                if (this.rd_ShowOriginal.Checked)
                    this.doubleDataGrids.DataSource = this.DataSource.OriginalTables;
                //Show Differenct Data
                else if (this.rd_ShowComparedData.Checked)
                {
                    this.doubleDataGrids.DataSource = this.DataSource;
                    this.doubleDataGrids.ShowColumnsByFields(this.DataSource.CompareInfo.CompareFields);
                    this.doubleDataGrids.SortColumnHeadersByIndexOfFields(this.DataSource.CompareInfo.CompareFields);
                    this.ChangeBackgroundColors(this.DataSource);

                }
                else if (this.rd_ShowNotFound.Checked)
                {
                    var result = this.DataSource.GetNotFoundRowsOnly();
                    this.doubleDataGrids.DataSource = result;
                    this.doubleDataGrids.ShowColumnsByFields(result.CompareInfo.CompareFields);
                    this.doubleDataGrids.SortColumnHeadersByIndexOfFields(result.CompareInfo.CompareFields);
                    this.ChangeBackgroundColors(result);
                }
                else //Show Identical Data
                {
                    this.doubleDataGrids.DataSource = this.DataSource.IdenticalTables;
                    this.doubleDataGrids.ShowColumnsByFields(this.DataSource.CompareInfo.CompareFields);
                    this.doubleDataGrids.SortColumnHeadersByIndexOfFields(this.DataSource.CompareInfo.CompareFields);
                }
            }

            this.ResumeLayout(false);
            this.doubleDataGrids.ResumeLayout(false);
            this.DisableWithWaitCursor(false);

            if (this.DataSourceChanged != null)
                this.DataSourceChanged(this, e);
        }

        private void rd_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                this.OnDataSourceChanged(e);
        }
    }
}
