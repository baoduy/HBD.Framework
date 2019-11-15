#region using

using System.Collections.Generic;
using System.Text;
using HBD.Framework.Extensions;

#endregion

namespace HBD.Framework
{
    public static class ReplacementExtensions
    {
        public static string Replace(this object @this, IDictionary<string, string> replaceItems)
        {
            if (@this == null) return string.Empty;
            var str = @this.ToString();
            if (replaceItems?.NotAny() == true) return str;

            //If the string huge then using String builder.
            if (replaceItems.Count > 10 && str.Length > byte.MinValue)
            {
                var strBuild = new StringBuilder(str);

                foreach (var item in replaceItems)
                    if (item.Key.IsNullOrEmpty())
                    {
                        if (strBuild.Length <= 0) strBuild.Append(item.Value);
                    }
                    else
                    {
                        strBuild = strBuild.Replace(item.Key, item.Value);
                    }
                return strBuild.ToString();
            }
            foreach (var item in replaceItems)
                if (item.Key.IsNullOrEmpty())
                {
                    if (str.IsNullOrEmpty()) str = item.Value;
                }
                else
                {
                    str = str.Replace(item.Key, item.Value);
                }
            return str;
        }
    }
}