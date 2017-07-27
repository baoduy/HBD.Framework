#region using

using System.Collections.Generic;

#endregion

namespace HBD.Framework.Data.GetSetters
{
    public interface IGetSetterCollection : IEnumerable<IGetSetter>
    {
        IGetSetter Header { get; }
        string Name { get; }
        int Count { get; }
    }
}