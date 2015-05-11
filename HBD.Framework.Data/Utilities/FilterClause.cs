using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.Framework.Data.Utilities
{
    public class FilterClause : IFilterClause
    {
        internal FilterClause() { }
        protected FilterClause(string fieldName, CompareOperation operation, object value)
        {
            this.FieldName = fieldName;
            this.Operation = operation;
            this.Value = value;
        }

        public string FieldName { get; set; }
        public CompareOperation Operation { get; set; }
        public object Value { get; set; }
    }
}
