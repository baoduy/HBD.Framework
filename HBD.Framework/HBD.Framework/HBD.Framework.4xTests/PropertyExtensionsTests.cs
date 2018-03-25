#region using

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

#endregion

namespace HBD.Framework.Tests
{
    [TestClass]
    public class PropertyExtensionsTests
    {
        [TestMethod]
        public void GetProperties_Tests()
        {
            new {A = "A"}.GetProperties().Should().NotBeEmpty()
                .And.Subject.ToList()[0].Name.Should().Be("A");
        }
    }
}