#region

using System;
using System.IO;

#endregion

namespace HBD.Framework.Serialization.Services
{
    public interface ISerializeService
    {
        string Serialize(object obj, params Type[] extraTypes);

        T Deserialize<T>(Stream stream, params Type[] extraTypes);

        object Deserialize(Type objType, string value, params Type[] extraTypes);

        T Deserialize<T>(string value, params Type[] extraTypes);
    }
}