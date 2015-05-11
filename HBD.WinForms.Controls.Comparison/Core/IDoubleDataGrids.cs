using System;
namespace HBD.WinForms.Controls.Comparison.Core
{
    interface IDoubleDataGrids
    {
        void BeginInit();
        void ClearSelection();
        HBDDataGridView DataGridA { get; }
        HBDDataGridView DataGridB { get; }
        void EndInit();
        void HideAllColumns();
        void HideAllRows();
        void HidEmptyFormatRows();
        void Refresh();
        void ResumeBinding();
        event EventHandler<DataGridSelectionChangedEventArgs> SelectionChanged;
        void ShowAllColumns();
        void ShowAllRows();
        void SortColumnHeaders();
        void SortColumnHeaders(HBD.Framework.Data.Comparison.FieldComparison excludeColumn);
        void SortColumnHeadersByIndexOfFields(HBD.Framework.Data.Comparison.FieldComparisonCollection fields);
        void SortRowsBy(HBD.Framework.Data.Comparison.FieldComparison field, System.ComponentModel.ListSortDirection direction);
        void SuspendBinding();
    }
}
