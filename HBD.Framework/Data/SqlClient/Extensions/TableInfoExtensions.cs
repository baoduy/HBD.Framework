using HBD.Framework.Data.SqlClient.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HBD.Framework.Data.SqlClient
{
    public static class TableInfoExtensions
    {
        #region TableInfo

        public static DataTable CreateDataTable(this TableInfo @this)
        {
            if (@this == null) return null;
            var data = new DataTable { TableName = @this.Name.FullName };

            foreach (var col in @this.Columns.OrderBy(c => c.OrdinalPosition))
            {
                var c = data.Columns.Add(col.Name, col.GetRuntimeType());
                c.AllowDBNull = col.IsNullable;
                c.AutoIncrement = col.IsIdentity;

                if (col.IsComputed && col.ComputedExpression.IsNullOrEmpty())
                    throw new Exception($"Computed Column {col.Name} is required the 'ComputedExpression'");

                c.ReadOnly = col.IsComputed;
                c.Expression = col.ComputedExpression;

                if (col.IsPrimaryKey)
                {
                    var list = new List<DataColumn>(data.PrimaryKey);
                    list.Add(c);
                    data.PrimaryKey = list.ToArray();
                }
            }

            return data;
        }

        #endregion TableInfo

        #region TableInfo Collection

        public static IList<TableInfo> SortByDependences(this TableInfoCollection @this)
            => @this.OrderBy(t => t.DependenceIndex).ToList();

        /// <summary>
        ///     Sort the input table by DependenceIndex
        /// </summary>
        /// <param name="@this"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public static string[] SortByDependences(this TableInfoCollection @this, params string[] tableNames)
            => tableNames.OrderBy(t => @this[t]?.DependenceIndex ?? 0).ToArray();

        public static IEnumerable<TableInfo> GetTableInfoByName(this TableInfoCollection @this,
            params string[] tableNames)
            => @this.Where(t => tableNames.Any(s => s == t.Name));

        #endregion TableInfo Collection
    }
}