using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.Test.TestObjects
{
    public class TestAttribute : Attribute { }

    [TestAttribute]
    [Serializable]
    public class HasAttributeTestClass1
    {
        public virtual int Prop0 { get; set; }

        [TestAttribute]
        public virtual string Prop1 { get; set; }
    }

    [Serializable]
    public class HasAttributeTestClass2 : HasAttributeTestClass1 
    {
        public override string Prop1
        {
            get { return "AAA"; }
            set{base.Prop1 = value;}
        }
    }

    public class HasAttributeTestClass3
    {
        public virtual string Prop3 { get; set; }
        public virtual object Prop4 { get; set; }
        public virtual int Prop5 { get; set; }
    }
}
