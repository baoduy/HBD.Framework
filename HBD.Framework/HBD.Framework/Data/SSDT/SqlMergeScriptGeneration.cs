#region using

using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HBD.Framework.Data.SqlClient;
using HBD.Framework.Data.SqlClient.Extensions;

#endregion

namespace HBD.Framework.Data.SSDT
{
    public class SqlMergeScriptGeneration : DataTableMergeScriptGeneration, ISqlMergeScriptGeneration
    {
        private readonly SqlClientAdapter _conn;

        public SqlMergeScriptGeneration(string nameOrConnectionString, string outputFolder = null) : base(outputFolder)
        {
            _conn = new SqlClientAdapter(nameOrConnectionString);
        }

        public void Dispose() => _conn?.Dispose();

        private string[] SortTablesByDependence(string[] tables)
        {
            if (tables == null || tables.Length <= 1) return tables;
            return _conn?.GetSchemaInfo().Tables.SortByDependences(tables);
        }

        #region Public Methods

        /// <summary>
        ///     Generate Static Data Migration Script for all Tables in DB.
        /// </summary>
        /// <param name="option"></param>
        public virtual void GenerateAll(MergeScriptOption option = MergeScriptOption.Default)
        {
            var tables = _conn?.GetSchemaInfo().Tables.Select(t => t.Name.FullName).ToArray();
            Generate(option, tables);
        }

        public virtual void Generate(MergeScriptOption option = MergeScriptOption.Default, params string[] tables)
            => Generate(option, null, tables);

        /// <summary>
        ///     Generate Static Data Migration Script for the list of Tables.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="updateStatus"></param>
        /// <param name="tables">The list of Table Name</param>
        public virtual void Generate(MergeScriptOption option = MergeScriptOption.Default,
            Action<string> updateStatus = null, params string[] tables)
        {
            if (tables.Length == 0) throw new ArgumentNullException(nameof(tables));

            var datatbs = from tb in SortTablesByDependence(tables)
                select GetDataTable(tb);
            Generate(datatbs, option, updateStatus);
        }

        public virtual Task GenerateAsync(MergeScriptOption option = MergeScriptOption.Default, params string[] tables)
            => Task.Run(() => Generate(option, tables));

        public virtual Task GenerateAsync(MergeScriptOption option = MergeScriptOption.Default,
            Action<string> updateStatus = null, params string[] tables)
            => Task.Run(() => Generate(option, updateStatus, tables));

        #endregion Public Methods

        #region Protected Methods

        protected virtual string Generate(string table, MergeScriptOption option = MergeScriptOption.Default)
        {
            using (var data = GetDataTable(table))
            {
                return Generate(data, option);
            }
        }

        protected virtual DataTable GetDataTable(string table)
            => _conn.ExecuteTable($"SELECT * FROM {Common.GetSqlName(table)}");

        #endregion Protected Methods
    }
}