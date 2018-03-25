#region using

using System;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Data.SqlClient.Base
{
    /// <summary>
    ///     As the name of each table in SQL has 2 element is Schema Name and Table Name.
    ///     And there are many name format were accepted: [ShemaName].[TableName], ShemaName.TableName, [TableName] and
    ///     dbo.TableName as dbo is default Shema Name.
    ///     So when compare 2 table name it has to be compare both Shema Name and Table Name together.
    ///     This class had been defined to standardize the name for both that elements to easier to compare and the standard
    ///     format is [ShemaName].[TableName].
    /// </summary>
    public sealed class DbName : IEquatable<DbName>, IEquatable<string>
    {
        private const string DefaultSchema = "dbo";

        public DbName(string name) : this(DefaultSchema, name)
        {
        }

        public DbName(string schema, string name)
        {
            if (schema.IsNullOrEmpty()) schema = DefaultSchema;

            schema = Common.RemoveSqlBrackets(schema);
            name = Common.RemoveSqlBrackets(name);

            Guard.ArgumentIsNotNull(name, nameof(name));

            Schema = schema;
            Name = name;
        }

        public string Schema { get; }
        public string Name { get; }
        public string FullName => Common.GetFullName(Schema, Name);

        public override string ToString() => FullName;

        public static DbName Parse(string fullName)
        {
            if (fullName.IsNullOrEmpty()) return null;
            string schema = null;
            string name;

            if (fullName.Contains("."))
            {
                var splited = fullName.Split('.');
                if (splited.Length <= 0) return null;
                if (splited.Length == 1)
                {
                    name = splited[0];
                }
                else
                {
                    schema = splited[0];
                    name = splited[1];
                }
            }
            else
            {
                name = fullName;
            }

            return name.IsNullOrEmpty() ? null : new DbName(schema, name);
        }

        /// <summary>
        ///     Compare TableName with object.
        ///     Object should me string or TableName.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is DbName name)
                return Equals(name);

            if (obj is string s)
                return Equals(s);
            return false;
        }

        //Two objects that are equal return hash codes that are equal. However, the reverse is not true.
        public override int GetHashCode() => Schema.GetHashCode() + Name.GetHashCode();

        public bool Equals(DbName other)
        {
            return this.FullName.EqualsIgnoreCase(other?.FullName);
        }

        public bool Equals(string other)
        {
            return Equals(Parse(other));
        }

        public static implicit operator string(DbName name) => name.FullName;

        public static implicit operator DbName(string fullName) => Parse(fullName);

        public static bool operator ==(DbName tbA, object tbB) => tbA?.Equals(tbB) ?? false;

        public static bool operator !=(DbName tbA, object tbB) => !tbA?.Equals(tbB) ?? false;
    }
}