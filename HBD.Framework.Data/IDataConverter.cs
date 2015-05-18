using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HBD.Framework.Data
{
    public interface IDataConverter : IDisposable
    {
        /// <summary>
        /// Convert particular objet to data table. if the name is empty, the first object will be taken.
        /// </summary>
        /// <param name="name">the name of object</param>
        /// <returns></returns>
        DataTable ToDataTable(string name = null);

        /// <summary>
        /// Convert all objects to Dataset.
        /// </summary>
        /// <returns></returns>
        DataSet ToDataSet();
    }
}
