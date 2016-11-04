using HBD.Framework.Core;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace HBD.Framework.Data.GetSetters
{
    internal class DataRowGetSetter : IGetSetter
    {
        public DataRow OrginalRow { get; }

        public DataRowGetSetter(DataRow row)
        {
            Guard.ArgumentIsNotNull(row, nameof(row));
            this.OrginalRow = row;
        }

        public object this[string name]
        {
            get { return this.OrginalRow[name]; }
            set { this.OrginalRow[name] = value; }
        }

        public object this[int index]
        {
            get { return this.OrginalRow[index]; }
            set { this.OrginalRow[index] = value; }
        }

        public IEnumerator<object> GetEnumerator() => ((IEnumerable<object>)this.OrginalRow.ItemArray).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}