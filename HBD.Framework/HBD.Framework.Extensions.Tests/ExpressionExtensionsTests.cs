using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using HBD.Framework.Extensions.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Framework.Extensions.Tests
{
    [TestClass]
    public class ExpressionExtensionsTests
    {
        [TestMethod]
        public void ExtractProperty()
        {
            Expression<Func<TestItem, object>> ex = t => t.Name;

            ex.ExtractProperty().Name.Should().Be(nameof(TestItem.Name));
        }


        [TestMethod]
        public void Expression_And()
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Expression<Func<int, bool>> left = i => i > 5;
            Expression<Func<int, bool>> right = i => i < 7;

            var and = left.And(right);

            list.First(and.Compile()).Should().Be(6);

        }

        [TestMethod]
        public void Expression_Or()
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Expression<Func<int, bool>> left = i => i == 5;
            Expression<Func<int, bool>> right = i => i == 7;

            var and = left.Or(right);

            list.Where(and.Compile()).Should().HaveCount(2);

        }

        [TestMethod]
        public void Expression_Not()
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Expression<Func<int, bool>> left = i => i <10;


            list.First(left.NotMe().Compile()).Should().Be(10);

        }
    }
}
