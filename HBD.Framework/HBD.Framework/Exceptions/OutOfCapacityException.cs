#region

using System;

#endregion

namespace HBD.Framework.Exceptions
{
    public class OutOfCapacityException : Exception
    {
        public OutOfCapacityException() : base($"The value is out of capacity.")
        {
        }

        public OutOfCapacityException(Type type) : base($"The value is out of {type.FullName} capacity.")
        {
        }

        public OutOfCapacityException(int maxLengh) : base($"The value is out of {maxLengh} Lengh.")
        {
        }
    }
}