using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.Framework.Data.Utilities
{
    public class BinaryFilterItem : IFilterClause
    {
        internal BinaryFilterItem() { }
        protected BinaryFilterItem(IFilterClause leftClause, BinaryOperation operation, IFilterClause rightClause)
        {
            this.LeftClause = leftClause;
            this.Operation = operation;
            this.RightClause = rightClause;
        }

        public IFilterClause LeftClause { get; set; }
        public BinaryOperation Operation { get; set; }
        public IFilterClause RightClause { get; set; }
    }
}
