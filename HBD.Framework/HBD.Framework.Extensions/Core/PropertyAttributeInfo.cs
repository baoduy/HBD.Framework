using System;
using System.Reflection;

namespace HBD.Framework.Extensions.Core
{
    public class PropertyAttributeInfo<TAttribute> where TAttribute : Attribute
    {
        #region Public Properties

        public TAttribute Attribute { get; protected internal set; }
        public PropertyInfo PropertyInfo { get; protected internal set; }

        #endregion Public Properties
    }
}