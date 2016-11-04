using System.Collections.Generic;

namespace HBD.Framework.Data.GetSetters
{
    public interface IGetSetterCollection : IEnumerable<IGetSetter>
    {
        IGetSetter Header { get; }
        string Name { get; }
    }
}