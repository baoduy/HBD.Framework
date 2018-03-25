#region

using HBD.Data.Comparisons;
using HBD.Data.Comparisons.Base;
using HBD.Framework.Core;

#endregion

namespace HBD.QueryBuilders.Base
{
    public class Field : FieldBase
    {
        internal Field(string fieldName)
        {
            Guard.ArgumentIsNotNull(fieldName, nameof(fieldName));
            Name = fieldName;
        }

        public string Name { get; }

        public ValueCondition Equals<T>(T value)
            => new ValueCondition(Name, CompareOperation.Equals, value);

        public ValueCondition NotEquals<T>(T value)
            => new ValueCondition(Name, CompareOperation.NotEquals, value);

        public ValueCondition GreaterThan<T>(T value)
            => new ValueCondition(Name, CompareOperation.GreaterThan, value);

        public ValueCondition LessThan<T>(T value)
            => new ValueCondition(Name, CompareOperation.LessThan, value);

        public ValueCondition GreaterThanOrEquals<T>(T value)
            => new ValueCondition(Name, CompareOperation.GreaterThanOrEquals, value);

        public ValueCondition LessThanOrEquals<T>(T value)
            => new ValueCondition(Name, CompareOperation.LessThanOrEquals, value);

        #region Field

        public FieldCondition Equals(Field compareField)
            => EqualsField(compareField.Name);

        public FieldCondition EqualsField(string compareFieldName)
            => new FieldCondition(Name, CompareOperation.Equals, compareFieldName);

        public FieldCondition NotEquals(Field compareField)
            => NotEqualsField(compareField.Name);

        public FieldCondition NotEqualsField(string compareFieldName)
            => new FieldCondition(Name, CompareOperation.NotEquals, compareFieldName);

        public FieldCondition GreaterThan(Field compareField)
            => GreaterThanField(compareField.Name);

        public FieldCondition GreaterThanField(string compareFieldName)
            => new FieldCondition(Name, CompareOperation.GreaterThan, compareFieldName);

        public FieldCondition LessThan(Field compareField)
            => LessThanField(compareField.Name);

        public FieldCondition LessThanField(string compareFieldName)
            => new FieldCondition(Name, CompareOperation.LessThan, compareFieldName);

        public FieldCondition GreaterThanOrEquals(Field compareField)
            => GreaterThanOrEqualsField(compareField.Name);

        public FieldCondition GreaterThanOrEqualsField(string compareFieldName)
            => new FieldCondition(Name, CompareOperation.GreaterThanOrEquals, compareFieldName);

        public FieldCondition LessThanOrEquals(Field compareField)
            => LessThanOrEqualsField(compareField.Name);

        public FieldCondition LessThanOrEqualsField(string compareFieldName)
            => new FieldCondition(Name, CompareOperation.LessThanOrEquals, compareFieldName);

        #endregion Field
    }
}