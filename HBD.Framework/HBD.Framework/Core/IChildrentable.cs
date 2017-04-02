#region using

using System.Collections;

#endregion

namespace HBD.Framework.Core
{
    public interface IChildrentable<out T> where T : ICollection
    {
        T Children { get; }
    }
}