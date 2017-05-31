#region

using System.Collections.Generic;
using System.Linq;
using HBD.Framework.Core;

#endregion

namespace HBD.Data.Comparisons.Base
{
    public class CompareColumnCollection : List<ICompareColumnInfo>
    {
        protected internal CompareColumnCollection(DataTableComparision parrent) : this(parrent, null)
        {
        }

        protected internal CompareColumnCollection(DataTableComparision parrent,
            IEnumerable<ICompareColumnInfo> collection)
        {
            Guard.ArgumentIsNotNull(parrent, nameof(parrent));
            Parrent = parrent;

            if (collection != null)
                AddRange(collection);
        }

        protected internal IDataTableComparision Parrent { get; }

        protected virtual ICompareColumnInfo NewCompareColumnInfo(string columnName, string compareColumnName = null)
            => new CompareColumnInfo(columnName, compareColumnName);

        public ICompareColumnInfo Add(string columnName, string compareColumnName)
        {
            var col = NewCompareColumnInfo(columnName, compareColumnName);
            Add(col);
            return col;
        }

        public new void Add(ICompareColumnInfo column)
        {
            if (column == null) return;
            if (this.Any(f => f == column)) return;

            column.ParentComparision = Parrent;
            base.Add(column);
        }

        public new void AddRange(IEnumerable<ICompareColumnInfo> collection)
        {
            foreach (var item in collection)
                Add(item);
        }
    }
}