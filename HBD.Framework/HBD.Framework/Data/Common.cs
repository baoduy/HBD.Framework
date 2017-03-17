#region

using System.Linq;

#endregion

namespace HBD.Framework.Data
{
    public static class Common
    {
        public static string GetSqlName(string name)
        {
            if (name.IsNullOrEmpty()
                || name.Contains("*")
                || IsMethod(name))
                return name;

            if (name.Contains("."))
            {
                var split = name.Split('.');
                return string.Join(".", split.Select(GetSqlName));
            }
            if ((name.Contains("[") && name.Contains("]"))
                || (name.Contains("(") && name.Contains(")")))
                return name;
            return $"[{name}]";
        }

        public static string GetFullName(string schema, string name) => $"[{schema}].[{name}]";

        /// <summary>
        ///     Remove the Brackets '[' and ']' in SQLName.
        ///     Input is [TableName] return is TableName.
        /// </summary>
        /// <param name="sqlName">The SQL Name</param>
        /// <returns>The Name without brackets</returns>
        public static string RemoveSqlBrackets(string sqlName)
        {
            if (sqlName.IsNullOrEmpty()) return sqlName;
            return sqlName.Replace("[", string.Empty).Replace("]", string.Empty);
        }

        public static bool IsMethod(string value) => value.IsNotNullOrEmpty() && value.AnyOf("'", "(", "[");
    }
}