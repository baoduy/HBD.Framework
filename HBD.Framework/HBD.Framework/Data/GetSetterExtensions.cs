#region

using System.Data;
using System.Linq;
using HBD.Framework.Data.GetSetters;

#endregion

namespace HBD.Framework.Data
{
    public static class GetSetterExtensions
    {
        public static DataTable ToDataTable(this IGetSetterCollection @this, bool firstRowIsColumnName = false,
            ColumnNamingType columnNamingType = ColumnNamingType.Auto)
        {
            if (@this == null) return null;
         
            var geters = @this.ToList();
            var data = new DataTable(@this.Name);
            var index = 0;

            if (!firstRowIsColumnName)
            {
                if (@this.Header != null)
                    foreach (var name in @this.Header)
                        data.Columns.Add(name.ToString());
            }
            else
            {
                index = 1;
                var firstRow = geters.FirstOrDefault();
                if (firstRow != null)
                    foreach (var name in firstRow)
                        data.Columns.Add(name.ToString());
            }

            for (; index < geters.Count; index++)
            {
                var vals = geters[index].ToArray();
                data.AddMoreColumns(vals.Length, columnNamingType);
                data.Rows.Add(vals);
            }
            ;

            return data;
        }
    }
}