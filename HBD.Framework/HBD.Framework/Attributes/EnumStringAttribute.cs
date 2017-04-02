#region using

using System;

#endregion

namespace HBD.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class EnumStringAttribute : Attribute
    {
        public EnumStringAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}