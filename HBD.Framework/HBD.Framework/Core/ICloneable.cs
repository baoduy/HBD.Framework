#region using

using System;

#endregion

namespace HBD.Framework.Core
{
    public interface ICloneable<out TItem> : ICloneable
    {
        new TItem Clone();
    }
}