using System;
using System.Collections.Generic;
using System.Linq;
using HBD.Framework.Core;
using HBD.Framework.Exceptions;

namespace HBD.Framework
{
    public static class CommonFuncs
    {
        /// <summary>
        /// Get Datatable colum name as format F1,F2,F3,F4
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static string GetColumnName(int columnIndex)
        {
            columnIndex.ShouldGreaterThan(-1, nameof(columnIndex));

            return $"F{columnIndex + 1}";
        }

        public static int GetColumnIndex(string columnName)
        {
            Guard.ArgumentIsNotNull(columnName, nameof(columnName));
            var index = -1;

            if (int.TryParse(columnName.Remove(0, 1), out index))
                return index - 1;
            throw new InvalidException("The columnName format is invalid. It should be a 'F{index}'.");
        }


        /// <summary>
        ///     Get Excel Column Name by Index
        ///     Index will start from 0.
        /// </summary>
        /// <returns></returns>
        public static string GetExcelColumnName(int columnIndex)
        {
            columnIndex.ShouldGreaterThan(-1, nameof(columnIndex));

            var dividend = columnIndex + 1;
            var list = new List<char>();

            //26 = 'Z' - 'A' + 1;
            while (dividend > 0)
            {
                var modulo = (dividend - 1) % 26;
                list.Add(Convert.ToChar(65 + modulo)); //'A' + modulo
                dividend = (dividend - modulo) / 26;
            }

            list.Reverse();
            return string.Join(string.Empty, list);
        }

        /// <summary>
        ///     Get ColumnIndex from ColumnLabled.
        ///     The index will start from 0 according with column label is 'A'.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static int GetExcelColumnIndex(string columnName)
        {
            Guard.ArgumentIsNotNull(columnName, nameof(columnName));

            if (columnName.Any(c => (c < 'A') || (c > 'Z')))
                throw new ArgumentOutOfRangeException(nameof(columnName));

            var colIndex = 0;
            for (int i = 0, pow = columnName.Count() - 1; i < columnName.Count(); ++i, --pow)
            {
                var cVal = Convert.ToInt32(columnName[i]) - 64;
                colIndex += cVal * (int)Math.Pow(26, pow);
            }
            return colIndex - 1;
        }
    }
}
