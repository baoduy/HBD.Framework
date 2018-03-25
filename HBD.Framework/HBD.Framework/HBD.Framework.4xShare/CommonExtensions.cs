using System;
using System.ComponentModel;
using System.Reflection;

namespace HBD.Framework
{
    public static class CommonExtensions
    {
        public static string GetName(this Enum @this)
        {
            var type = @this.GetType();
            var name = Enum.GetName(type, @this);
            var field = type.GetTypeInfo().GetField(name);

            var attr = field.GetCustomAttribute<DescriptionAttribute>();
            return attr == null ? name.ConsolidateWords() : attr.Description;
        }
    }
}