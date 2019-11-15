using HBD.Framework.Extensions.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Framework.Extensions.Tests
{
    [TestClass]
    public class PropertyExtensionsTests
    {
        #region Public Methods

        [TestMethod]
        public void Get_Private_Property_Value()
        {
            Assert.IsNotNull(new TestItem3("Duy").PropertyValue("PrivateObj"));
        }

        [TestMethod]
        public void Get_Protected_Property_Value()
        {
            Assert.IsNotNull(new TestItem3("Duy").PropertyValue("ProtectedObj"));
        }

        [TestMethod]
        public void Get_Public_Property_Value_IgnoreCase()
        {
            Assert.IsNotNull(new TestItem3("Duy").PropertyValue("name"));
        }

        [TestMethod]
        public void Set_Private_Property_Value()
        {
            var t = new TestItem3("Duy");

            t.SetPropertyValue("PrivateObj", "Duy");

            Assert.AreEqual(t.PropertyValue("PrivateObj"), "Duy");
        }

        [TestMethod]
        public void Set_Protected_Property_Value()
        {
            var t = new TestItem3("Duy");

            t.SetPropertyValue("ProtectedObj", "Duy");

            Assert.AreEqual(t.PropertyValue("ProtectedObj"), "Duy");
        }

        [TestMethod]
        public void Set_Public_Property_Value_IgnoreCase()
        {
            var t = new TestItem3("Duy");

            t.SetPropertyValue("Description", "Duy");

            Assert.AreEqual(t.Description, "Duy");
        }

        #endregion Public Methods
    }
}