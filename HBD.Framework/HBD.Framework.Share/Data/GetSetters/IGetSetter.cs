#region using

using System.Collections.Generic;

#endregion

namespace HBD.Framework.Data.GetSetters
{
    public interface IGetSetter : IEnumerable<object>
    {
        object this[string name] { get; set; }
        object this[int index] { get; set; }
        //int Count { get; }
    }
}