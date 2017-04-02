#region using

using System.IO;
using System.Net.Fakes;
using System.Threading.Tasks;
using HBD.Framework.Net.Adapters;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Net.Helpers.Tests
{
    [TestClass]
    public class GravatarHelperTests
    {
        [TestMethod]
        [TestCategory("Fw.Net.Helpers")]
        public void GetUrlTest()
        {
            var g = new GravatarAdapter("baoduy2412@yahoo.com", 600);
            var u = g.GetUrl();
            Assert.IsNotNull(u);
            Assert.IsTrue(u.PathAndQuery.Contains("512"));
        }

        [TestMethod]
        [TestCategory("Fw.Net.Helpers")]
        public void GetImageTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpWebRequest.AllInstances.GetResponse = w
                    => new StubWebResponse
                    {
                        GetResponseStream01 = () => File.OpenRead("TestData\\baoduy2412@yahoo.com.png")
                    };

                var g = new GravatarAdapter("baoduy2412@yahoo.com", -1);
                var i = g.GetImage();
                Assert.IsNotNull(i);
                Assert.IsTrue(i.Size.Height > 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Net.Helpers")]
        public async Task GetImageAsyncTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpWebRequest.AllInstances.EndGetResponseIAsyncResult = (w, ia)
                    => new StubWebResponse
                    {
                        GetResponseStream01 = () => File.OpenRead("TestData\\baoduy2412@yahoo.com.png")
                    };

                var g = new GravatarAdapter("baoduy2412@yahoo.com", 128);
                var i = await g.GetImageAsync();
                Assert.IsNotNull(i);
                Assert.IsTrue(i.Size.Width > 1);
            }
        }
    }
}