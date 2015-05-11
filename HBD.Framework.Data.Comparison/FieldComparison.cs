using System;

namespace HBD.Framework.Data.Comparison
{
    [Serializable]
    public class FieldComparison
    {
        public string FieldA { get; set; }
        public string FieldB { get; set; }

        public FieldComparison() { }
        public FieldComparison(string fieldA, string fieldB)
        {
            this.FieldA = fieldA;
            this.FieldB = fieldB;
        }

        public virtual bool IsEmpty()
        {
            return string.IsNullOrEmpty(this.FieldA) && string.IsNullOrEmpty(this.FieldB);
        }

        //public FieldComparison Copy()
        //{ return new FieldComparison(this.FieldA, this.FieldB); }
    }
}
