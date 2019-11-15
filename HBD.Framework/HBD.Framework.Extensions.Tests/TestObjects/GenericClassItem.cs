namespace HBD.Framework.Extensions.Tests.TestObjects
{
    public abstract class GenericClassItem<T> where T : class
    {



    }

    public sealed class Implemented : GenericClassItem<TestItem>
    {
    }
}
