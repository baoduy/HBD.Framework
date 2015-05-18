using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Data.Comparison;
using HBD.Framework.Extension.WinForms;
using HBD.WinForms.Controls.Comparison.Core;

namespace HBD.WinForms.Controls.Comparison
{
    public partial class DoubleDataGrids : UserControl, IDoubleDataGrids
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HBDDataGridView DataGridA
        {
            get { return this.dataGridA; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HBDDataGridView DataGridB
        {
            get { return this.dataGridB; }
        }

        CompareInfoBase _dataSource = null;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CompareInfoBase DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                if (value == null)
                    return;

                if (_dataSource != value)
                {
                    _dataSource = value;

                    this.DataGridA.DataSource = null;
                    this.DataGridB.DataSource = null;

                    this.DataGridA.Name = value.TableA.TableName;
                    this.DataGridA.DataSource = value.TableA;

                    this.DataGridB.Name = value.TableB.TableName;
                    this.DataGridB.DataSource = value.TableB;


                    if (this.DataGridA.Name == this.DataGridB.Name)
                    {
                        this.DataGridA.Name += "_A";
                        this.DataGridB.Name += "_B";
                    }
                }
            }
        }

        public DoubleDataGrids()
        {
            InitializeComponent();
        }

        public void ClearSelection()
        {
            this.dataGridA.ClearSelection();
            this.dataGridB.ClearSelection();
        }

        public void ShowAllColumns()
        {
            this.dataGridA.ShowAllColumns();
            this.dataGridB.ShowAllColumns();
        }

        public void HideAllColumns()
        {
            this.dataGridA.HideAllColumns();
            this.dataGridB.HideAllColumns();
        }

        public void ShowColumnsByFields(FieldComparisonCollection fields)
        {
            this.HideAllColumns();
            foreach (var f in fields)
            {
                this.ShowColumnsByField(f);
            }
        }

        private void ShowColumnsByField(FieldComparison f)
        {
            if (this.dataGridA.Columns.Contains(f.FieldA))
                this.dataGridA.Columns[f.FieldA].Visible = true;
            if (this.dataGridB.Columns.Contains(f.FieldB))
                this.dataGridB.Columns[f.FieldB].Visible = true;
        }
        public void ShowAllRows()
        {
            this.ClearSelection();
            for (int i = 0; i < this.DataGridA.RowCount; i++)
            {
                this.DataGridA.Rows[i].Visible = true;
                this.DataGridB.Rows[i].Visible = true;
            }
        }

        public void BeginInit()
        {
            ((System.ComponentModel.ISupportInitialize)(this.dataGridA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridB)).BeginInit();
        }

        public void EndInit()
        {
            ((System.ComponentModel.ISupportInitialize)(this.dataGridA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridB)).EndInit();
        }

        public void SuspendBinding()
        {
            ((CurrencyManager)BindingContext[this.DataGridA.DataSource]).SuspendBinding();
            ((CurrencyManager)BindingContext[this.DataGridB.DataSource]).SuspendBinding();
        }

        public void ResumeBinding()
        {
            ((CurrencyManager)BindingContext[this.DataGridA.DataSource]).ResumeBinding();
            ((CurrencyManager)BindingContext[this.DataGridB.DataSource]).ResumeBinding();
        }

        public void HideAllRows()
        {
            this.dataGridA.HideAllRows();
            this.dataGridB.HideAllRows();
        }

        public void HidEmptyFormatRows()
        {
            this.DataGridA.HideEmptyFormatRows();
            this.DataGridB.HideEmptyFormatRows();
        }

        public void SortColumnHeaders()
        {
            this.dataGridA.SortColumnHeaders(string.Empty);
            this.dataGridB.SortColumnHeaders(string.Empty);
        }

        public void SortColumnHeaders(FieldComparison excludeColumn)
        {
            if (excludeColumn != null)
            {
                this.dataGridA.SortColumnHeaders(excludeColumn.FieldA);
                this.dataGridB.SortColumnHeaders(excludeColumn.FieldB);
            }
            else this.SortColumnHeaders();
        }

        public void SortColumnHeadersByIndexOfFields(FieldComparisonCollection fields)
        {
            this.dataGridA.SortColumnHeadersByIndexOfFields(fields.GetAllFieldA());
            this.dataGridB.SortColumnHeadersByIndexOfFields(fields.GetAllFieldB());
        }

        public void SortRowsBy(FieldComparison field, ListSortDirection direction)
        {
            if (this.DataGridA.Columns.Contains(field.FieldA))
            {
                var col = this.DataGridA.Columns[field.FieldA];
                this.DataGridA.Sort(col, direction);
            }

            if (this.DataGridB.Columns.Contains(field.FieldB))
            {
                var col = this.DataGridB.Columns[field.FieldB];
                this.DataGridB.Sort(col, direction);
            }
        }

        public override void Refresh()
        {
            this.dataGridA.Refresh();
            this.dataGridB.Refresh();

            base.Refresh();
        }

        private void dataGrid_Scroll(object sender, ScrollEventArgs e)
        {
            if (!this.dataGridA.Focused && !this.dataGridB.Focused)
                ((HBDDataGridView)sender).Focus();

            if (sender == this.dataGridA)
            {
                if (this.DataGridA.Focused)
                    this.DataGridA.SyncScrolledPositionTo(this.DataGridB);
            }
            else if (this.DataGridB.Focused)
                this.DataGridB.SyncScrolledPositionTo(this.DataGridA);
        }

        private void dataGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (sender == this.dataGridA)
            {
                if (this.DataGridA.Focused)
                    this.DataGridA.SyncSelectedTo(this.DataGridB);
            }
            else if (this.DataGridB.Focused)
                this.DataGridB.SyncSelectedTo(this.DataGridA);

            this.OnSelectionChanged(new DataGridSelectionChangedEventArgs(sender as DataGridView));
        }

        public event EventHandler<DataGridSelectionChangedEventArgs> SelectionChanged;
        protected virtual void OnSelectionChanged(DataGridSelectionChangedEventArgs e)
        {
            if (this.SelectionChanged != null)
                this.SelectionChanged(this, e);
        }

        private void dataGrid_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.Programmatic;
        }

        private void dataGridA_SelectionModeChanged(object sender, EventArgs e)
        {
            if (sender == this.dataGridA && this.DataGridA.Focused)
            {
                this.DataGridB.SelectionMode = this.DataGridA.SelectionMode;
            }
            else if (this.DataGridB.Focused)
            {
                this.DataGridA.SelectionMode = this.DataGridB.SelectionMode;
            }
        }
    }
}
