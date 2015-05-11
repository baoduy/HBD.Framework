using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Data.Comparison;
using System.Data;
using HBD.Framework.Data;
using HBD.Framework.Core;
using System.ComponentModel;
using System.Reflection;
using HBD.Framework.Extension;
using HBD.Framework.Data.Utilities;

namespace HBD.Framework.Extension.WinForms
{
    public static class WinFormsExtension
    {
        public static void HideAllColumns(this DataGridView grid)
        {
            grid.ClearSelection();
            foreach (DataGridViewColumn col in grid.Columns)
                col.Visible = false;
        }

        public static void ShowAllColumns(this DataGridView grid)
        {
            foreach (DataGridViewColumn col in grid.Columns)
                col.Visible = true;
        }

        public static void HideAllRows(this DataGridView grid)
        {
            grid.ClearSelection();
            for (int i = 0; i < grid.RowCount; i++)
                grid.Rows[i].Visible = false;
        }

        public static void SortColumnHeaders(this DataGridView grid, params string[] excludeColumns)
        {
            for (int i = 0; i < grid.ColumnCount - 1; i++)
            {
                var coli = grid.Columns[i];

                if (excludeColumns != null
                    && excludeColumns.Contains(coli.HeaderText))
                    continue;

                for (int j = i + 1; j < grid.ColumnCount; j++)
                {
                    var colj = grid.Columns[j];

                    if (coli.HeaderText.Trim().ToLower().CompareTo(colj.HeaderText.Trim().ToLower()) > 0)
                    {
                        int tmp = coli.DisplayIndex;
                        coli.DisplayIndex = colj.DisplayIndex;
                        colj.DisplayIndex = tmp;
                    }
                }
            }
        }

        public static void SortColumnHeadersByIndexOfFields(this DataGridView grid, IEnumerable<string> fields)
        {
            if (fields == null)
                return;

            int i = 0;
            foreach (string f in fields)
            {
                if (grid.Columns.Contains(f))
                    grid.Columns[f].DisplayIndex = i++;
            }
        }

        public static ColumnNamesCollection ToList(this DataColumnCollection columns)
        {
            return new ColumnNamesCollection(columns);
        }

        private static FieldInfo GetLayoutSuspendCountField()
        {
            return typeof(Control).GetField("layoutSuspendCount", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        }

        public static byte GetLayoutSuspendCount(this Form form)
        {
            var field = GetLayoutSuspendCountField();
            if (field != null)
                return (byte)field.GetValue(form);
            return 0;
        }

        public static byte GetLayoutSuspendCount(this UserControl control)
        {
            var field = GetLayoutSuspendCountField();
            if (field != null)
                return (byte)field.GetValue(control);
            return 0;
        }

        public static bool IsLayoutSuspended(this Form form)
        {
            return form.GetLayoutSuspendCount() > 0;
        }

        public static bool IsLayoutSuspended(this UserControl control)
        {
            return control.GetLayoutSuspendCount() > 0;
        }

        /// <summary>
        /// Find control/chillrend controls by name
        /// </summary>
        /// <param name="control"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object FindControl(this Control control, string name)
        {
            Guard.ArgumentNotNull(control, "control");

            var queue = new Queue<object>();
            queue.Enqueue(control);

            do
            {
                var item = queue.Dequeue();
                if (item is Control)
                {
                    var co = item as Control;

                    if (co.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                        return co;
                    if (co.Controls.Count > 0)
                        queue.EnqueueAll(co.Controls.Cast<object>());
                }

                if (item is ToolStrip)
                {
                    var tool = (ToolStrip)item;
                    if (tool.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                        return tool;
                    if (tool.Items.Count > 0)
                        queue.EnqueueAll(tool.Items.Cast<object>());
                }
                else if (item is ToolStripItem)
                {
                    var tool = (ToolStripItem)item;
                    if (tool.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                        return tool;
                }

            } while (queue.Count > 0);

            return null;
        }

        #region Filtering
        public static bool IsSatisfy(this DataGridViewRow row, FilterClause filter)
        {
            Guard.ArgumentNotNull(row, "DataGridViewRow");
            Guard.ArgumentNotNull(filter, "FilterClause");

            if (!row.DataGridView.Columns.Contains(filter.FieldName))
                return true;

            var rowValue = row.Cells[filter.FieldName].Value;
            return filter.Value.Compare(rowValue, filter.Operation);
        }

        public static bool IsSatisfy(this DataGridViewRow row, BinaryFilterItem filter)
        {
            Guard.ArgumentNotNull(filter, "BinaryFilterItem");

            switch (filter.Operation)
            {
                case BinaryOperation.AND:
                    return row.IsSatisfy(filter.LeftClause) && row.IsSatisfy(filter.RightClause);
                case BinaryOperation.OR:
                    return row.IsSatisfy(filter.LeftClause) || row.IsSatisfy(filter.RightClause);
            }
            return false;
        }

        public static bool IsSatisfy(this DataGridViewRow row, IFilterClause filterClause)
        {
            Guard.ArgumentNotNull(filterClause, "IFilterClause");

            if (filterClause is FilterClause)
            {
                return row.IsSatisfy(filterClause as FilterClause);
            }
            else if (filterClause is BinaryFilterItem)
            {
                return row.IsSatisfy(filterClause as BinaryFilterItem);
            }
            else throw new ArgumentException(string.Format("This render is not cover the FilterClause type: {0}", filterClause.GetType().FullName));
        }

        public static void FilterRows(this DataGridView grid, IFilterClause filterClause)
        {
            Guard.ArgumentNotNull(grid, "DataGridView");
            Guard.ArgumentNotNull(filterClause, "FilterClause");

            grid.ClearSelection();
            grid.CurrentCell = null;

            foreach (DataGridViewRow row in grid.Rows)
                row.Visible = row.IsSatisfy(filterClause);
        }
        #endregion
    }
}
