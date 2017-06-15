#region using

using System;

#endregion

namespace HBD.Framework.Core
{
#if !NETSTANDARD1_6
    public interface ICloneable<out TItem> : ICloneable
    {
        new TItem Clone();
    }
#endif
}