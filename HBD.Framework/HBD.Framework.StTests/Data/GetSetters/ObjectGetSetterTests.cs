using FluentAssertions;
using HBD.Framework.Data.GetSetters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBD.Framework.StTests.Data.GetSetters
{
    [TestClass]
    public class ObjectGetSetterTests
    {
        [TestMethod]
        public void Object_GetSetters_Tests()
        {
            var list = new List<object> {
                new{ Id="1", CreatedOn=DateTime.Now,UpdatedOn=DateTimeOffset.Now, Val=123456},
                new{ Id="2", CreatedOn=DateTime.Now,UpdatedOn=DateTimeOffset.Now, Val=444444}
            };

            var g = new ObjectGetSetterCollection<object>(list);
            g.Header.Count.Should().Be(4);
            g.Count.Should().Be(2);

            foreach (var item in g)
                item.Count.Should().Be(4);
        }
    }
}
