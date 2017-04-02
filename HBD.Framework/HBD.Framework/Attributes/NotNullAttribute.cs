#region using

using System;

#endregion

namespace HBD.Framework.Attributes
{
    /// <summary>
    ///     Indicates that the value of the marked element could never be <c>null</c>
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter |
        AttributeTargets.Property | AttributeTargets.Delegate |
        AttributeTargets.Field)]
    public sealed class NotNullAttribute : Attribute
    {
    }
}