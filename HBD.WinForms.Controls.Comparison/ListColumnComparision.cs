using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Data.Comparison;
using HBD.Framework.Data;
using HBD.WinForms.Controls.Core;
using HBD.WinForms.Controls.Attributes;

namespace HBD.WinForms.Controls.Comparison
{
    public partial class ListColumnComparision : HBDControl
    {
        ColumnNamesCollection dataSourceA;
        ColumnNamesCollection dataSourceB;

        public ListColumnComparision()
        {
            InitializeComponent();
            this.CompareRowType = CompareRowType.PrimaryKey;
            this.CompareColumnType = CompareColumnType.ColumnSpecification;
        }

        [DefaultValue(null), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColumnNamesCollection DataSourceA
        {
            get { return this.dataSourceA; }
            set
            {
                this.dataSourceA = value;
                this.OnDataSourceChanged(EventArgs.Empty);
            }
        }

        [DefaultValue(null), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColumnNamesCollection DataSourceB
        {
            get { return this.dataSourceB; }
            set
            {
                this.dataSourceB = value;
                this.OnDataSourceChanged(EventArgs.Empty);
            }
        }

        [DefaultValue(null), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ControlPropertyState]
        public FieldComparison PrimaryKey
        {
            get { return this.primaryKeyColumn.SelectedField; }
            set { this.primaryKeyColumn.SelectedField = value; }
        }

        FieldComparisonCollection _compareFields;
        [DefaultValue(null), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ControlPropertyState]
        public FieldComparisonCollection CompareFields
        {
            get
            {
                var columns = this.columnCollection.SelectedFields;

                //if (this.DataSourceA == null || this.DataSourceB == null)
                //    return columns;

                //if (columns.Count <= 0)
                //{
                //    if (this._compareFields == null)
                //    {
                //        this._compareFields = new FieldComparisonCollection();

                //        int columnCount = this.DataSourceA.Count > this.DataSourceB.Count ? this.DataSourceB.Count : this.DataSourceA.Count;
                //        for (int i = 0; i < columnCount; i++)
                //        {
                //            this._compareFields.Add(new FieldComparison(this.DataSourceA[i].ColumnName, this.DataSourceB[i].ColumnName));
                //        }
                //    }

                //    columns = this._compareFields;
                //}

                return columns;
            }
            set { this.columnCollection.SelectedFields = value; }
        }

        [DefaultValue(null), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [ControlPropertyState]
        public string TitleA
        {
            get { return this.lbSheetA.Text; }
            set
            {
                this.lbSheetA.Text = value;
                this.columnCollection.TitleA = value;
            }
        }

        [DefaultValue(null), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [ControlPropertyState]
        public string TitleB
        {
            get { return this.lbSheetB.Text; }
            set
            {
                this.lbSheetB.Text = value;
                this.columnCollection.TitleB = value;
            }
        }

        [DefaultValue(CompareRowType.PrimaryKey), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [ControlPropertyState]
        public CompareRowType CompareRowType { get; set; }

        [DefaultValue(CompareColumnType.ColumnSpecification), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [ControlPropertyState]
        public CompareColumnType CompareColumnType { get; set; }

        public event EventHandler DataSourceChanged;
        protected virtual void OnDataSourceChanged(EventArgs e)
        {
            #region Set Column
            this.primaryKeyColumn.SelectedField = this.PrimaryKey;

            this.primaryKeyColumn.DataSourceA = this.DataSourceA;
            this.primaryKeyColumn.DataSourceB = this.DataSourceB;

            this.columnCollection.DataSourceA = this.DataSourceA;
            this.columnCollection.DataSourceB = this.DataSourceB;
            #endregion

            if (this.DataSourceChanged != null)
                this.DataSourceChanged(this, e);
        }

        protected override void SuspendLayout()
        {
            if (this.columnCollection != null)
                this.columnCollection.SuspendLayout();
            base.SuspendLayout();
        }

        public override void ResumeLayout()
        {
            base.ResumeLayout();
            if (this.columnCollection != null)
                this.columnCollection.ResumeLayout();
        }

        public override void ResumeLayout(bool performLayout)
        {
            base.ResumeLayout(performLayout);
            if (this.columnCollection != null)
                this.columnCollection.ResumeLayout(performLayout);
        }

        /// <summary>
        /// Add compare column
        /// </summary>
        /// <param name="columnA"></param>
        /// <param name="columnB"></param>
        public void AddControl(FieldComparison field = null)
        {
            this.columnCollection.Add(field);
        }

        public void PopulateComparisonByColumnNames()
        {
            if (this.DataSourceA.Count <= 0
                || this.DataSourceB.Count <= 0)
                return;

            this.SuspendLayout();
            this.ClearControl();
            this.columnCollection.Visible = false;

            this._compareFields = FieldComparisonCollection.AutoPopulateByColumnNames(this.DataSourceA, this.DataSourceB, this.PrimaryKey);
            foreach (var f in this._compareFields)
                this.AddControl(f);

            this.columnCollection.Visible = true;
            this.ResumeLayout(false);
        }

        /// <summary>
        /// Clear all compare columns
        /// </summary>
        public void ClearControl()
        {
            this.columnCollection.Clear();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            this.lbSheetA.Width = this.primaryKeyColumn.Width / 2;
        }

        public override void CreateChildrenControl()
        {
            base.CreateChildrenControl();

            this.OnDataSourceChanged(EventArgs.Empty);

            if (this.CompareColumnType == Comparison.CompareColumnType.ColumnSpecification)
                this.ch_SpecifyColumn.Checked = true;
            else ch_ColumnByColumn.Checked = true;

            if (this.CompareRowType == Comparison.CompareRowType.PrimaryKey)
                this.ch_PrimaryKey.Checked = true;
            else this.ch_RowByRow.Checked = true;
        }

        public override void LoadControlData()
        {
            base.LoadControlData();
            this.primaryKeyColumn.LoadControlData();
            this.columnCollection.LoadControlData();
        }

        private void ch_SpecifyColumn_CheckedChanged(object sender, EventArgs e)
        {
            this.columnCollection.EnableChildControls(this.ch_SpecifyColumn.Checked);
            this.columnCollection.BringToFront();
        }

        private void dataPrimaryKey_CheckedChanged(object sender, EventArgs e)
        {
            this.grPrimary.Enabled = this.ch_PrimaryKey.Checked;
        }

        private void columnCollection_ColumnAdded(object sender, Events.CompareFieldEventArgs e)
        {
            e.CompareColumnControl.Enabled = this.ch_SpecifyColumn.Checked;
        }
    }
}
