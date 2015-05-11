using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using HBD.Framework.Log;

namespace HBD.Framework.Core
{
    public static class XmlSerializeManager
    {
        private static XmlSerializer GetXmlSerializer(Type objType, Type[] extraTypes = null)
        {
            if (extraTypes != null && extraTypes.Length > 0)
                return new XmlSerializer(objType, extraTypes);
            return new XmlSerializer(objType);
        }

        private static void Serialize(object obj, Stream stream, Type[] extraTypes = null)
        {
            try
            {
                var x = GetXmlSerializer(obj.GetType(), extraTypes);
                x.Serialize(stream, obj);
            }
            catch (Exception ex)
            {
#if !DEBUG
                LogManager.Write(ex);
#else
                throw ex;
#endif
            }
        }

        private static object Deserialize(Type objType, Stream stream, Type[] extraTypes = null)
        {
            try
            {
                var x = GetXmlSerializer(objType, extraTypes);
                return x.Deserialize(stream);
            }
            catch (Exception ex)
            {
#if !DEBUG
                LogManager.Write(ex);
#else
                throw ex;
#endif
            }
            return null;
        }


        public static string Serialize(object obj, Type[] extraTypes = null)
        {
            var writer = new MemoryStream();
            Serialize(obj, writer, extraTypes);
            return Encoding.Default.GetString((writer.ToArray()));
        }

        private static T Deserialize<T>(Stream stream, Type[] extraTypes = null)
        {
            return (T)Deserialize(typeof(T), stream, extraTypes);
        }

        public static object Deserialize(Type objType, string value, Type[] extraTypes = null)
        {
            return Deserialize(objType, new MemoryStream(System.Text.Encoding.UTF8.GetBytes(value)), extraTypes);
        }

        public static T Deserialize<T>(string value, Type[] extraTypes = null)
        {
            return (T)Deserialize(typeof(T), value, extraTypes);
        }
    }
}
