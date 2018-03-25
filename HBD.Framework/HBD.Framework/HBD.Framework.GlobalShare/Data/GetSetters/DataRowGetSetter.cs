#region using

using System.Collections;
using System.Collections.Generic;
using System.Data;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Data.GetSetters
{
#if !NETSTANDARD1_6
    internal class DataRowGetSetter : IGetSetter
    {
        public DataRowGetSetter(DataRow row)
        {
            Guard.ArgumentIsNotNull(row, nameof(row));
            OrginalRow = row;
        }

        public DataRow OrginalRow { get; }

        public int Count => OrginalRow.ItemArray.Length;

        public object this[string name]
        {
            get => OrginalRow[name];
            set => OrginalRow[name] = value;
        }

        public object this[int index]
        {
            get => OrginalRow[index];
            set => OrginalRow[index] = value;
        }

        public IEnumerator<object> GetEnumerator() => ((IEnumerable<object>) OrginalRow.ItemArray).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
#endif
}