#region using

using System;
using HBD.Framework.Attributes;

#endregion

namespace HBD.Framework.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("The 'object' is not found.")
        {
        }

        public NotFoundException([NotNull] string name) : base($"The '{name}' is not found.")
        {
        }

        public NotFoundException([NotNull] string[] names) : base($"The '{string.Join(",", names)}' are not found.")
        {
        }
    }
}