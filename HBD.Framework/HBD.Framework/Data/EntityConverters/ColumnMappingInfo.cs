#region

using System.Reflection;

#endregion

namespace HBD.Framework.Data.EntityConverters
{
    internal class ColumnMappingInfo
    {
        public ColumnMappingInfo(PropertyInfo propertyInfo, string fieldName)
        {
            PropertyInfo = propertyInfo;
            FieldName = fieldName;
        }

        public string PropertyName => PropertyInfo.Name;
        public string FieldName { get; }
        internal PropertyInfo PropertyInfo { get; }
    }
}