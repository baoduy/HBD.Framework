#region using

using System.Data;
using System.Linq;
using HBD.Framework.Data.GetSetters;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Data
{
#if !NETSTANDARD1_6
    public static class GetSetterExtensions
    {
        public static DataTable ToDataTable(this IGetSetterCollection @this, bool firstRowIsColumnName = false,
            ColumnNamingType columnNamingType = ColumnNamingType.Auto)
        {
            var data = new DataTable();
            @this.ToDataTable(data, firstRowIsColumnName, columnNamingType);
            return data;
        }

        public static void ToDataTable(this IGetSetterCollection @this, DataTable dataTable, bool firstRowIsColumnName = false,
            ColumnNamingType columnNamingType = ColumnNamingType.Auto)
        {
            if (@this == null) return;
            Guard.ArgumentIsNotNull(dataTable, nameof(dataTable));

            dataTable.TableName = @this.Name;

            var geters = @this.ToList();
            var index = 0;

            if (!firstRowIsColumnName)
            {
                if (@this.Header != null)
                    foreach (var name in @this.Header)
                        dataTable.Columns.Add(name.ToString());
            }
            else
            {
                index = 1;
                var firstRow = geters.FirstOrDefault();
                if (firstRow != null)
                    foreach (var name in firstRow)
                        dataTable.Columns.Add(name.ToString());
            }

            for (; index < geters.Count; index++)
            {
                var vals = geters[index].ToArray();
                dataTable.AddMoreColumns(vals.Length, columnNamingType);
                dataTable.Rows.Add(vals);
            }
        }
    }
#endif
}