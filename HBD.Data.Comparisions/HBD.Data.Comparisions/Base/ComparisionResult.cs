#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace HBD.Data.Comparisons.Base
{
    public class ComparisionResult : IComparisionResult
    {
        private readonly IDictionary<DataTablesResultType, DataTablesResult> _tbResults =
            new Dictionary<DataTablesResultType, DataTablesResult>();

        public ComparisionResult(IDataTableComparision source)
        {
            Source = source;
        }

        public virtual IDataTableComparision Source { get; }

        public virtual DifferenceRowCollection Rows { get; } = new DifferenceRowCollection();
        public virtual NotFoundRows NotFoundRows { get; } = new NotFoundRows();

        public virtual DataTablesResult GetDataTableResult(DataTablesResultType type)
        {
            if (_tbResults.ContainsKey(type)) return _tbResults[type];

            var result = new DataTablesResult(this, type);

            if (type.HasFlag(DataTablesResultType.DifferenceRows))
                result.AddRange(Rows.Where(r => r.Cells.Length > 0));

            if (type.HasFlag(DataTablesResultType.NotFoundRows))
                result.Add(NotFoundRows);

            if (type.HasFlag(DataTablesResultType.IdentityRows))
                result.AddRange(Rows.Where(r => r.Cells.Length == 0));

            _tbResults[type] = result;

            return result;
        }

        public bool IsIdentity => !Rows.Any(c => c.Cells.Length > 0);

        public void Dispose()
        {
            foreach (var result in _tbResults)
                result.Value.Dispose();
            _tbResults.Clear();
        }
    }

    [Flags]
    public enum DataTablesResultType
    {
        None = 0,
        All = IdentityRows | DifferenceRows | NotFoundRows,
        IdentityRows = 2,
        DifferenceRows = 4,
        NotFoundRows = 8
    }
}