#region

using System;
using System.Text;

#endregion

namespace HBD.Framework.Exceptions
{
    public class InvalidException : Exception
    {
        public InvalidException(params string[] names) : base(BuildMessage(names))
        {
        }

        private static string BuildMessage(params string[] names)
        {
            var builder = new StringBuilder();
            if (names.NotAnyItem()) builder.Append("Value");
            else
                for (var i = 0; i < names.Length; i++)
                {
                    if (builder.Length > 0)
                        builder.Append(i < names.Length - 1 ? ", " : " and ");
                    builder.Append(names[i]);
                }

            return builder.Append(names.Length > 1 ? " are" : " is").Append(" invalid.").ToString();
        }
    }
}