#region

using System;

#endregion

namespace HBD.Framework.Exceptions
{
    public class DuplicatedException : Exception
    {
        public DuplicatedException() : base($"Item is already existed.")
        {
        }

        public DuplicatedException(object obj) : base($"{obj} is already existed.")
        {
        }
    }
}