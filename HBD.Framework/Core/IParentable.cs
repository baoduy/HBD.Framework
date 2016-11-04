namespace HBD.Framework.Core
{
    public interface IParentable<T>
    {
        T Parent { get; }
    }
}