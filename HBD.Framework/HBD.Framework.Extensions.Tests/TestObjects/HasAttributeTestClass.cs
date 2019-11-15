using System;

namespace HBD.Framework.Extensions.Tests.TestObjects
{
    [Test]
    public class HasAttributeTestClass1
    {
        #region Public Properties

        public virtual int Prop0 { get; set; }

        [Test] public virtual string Prop1 { get; set; }

        #endregion Public Properties
    }

    public class HasAttributeTestClass2 : HasAttributeTestClass1
    {
        #region Public Properties

        public override string Prop1
        {
            get => "AAA";
            set => base.Prop1 = value;
        }

        #endregion Public Properties
    }

    public class HasAttributeTestClass3
    {
        #region Public Properties

        public virtual string Prop3 { get; set; }
        public virtual object Prop4 { get; set; }
        public virtual int Prop5 { get; set; }

        #endregion Public Properties
    }

    public class TestAttribute : Attribute
    {
    }
}