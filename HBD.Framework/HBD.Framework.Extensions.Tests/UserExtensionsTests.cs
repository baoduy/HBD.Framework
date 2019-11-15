using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Framework.Extensions.Tests
{
    [TestClass]
    public class UserExtensionsTests
    {
        [TestMethod]
        public void TestWithoutDomain()
        {
            "duy".WithoutDomain().Should().Be("duy");
            "\\duy".WithoutDomain().Should().Be("duy");
            "bao\\".WithoutDomain().Should().Be("");
            "bao\\duy".WithoutDomain().Should().Be("duy");

            "duy@bao.net".WithoutDomain().Should().Be("duy");
            "@duy".WithoutDomain().Should().Be("");
            "duy@".WithoutDomain().Should().Be("duy");

            "".WithoutDomain().Should().Be("");
            (null as string).WithoutDomain().Should().BeNull();
        }
    }
}
