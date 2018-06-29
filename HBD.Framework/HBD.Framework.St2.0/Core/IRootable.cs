namespace HBD.Framework.Core
{
    public interface IRootable<out T>
    {
        T Root { get; }
    }
}