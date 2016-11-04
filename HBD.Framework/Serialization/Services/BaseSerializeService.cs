using System;
using System.IO;

namespace HBD.Framework.Serialization.Services
{
    public abstract class BaseSerializeService : ISerializeService
    {
        public abstract object Deserialize(Type objType, string value, params Type[] extraTypes);

        public abstract string Serialize(object obj, params Type[] extraTypes);

        public T Deserialize<T>(string value, params Type[] extraTypes)
            => (T)Deserialize(typeof(T), value, extraTypes);

        public T Deserialize<T>(Stream stream, params Type[] extraTypes)
            => Deserialize<T>(new StreamReader(stream).ReadToEnd());
    }
}