#region

using System.Collections;
using System.Collections.Generic;
using System.Data;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Data.GetSetters
{
    internal class DataRowGetSetter : IGetSetter
    {
        public DataRowGetSetter(DataRow row)
        {
            Guard.ArgumentIsNotNull(row, nameof(row));
            OrginalRow = row;
        }

        public DataRow OrginalRow { get; }

        public object this[string name]
        {
            get { return OrginalRow[name]; }
            set { OrginalRow[name] = value; }
        }

        public object this[int index]
        {
            get { return OrginalRow[index]; }
            set { OrginalRow[index] = value; }
        }

        public IEnumerator<object> GetEnumerator() => ((IEnumerable<object>) OrginalRow.ItemArray).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}