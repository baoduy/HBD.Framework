using System;

namespace HBD.Framework.Exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public string ValueName { get; private set; }

        public ObjectNotFoundException(string valueName) : base($"{valueName} is not found.")
        {
            this.ValueName = valueName;
        }

        public ObjectNotFoundException(string valueName, string inListName) : base($"{valueName} is not found in {inListName}.")
        {
            this.ValueName = valueName;
        }
    }
}