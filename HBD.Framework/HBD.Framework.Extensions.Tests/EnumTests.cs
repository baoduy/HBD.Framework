using System.Linq;
using FluentAssertions;
using HBD.Framework.Extensions.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Framework.Extensions.Tests
{
    [TestClass]
    public class EnumExtensionsTests
    {
        //[TestMethod]
        //public void GetAttribute()
        //{
        //    HBDEnum.DescriptionEnum.GetAttribute<System.ComponentModel.DisplayAttribute>()
        //        .Should().BeOfType<System.ComponentModel.DescriptionAttribute>();
        //}

        [TestMethod]
        public void TestGetEnumInfo()
        {
            HBDEnum.DescriptionEnum.GetEumInfo().Name.Should().Be("HBD");
        }

        [TestMethod]
        public void TestGetEnumInfos()
        {
            var list = EnumExtensions.GetEumInfos<HBDEnum>().ToList();
            list.Count.Should().Be(3);
        }
    }
}
