#region

using System.IO;
using System.Text;
using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Serialization.Services.Tests
{
    [TestClass]
    public class XmlSerializeServiceTests
    {
        [TestMethod]
        [TestCategory("Fw.Serialization.Xml")]
        public void SerializeTest()
        {
            var service = new XmlSerializeService();
            var str = service.Serialize(new TestItem {Id = 1, Name = "One", Details = "It is number one"});

            Assert.IsTrue(str.IsNotNullOrEmpty());
            Assert.IsTrue(str.StartsWith("<") && str.EndsWith("</TestItem>"));
            Assert.IsTrue(str.Contains("Id") && str.Contains("1"));
            Assert.IsTrue(str.Contains("Name") && str.Contains("One"));
            Assert.IsTrue(str.Contains("Details") && str.Contains("It is number one"));
        }

        [TestMethod]
        [TestCategory("Fw.Serialization.Xml")]
        public void Deserialize_Object_Test()
        {
            var service = new XmlSerializeService();
            var str = service.Serialize(new TestItem {Id = 1, Name = "One", Details = "It is number one"});
            var obj = service.Deserialize(typeof(TestItem), str);

            Assert.IsNotNull(obj);
            Assert.IsTrue(obj.GetValueFromProperty("Id").Equals(1));
            Assert.IsTrue(obj.GetValueFromProperty("Name").Equals("One"));
            Assert.IsTrue(obj.GetValueFromProperty("Details").Equals("It is number one"));
        }

        [TestMethod]
        [TestCategory("Fw.Serialization.Xml")]
        public void Deserialize_Generict_Test()
        {
            var service = new XmlSerializeService();
            var str = service.Serialize(new TestItem {Id = 1, Name = "One", Details = "It is number one"});
            var obj = service.Deserialize<TestItem>(str);

            Assert.IsNotNull(obj);
            Assert.IsTrue(obj.Id == 1);
            Assert.IsTrue(obj.Name == "One");
            Assert.IsTrue(obj.Details == "It is number one");
        }

        [TestMethod]
        [TestCategory("Fw.Serialization.Xml")]
        public void Deserialize_Stream_Test()
        {
            var service = new XmlSerializeService();
            var str = service.Serialize(new TestItem {Id = 1, Name = "One", Details = "It is number one"});
            var obj = service.Deserialize<TestItem>(new MemoryStream(Encoding.UTF8.GetBytes(str)));

            Assert.IsNotNull(obj);
            Assert.IsTrue(obj.GetValueFromProperty("Id").Equals(1));
            Assert.IsTrue(obj.GetValueFromProperty("Name").Equals("One"));
            Assert.IsTrue(obj.GetValueFromProperty("Details").Equals("It is number one"));
        }
    }
}