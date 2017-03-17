#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HBD.Framework.Cache;
using HBD.Framework.Core;
using HBD.Framework.Data.Base;

#endregion

namespace HBD.Framework.Data.SqlClient.Base
{
    public abstract class SqlClientAdapterBase : DataClientAdapter
    {
        protected SqlClientAdapterBase(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        protected SqlClientAdapterBase(DbConnectionStringBuilder connectionString) : base(connectionString)
        {
        }

        protected SqlClientAdapterBase(IDbConnection connection) : base(connection)
        {
        }

        #region DataBase Info Methods

        /// <summary>
        ///     Get available Schema Names in SQL.
        ///     The names will be cache into CacheManager.Default.
        /// </summary>
        /// <returns></returns>
        public virtual IReadOnlyCollection<DatabaseInfo> GetDataBaseInfos()
        {
            IReadOnlyCollection<DatabaseInfo> schemas;
            if (CacheManager.Default.IsContains(CacheAllSchemaNameKey))
            {
                schemas = CacheManager.Default.Get<IReadOnlyCollection<DatabaseInfo>>(CacheAllSchemaNameKey);
                if (schemas == null) CacheManager.Default.Remove(CacheAllSchemaNameKey);
                else return schemas;
            }

            var list = new List<DatabaseInfo>();
            using (var reader = ExecuteReader(QueryAllSchemaNames))
            {
                while (reader.Read())
                    list.Add(new DatabaseInfo(this, reader.GetValue<string>(FieldTableCatalog)));
            }

            schemas = list;
            CacheManager.Default.AddOrUpdate(CacheAllSchemaNameKey, schemas);
            return schemas;
        }

        #endregion DataBase Info Methods

        #region Fields

        private const string FieldOrdinalPosition = "Ordinal_Position";
        private const string FieldTableCatalog = "Table_Catalog";
        private const string FieldTableName = "Table_Name";
        private const string FieldTableSchema = "Table_Schema";
        private const string FieldColumnName = "Column_Name";
        private const string FieldIsNullable = "Is_Nullable";
        private const string FieldDataType = "Data_Type";
        private const string FieldCharacterMaximumLength = "Character_Maximum_Length";
        private const string FieldIsIdentity = "Is_Identity";
        private const string FieldIsComputed = "Is_Computed";
        private const string FieldIsPrimaryKey = "Is_Primary_Key";
        private const string FieldPrimaryKeyName = "Primary_Key_Name";
        private const string FieldIsPoreignKey = "Is_Foreign_Key";
        private const string FieldFkTableSchema = "Fk_Table_Schema";
        private const string FieldFkTableName = "Fk_Table_Name";
        private const string FieldFkColumnName = "Fk_Column_Name";
        private const string FieldForeignKeyName = "Foreign_Key_Name";
        private const string FieldRowCount = "Row_Count";
        private const string FieldIsTable = "Is_Table";
        private const string FieldIsView = "Is_View";

        private const string CacheSchemaInfoKey = "SchemaInfoOf_";
        private const string CacheAllSchemaNameKey = "AllSchemaNames";

        private const string QueryAllSchemaNames = @"SELECT [Name] as [Table_Catalog]
FROM   Sys.Databases
WHERE  [Name] NOT IN('master', 'tempdb', 'model', 'msdb');
";

        private const string QuerySchemaInfo = @"SELECT DISTINCT
       C.Ordinal_Position,
       C.Table_Catalog,
       C.Table_Name,
       C.Table_Schema,
       C.Column_Name,
       Is_Nullable = CASE
                         WHEN C.Is_Nullable = 'YES'
                         THEN 1
                         ELSE NULL
                     END,
       C.Data_Type,
       C.Character_Maximum_Length,
       COLUMNPROPERTY(OBJECT_ID(C.Table_Schema+'.'+C.Table_Name), C.Column_Name, 'IsIdentity') AS Is_Identity,
       COLUMNPROPERTY(OBJECT_ID(C.Table_Schema+'.'+C.Table_Name), C.Column_Name, 'IsComputed') AS Is_Computed,
       Pk.Is_Primary_Key,
       Pk.Primary_Key_Name,
       Fk.Is_Foreign_Key,
       Fk.Fk_Table_Schema,
       Fk.Fk_Table_Name,
       Fk.Fk_Column_Name,
       Fk.Foreign_Key_Name,
       Tbcnt.Row_Count,
       Is_Table = CASE
                      WHEN T.Table_Type = 'BASE TABLE'
                      THEN 1
                      ELSE NULL
                  END,
       Is_View = CASE
                     WHEN T.Table_Type = 'VIEW'
                     THEN 1
                     ELSE NULL
                 END
FROM      Information_Schema.Columns C
          INNER JOIN Information_Schema.Tables T ON T.Table_Schema = C.Table_Schema
                                                    AND T.Table_Name = C.Table_Name
          LEFT JOIN
(
    SELECT Pk.Table_Schema,
           Pk.Table_Name,
           1 AS Is_Primary_Key,
           Cc.Column_Name,
           Cc.Constraint_Name AS Primary_Key_Name
    FROM   Information_Schema.Constraint_Column_Usage Cc
           INNER JOIN Information_Schema.Table_Constraints Pk ON Cc.Constraint_Name = Pk.Constraint_Name
                                                                 AND Pk.Constraint_Type = 'PRIMARY KEY'
) Pk ON C.Table_Schema = Pk.Table_Schema
        AND C.Table_Name = Pk.Table_Name
        AND C.Column_Name = Pk.Column_Name
          LEFT JOIN
(
    SELECT Tc1.Table_Schema,
           Tc1.Table_Name,
           1 AS Is_Foreign_Key,
           Cc1.Column_Name,
           Tc2.Table_Schema AS Fk_Table_Schema,
           Tc2.Table_Name AS Fk_Table_Name,
           Tc2.Constraint_Type AS Fk_Constraint_Type,
           Cc2.Column_Name AS Fk_Column_Name,
           Cc1.Constraint_Name AS Foreign_Key_Name
    FROM   Information_Schema.Table_Constraints Tc1
           INNER JOIN Information_Schema.Constraint_Column_Usage Cc1 ON Tc1.Constraint_Name = Cc1.Constraint_Name
           INNER JOIN Information_Schema.Referential_Constraints Re ON Cc1.Constraint_Name = Re.Constraint_Name
           INNER JOIN Information_Schema.Table_Constraints Tc2 ON Re.Unique_Constraint_Name = Tc2.Constraint_Name
           INNER JOIN Information_Schema.Constraint_Column_Usage Cc2 ON Tc2.Constraint_Name = Cc2.Constraint_Name
) Fk ON C.Table_Schema = Fk.Table_Schema
        AND C.Table_Name = Fk.Table_Name
        AND C.Column_Name = Fk.Column_Name
          LEFT JOIN
(--Get RowCount of Tables
    SELECT SCHEMA_NAME(T.Schema_Id) AS Table_Schema,
           T.Name AS Table_Name,
           R.Rows AS Row_Count
    FROM   Sys.Objects AS T
           INNER JOIN Sys.Partitions AS R ON T.Object_Id = R.Object_Id
    WHERE  T.Type = 'U'
           AND R.Index_Id < 2
) AS Tbcnt ON T.Table_Schema = Tbcnt.Table_Schema
              AND T.Table_Name = Tbcnt.Table_Name
ORDER BY C.Ordinal_Position;";

        #endregion Fields

        #region ShemaInfo Methods

        protected virtual string GetMaxPrimaryKeyQuery(IList<ColumnInfo> columns)
        {
            var builder = new StringBuilder();
            foreach (var col in columns)
            {
                if (builder.Length > 0) builder.Append("UNION").Append(Environment.NewLine);
                builder.AppendFormat(
                        "SELECT MaxValue = CAST(MAX({0}) as nvarchar), ColumnName='{0}', TableName = '{1}' FROM {1}",
                        col.Name, col.Table.Name)
                    .Append(Environment.NewLine);
            }
            return builder.ToString();
        }

        /// <summary>
        ///     Get Schema Information for current DB.
        ///     The Schema Information will be cache into CacheManager.Default
        /// </summary>
        /// <returns></returns>
        public virtual SchemaInfo GetSchemaInfo(string databaseName = null)
        {
            if (databaseName.IsNullOrEmpty())
            {
                var cn = (SqlConnectionStringBuilder) ConnectionString;
                databaseName = cn.InitialCatalog;
                if (databaseName.IsNullOrEmpty())
                    databaseName = cn.AttachDBFilename;
            }
            Guard.ArgumentIsNotNull(databaseName, nameof(databaseName));

            var cacheKey = CacheSchemaInfoKey + databaseName;
            SchemaInfo schema = null;
            if (CacheManager.Default.IsContains(cacheKey))
            {
                schema = CacheManager.Default.Get<SchemaInfo>(cacheKey);
                if (schema == null) CacheManager.Default.Remove(cacheKey);
                else return schema;
            }

            using (var reader = ExecuteReader(QuerySchemaInfo))
            {
                schema = new SchemaInfo(databaseName);

                while (reader.Read())
                {
                    #region Field Values

                    var ordinalPosition = reader.GetValue<int>(FieldOrdinalPosition);
                    var tableName = reader.GetValue<string>(FieldTableName);
                    var tableSchema = reader.GetValue<string>(FieldTableSchema);
                    var columnName = reader.GetValue<string>(FieldColumnName);
                    var isNullable = reader.GetValue<bool>(FieldIsNullable);
                    var dataType = reader.GetValue<string>(FieldDataType).ToSqlDbType();
                    var characterMaximumLength = reader.GetValue<int>(FieldCharacterMaximumLength);
                    var isIdentity = reader.GetValue<bool>(FieldIsIdentity);
                    var isComputed = reader.GetValue<bool>(FieldIsComputed);
                    var isPrimaryKey = reader.GetValue<bool>(FieldIsPrimaryKey);
                    var primaryKeyName = reader.GetValue<string>(FieldPrimaryKeyName);
                    var isPoreignKey = reader.GetValue<bool>(FieldIsPoreignKey);
                    var fkTableSchema = reader.GetValue<string>(FieldFkTableSchema);
                    var fkTableName = reader.GetValue<string>(FieldFkTableName);
                    var fkColumnName = reader.GetValue<string>(FieldFkColumnName);
                    var rowCount = reader.GetValue<int>(FieldRowCount);
                    var foreignKeyName = reader.GetValue<string>(FieldForeignKeyName);
                    var isTable = reader.GetValue<bool>(FieldIsTable);
                    var isView = reader.GetValue<bool>(FieldIsView);

                    #endregion Field Values

                    #region Collect Info

                    var tbName = new DbName(tableSchema, tableName);

                    if (isTable)
                    {
                        //Collect Tables
                        var table = schema.Tables[tbName];

                        if (table == null)
                        {
                            table = new TableInfo(tbName) {RowCount = rowCount};
                            schema.Tables.Add(table);
                        }

                        //Collect ColumnInfo
                        var column = table.Columns[columnName];
                        if (column == null)
                        {
                            column = new ColumnInfo
                            {
                                Name = columnName,
                                OrdinalPosition = ordinalPosition,
                                DataType = dataType,
                                IsIdentity = isIdentity,
                                IsPrimaryKey = isPrimaryKey,
                                IsNullable = isNullable,
                                MaxLengh = characterMaximumLength,
                                IsComputed = isComputed
                            };
                            table.Columns.Add(column);
                        }

                        if (!isPoreignKey) continue;

                        //Collect ForeignKeys
                        var refCol = new ReferencedColumnInfo(fkTableSchema, fkTableName, fkColumnName);
                        table.ForeignKeys.Add(new ForeignKeyInfo(foreignKeyName, column, refCol));
                    }
                    else
                    {
                        //Collect Views
                        var view = schema.Views[tbName];

                        if (view == null)
                        {
                            view = new ViewInfo(tbName);
                            schema.Views.Add(view);
                        }

                        if (!view.Columns.Contains(columnName))
                            view.Columns.Add(columnName);
                    }

                    #endregion Collect Info
                }
            }

            schema = GetMaxPrimaryKeyValues(schema);
            CacheManager.Default.AddOrUpdate(cacheKey, schema);
            return schema;
        }

        /// <summary>
        ///     Clear all cache items that in CacheManager.Default.
        /// </summary>
        public virtual void ClearCacheItems()
        {
            CacheManager.Default.Remove(CacheAllSchemaNameKey);
            foreach (
                var key in
                CacheManager.Default.GetAllKeys().Where(n => n.StartsWithIgnoreCase(CacheSchemaInfoKey)).ToList())
                CacheManager.Default.Remove(key);
        }

        /// <summary>
        ///     Get Max Primary Key for all Tables in Schema. Which the Primary key is not IsIdentity.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        protected virtual SchemaInfo GetMaxPrimaryKeyValues(SchemaInfo schema)
        {
            var columns = (from tb in schema.Tables
                from col in tb.Columns
                where col.IsPrimaryKey && !col.IsIdentity
                select col).ToList();

            if (columns.Count == 0) return schema;

            using (var reader = ExecuteReader(GetMaxPrimaryKeyQuery(columns)))
            {
                while (reader.Read())
                {
                    var max = reader.GetValue(0);
                    var colName = reader.GetValue<string>(1);
                    var tbName = reader.GetValue<string>(2);

                    var col = columns.FirstOrDefault(c => c.Name.EqualsIgnoreCase(colName) && c.Table.Name == tbName);
                    if (col != null && max.IsNotNull())
                        col.MaxPrimaryKeyValue = max.ChangeType(col.GetRuntimeType());
                }
            }
            return schema;
        }

        #endregion ShemaInfo Methods
    }
}