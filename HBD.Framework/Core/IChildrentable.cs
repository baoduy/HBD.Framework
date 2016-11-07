using System.Collections;

namespace HBD.Framework.Core
{
    public interface IChildrentable<T> where T : ICollection
    {
        T Children { get; }
    }
}