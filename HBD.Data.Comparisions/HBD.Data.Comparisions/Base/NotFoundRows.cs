#region

using System.Collections.Generic;
using System.Data;

#endregion

namespace HBD.Data.Comparisons.Base
{
    public class NotFoundRows
    {
        public IList<DataRow> Rows { get; } = new List<DataRow>();
        public IList<DataRow> CompareRows { get; } = new List<DataRow>();
    }
}