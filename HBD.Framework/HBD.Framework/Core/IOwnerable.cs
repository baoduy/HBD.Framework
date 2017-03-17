namespace HBD.Framework.Core
{
    public interface IOwnerable<out T>
    {
        T Owner { get; }
    }
}