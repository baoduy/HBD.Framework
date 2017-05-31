#region

using System;

#endregion

namespace HBD.Data.Comparisons.Base
{
    public interface IComparisionResult : IDisposable
    {
        bool IsIdentity { get; }
        NotFoundRows NotFoundRows { get; }
        DifferenceRowCollection Rows { get; }
        IDataTableComparision Source { get; }

        DataTablesResult GetDataTableResult(DataTablesResultType type);
    }
}