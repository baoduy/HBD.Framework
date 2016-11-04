using HBD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.Test.TestObjects
{
    public class NotifyPropertyChangedObject : NotifyPropertyChange
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { this.SetValue(ref name, value); }
        }

        private TestItem item;

        public TestItem Item
        {
            get
            {
                return item;
            }

            set
            {
                this.SetValue(ref item, value);
            }
        }
    }
}