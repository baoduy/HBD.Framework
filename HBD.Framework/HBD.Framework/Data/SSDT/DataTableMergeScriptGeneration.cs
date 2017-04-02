#region using

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Data.SSDT
{
    public class DataTableMergeScriptGeneration : IDataTableMergeScriptGeneration
    {
        public DataTableMergeScriptGeneration(string outputFolder = null)
        {
            OutputFolder = outputFolder.IsNullOrEmpty() ? DefaultOutputFolder : outputFolder;
        }

        public string OutputFolder { get; set; }

        public virtual void Generate(IEnumerable<DataTable> tables, MergeScriptOption option = MergeScriptOption.Default,
            Action<string> updateStatus = null)
        {
            if (!Directory.Exists(OutputFolder))
                Directory.CreateDirectory(OutputFolder);

            var builder = new StringBuilder();
            AddHeader(builder);

            foreach (var tb in tables)
            {
                updateStatus?.Invoke(tb.TableName);

                var script = Generate(tb, option);
                if (string.IsNullOrEmpty(script)) continue;
                try
                {
                    var fileName = GetFileName(tb.TableName);
                    builder.AppendFormat(":r .\\{0}", fileName).Append(Environment.NewLine);
                    File.WriteAllText(Path.Combine(OutputFolder, fileName), script);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            //Write Execute Script.
            File.WriteAllText(Path.Combine(OutputFolder, DefaultOutputExecuteFileName), builder.ToString());
        }

        public virtual Task GenerateAsync(IEnumerable<DataTable> tables,
            MergeScriptOption option = MergeScriptOption.Default, Action<string> updateStatus = null)
            => Task.Run(() => Generate(tables, option, updateStatus));

        /// <summary>
        ///     Generate Static Data Migration Script From DataTable
        /// </summary>
        /// <param name="table"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public virtual string Generate(DataTable table, MergeScriptOption option = MergeScriptOption.Default)
        {
            if (table == null || table.Rows.Count == 0) return null;
            var tableName = Common.GetSqlName(table.TableName);
            var isIdentity = table.Columns.OfType<DataColumn>().Any(c => c.AutoIncrement);

            var builder = new StringBuilder();
            AddHeader(builder);

            builder.AppendFormat("PRINT N'Merging static data to {0}';", tableName).Append(Environment.NewLine)
                .Append("GO").Append(Environment.NewLine).Append(Environment.NewLine);

            if (isIdentity)
                builder.Append("BEGIN TRY").Append(Environment.NewLine)
                    .Append($"SET IDENTITY_INSERT {tableName} ON").Append(Environment.NewLine)
                    .Append("END TRY").Append(Environment.NewLine)
                    .Append("BEGIN CATCH").Append(Environment.NewLine)
                    .Append("END CATCH").Append(Environment.NewLine)
                    .Append("GO").Append(Environment.NewLine).Append(Environment.NewLine);

            builder.Append($"MERGE INTO {tableName} AS Target")
                .Append(Environment.NewLine).Append("USING( VALUES").Append(Environment.NewLine);

            for (var i = 0; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                var strValues = string.Join(",", BuildSqlValueList(row.ItemArray));
                builder.Append("(").Append(strValues).Append(")");
                if (i < table.Rows.Count - 1) builder.Append(",");
                builder.Append(Environment.NewLine);
            }

            var columns = BuildSqlFieldList(table.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray());
            builder.AppendFormat(")AS Source({0})", string.Join(",", columns))
                .Append(Environment.NewLine)
                .Append("ON ");

            var primaryKeys = string.Join(" AND ",
                table.PrimaryKey.Select(p => $"Target.[{p.ColumnName}] = Source.[{p.ColumnName}]"));
            builder.Append(primaryKeys).Append(Environment.NewLine);

            var setColumns = string.Join("," + Environment.NewLine,
                table.Columns.Cast<DataColumn>()
                    .Where(c => !table.PrimaryKey.Contains(c))
                    .Select(c => $"[{c.ColumnName}] = Source.[{c.ColumnName}]"));

            if (option.HasFlag(MergeScriptOption.Update)
                && !string.IsNullOrWhiteSpace(setColumns))
                builder.Append("WHEN MATCHED THEN").Append(Environment.NewLine)
                    .Append("UPDATE SET ")
                    .Append(setColumns).Append(Environment.NewLine);

            if (option.HasFlag(MergeScriptOption.Insert))
                builder.Append("WHEN NOT MATCHED BY TARGET THEN").Append(Environment.NewLine)
                    .AppendFormat("INSERT({0})", columns).Append(Environment.NewLine)
                    .AppendFormat("VALUES({0})", columns).Append(Environment.NewLine);

            if (option.HasFlag(MergeScriptOption.Delete))
                builder.Append("WHEN NOT MATCHED BY SOURCE THEN").Append(Environment.NewLine)
                    .Append("DELETE");

            builder.Append(";").Append(Environment.NewLine)
                .Append("GO").Append(Environment.NewLine).Append(Environment.NewLine);

            if (isIdentity)
                builder.Append("BEGIN TRY").Append(Environment.NewLine)
                    .Append($"SET IDENTITY_INSERT {tableName} OFF").Append(Environment.NewLine)
                    .Append("END TRY").Append(Environment.NewLine)
                    .Append("BEGIN CATCH").Append(Environment.NewLine)
                    .Append("END CATCH").Append(Environment.NewLine)
                    .Append("GO").Append(Environment.NewLine).Append(Environment.NewLine);

            builder.AppendFormat("PRINT N'Completed merge static data to {0}';", table)
                .Append(Environment.NewLine)
                .Append(Environment.NewLine);
            return builder.ToString();
        }

        #region constants

        protected const string DefaultOutputFolder = "Output";
        protected const string DefaultOutputFileName = "Merge_Data_{0}_Table.sql";
        protected const string DefaultOutputExecuteFileName = "Merges_Data.sql";

        #endregion constants

        #region Protected Methods

        public static string GetFileName(string tableName)
            =>
                string.Format(DefaultOutputFileName,
                    tableName.Replace("[", string.Empty).Replace("]", string.Empty).Replace(" ", string.Empty));

        public virtual void AddHeader(StringBuilder builder)
        {
            builder.Append("/*").Append(Environment.NewLine)
                .AppendFormat("Generated Date: {0:dd-MMM-yyyy hh:mm:ss}", DateTime.Now).Append(Environment.NewLine)
                .AppendFormat("Generated User: {0}", UserPrincipalHelper.UserName).Append(Environment.NewLine)
                .Append("*/").Append(Environment.NewLine).Append(Environment.NewLine);
        }

        #endregion Protected Methods

        #region Static Methods

        private static string BuildSqlFieldList(string[] fields)
        {
            var builder = new StringBuilder();
            if (fields.Length == 0) return builder.ToString();

            foreach (var f in fields)
            {
                if (builder.Length > 0) builder.Append(",");
                builder.Append(Common.GetSqlName(f));
            }

            return builder.ToString();
        }

        public static byte[] GetHexBinaryBytes(string value)
        {
            var shb = SoapHexBinary.Parse(value);
            return shb.Value;
        }

        public static string GetHexBinaryString(byte[] value)
        {
            var shb = new SoapHexBinary(value);
            return "0x" + shb;
        }

        private static string BuildSqlValueList(object[] items)
        {
            var builder = new StringBuilder();
            if (items.Length == 0) return builder.ToString();

            foreach (var obj in items)
            {
                if (builder.Length > 0) builder.Append(",");

                if (obj == null || obj == DBNull.Value)
                {
                    builder.Append("NULL");
                }
                else if (obj is byte[])
                {
                    var strByte = GetHexBinaryString((byte[]) obj);
                    builder.Append(strByte);
                }
                else if (obj is bool)
                {
                    builder.Append(obj.Equals(true) ? 1 : 0);
                }
                else if (obj is DateTime)
                {
                    builder.AppendFormat("'{0:yyyy-MM-dd hh:mm:ss}'", (DateTime) obj);
                }
                else
                {
                    var strValue = obj.ToString().Replace("'", "''");
                    builder.AppendFormat(obj is string ? "N'{0}'" : "{0}", strValue);
                }
            }

            return builder.ToString();
        }

        #endregion Static Methods
    }

    [Flags]
    public enum MergeScriptOption
    {
        Default = Insert | Update,
        All = Insert | Update | Delete,
        Insert = 2,
        Update = 4,
        Delete = 8
    }
}