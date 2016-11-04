using System;

namespace HBD.Framework.Exceptions
{
    public class ObjectNullException : Exception
    {
        public string ValueName { get; private set; }

        public ObjectNullException(string valueName) : base($"{valueName} is null or empty.")
        {
            this.ValueName = valueName;
        }
    }
}