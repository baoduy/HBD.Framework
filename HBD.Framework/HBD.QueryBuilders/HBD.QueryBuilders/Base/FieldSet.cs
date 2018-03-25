#region

using HBD.Framework.Core;

#endregion

namespace HBD.QueryBuilders.Base
{
    public class FieldSet<TQuery> : Field where TQuery : SetQueryBuilder
    {
        private readonly TQuery query;

        internal FieldSet(TQuery query, string fieldName) : base(fieldName)
        {
            Guard.ArgumentIsNotNull(query, nameof(query));
            Guard.ArgumentIsNotNull(fieldName, nameof(fieldName));

            this.query = query;
            FieldName = fieldName;
        }

        internal string FieldName { get; set; }

        public TQuery By(object value)
        {
            query.Sets.Add(FieldName, value);
            return query;
        }
    }
}