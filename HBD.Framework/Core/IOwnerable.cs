namespace HBD.Framework.Core
{
    public interface IOwnerable<T>
    {
        T Owner { get; }
    }
}