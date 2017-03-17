#region

using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

#endregion

namespace HBD.Framework.Serialization.Services
{
    public class XmlSerializeService : BaseSerializeService
    {
        private static XmlSerializer GetXmlSerializer(Type objType, params Type[] extraTypes)
            => extraTypes != null && extraTypes.Length > 0
                ? new XmlSerializer(objType, extraTypes)
                : new XmlSerializer(objType);

        public override string Serialize(object obj, params Type[] extraTypes)
        {
            var writer = new MemoryStream();
            GetXmlSerializer(obj.GetType(), extraTypes).Serialize(writer, obj);
            return Encoding.Default.GetString(writer.ToArray());
        }

        public override object Deserialize(Type objType, string value, params Type[] extraTypes)
        {
            var reader = new MemoryStream(Encoding.UTF8.GetBytes(value));
            return GetXmlSerializer(objType, extraTypes).Deserialize(reader);
        }
    }
}