namespace HBD.Framework.Core
{
    public interface IParentable<out T>
    {
        T Parent { get; }
    }
}