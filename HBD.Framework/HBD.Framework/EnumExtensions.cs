#region using

using System;
using System.Linq;
using HBD.Framework.Attributes;

#endregion

namespace HBD.Framework
{
    public static class EnumExtensions
    {
        public static string ToEnumString(this Enum @this)
        {
            string output;
            @this.TryToEnumString(out output);
            return output;
        }

        /// <summary>
        ///     Try to get EnumStringAttribute.
        ///     return True when EnumStringAttribute found.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryToEnumString(this Enum @this, out string value)
        {
            var att = @this.GetAttribute<EnumStringAttribute>();
            if (att != null)
            {
                value = att.Name;
                return true;
            }
            value = @this.ToString();
            return false;
        }

        public static T ToEnum<T>(this string @this) where T : struct
        {
            var enumField = Enum.GetValues(typeof(T)).Cast<T>().FirstOrDefault(e => e.ToString() == @this);
            return enumField.IsNotDefault() ? enumField : default(T);
        }
    }
}