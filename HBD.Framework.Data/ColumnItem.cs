using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.Framework.Data
{
    public class ColumnItem
    {
        public string Name { get; set; }
        public Type DataType { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
