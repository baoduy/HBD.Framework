#region using

using System.Data;
using System.Diagnostics;
using System.Linq;

#endregion

namespace HBD.Framework.Data.SqlClient.Base
{
    [DebuggerDisplay("Column Name = {Name}, IsPrimaryKey = {IsPrimaryKey}")]
    public class ColumnInfo
    {
        private bool _isNullable = true;
        public int OrdinalPosition { get; set; }
        public string Name { get; set; }
        public SqlDbType DataType { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsComputed { get; set; }
        public object MaxPrimaryKeyValue { get; set; }
        public int MaxLengh { get; set; }
        public TableInfo Table { get; protected internal set; }
        public string ComputedExpression { get; set; }
        public bool IsForeignKey => Table.ForeignKeys.Any(f => f.Column == this);

        public bool IsNullable
        {
            get => !IsPrimaryKey && !IsComputed && _isNullable;
            set => _isNullable = value;
        }

        public override string ToString() => Name;
    }
}