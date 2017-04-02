#region using

using System.IO;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace HBD.Framework
{
    public static class JsonExtensions
    {
        public static JObject LoadFrom(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;
            if (!File.Exists(path)) return null;

            var jsonValue = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(jsonValue)) return null;

            try
            {
                //Trim for StartsWith, EndsWith checking purpose.
                jsonValue = jsonValue.Trim();
                //Convert XML to Json
                if (!jsonValue.StartsWith("<") || !jsonValue.EndsWith(">")) return JObject.Parse(jsonValue);

                var doc = new XmlDocument();
                doc.LoadXml(jsonValue);
                jsonValue =
                    JsonConvert.SerializeXmlNode(doc.ChildNodes[0].NodeType == XmlNodeType.XmlDeclaration
                        ? doc.ChildNodes[1]
                        : doc);

                return JObject.Parse(jsonValue);
            }
            catch
            {
                return null;
            }
        }
    }
}