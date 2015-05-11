namespace HBD.Framework.Data.Comparison
{
    public class CompareInfo : CompareInfoBase
    {
        public FieldComparison PrimaryField { get; set; }
        public FieldComparisonCollection CompareFields { get; set; }
        public string[] IgnoreStrings { get; set; }

        public CompareInfo()
        {
            this.CompareFields = new FieldComparisonCollection();
        }

        public void PopulateComparisonByColumnNames()
        {
            this.CompareFields = FieldComparisonCollection.AutoPopulateByColumnNames(this.TableA.Columns, this.TableB.Columns, this.PrimaryField);
        }
    }
}
