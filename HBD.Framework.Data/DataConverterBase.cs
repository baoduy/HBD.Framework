using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HBD.Framework.Data
{
    public abstract class DataConverterBase : IDataConverter
    {
        public abstract DataTable ToDataTable(string name = null);

        public virtual DataSet ToDataSet()
        {
            var dataSet = new DataSet();
            dataSet.Tables.Add(this.ToDataTable());
            return dataSet;
        }

        public virtual void Dispose() { }
    }
}
