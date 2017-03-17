#region

using System.Collections;

#endregion

namespace HBD.Framework.Core
{
    public interface IChildrentable<T> where T : ICollection
    {
        T Children { get; }
    }
}