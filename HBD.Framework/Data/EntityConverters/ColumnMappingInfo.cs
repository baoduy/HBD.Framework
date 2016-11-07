using System.Reflection;

namespace HBD.Framework.Data.EntityConverters
{
    internal class ColumnMappingInfo
    {
        public ColumnMappingInfo(PropertyInfo propertyInfo, string fieldName)
        {
            this.PropertyInfo = propertyInfo;
            this.FieldName = fieldName;
        }

        public string PropertyName => PropertyInfo.Name;
        public string FieldName { get; }
        internal PropertyInfo PropertyInfo { get; }
    }
}