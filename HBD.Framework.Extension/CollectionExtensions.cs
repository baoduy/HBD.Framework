using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HBD.Framework.Extension
{
    public enum SortDirection
    { Ascending, Descending }
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var i in items)
                collection.Add(i);
        }

        public static void EnqueueAll<TSource>(this Queue<TSource> queue, IEnumerable<TSource> collection)
        {
            foreach (var item in collection)
                queue.Enqueue(item);
        }

        private static string BuildSortExpression(SortDirection direction, params string[] columnNames)
        {
            var build = new StringBuilder();
            foreach (var col in columnNames)
            {
                if (build.Length > 0) build.Append(",");
                build.AppendFormat("[{0}] {1}", col, direction == SortDirection.Ascending ? "ASC" : "DESC");
            }
            return build.ToString();
        }
        public static DataTable Sort(this DataTable data, SortDirection direction, params string[] columnNames)
        {
            var sortExpression = BuildSortExpression(direction, columnNames);

            var rows = data.Select("1=1", sortExpression);
            var sortedTable = data.Clone();
            foreach (var row in rows)
                sortedTable.ImportRow(row);
            data = sortedTable;
            return sortedTable;
        }
    }
}
