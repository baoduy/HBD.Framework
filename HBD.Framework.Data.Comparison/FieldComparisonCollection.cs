using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HBD.Framework.Core;
using HBD.Framework.Extension;

namespace HBD.Framework.Data.Comparison
{
    [Serializable]
    public class FieldComparisonCollection : List<FieldComparison>
    {
        public FieldComparisonCollection() { }
        public FieldComparisonCollection(IEnumerable<FieldComparison> collection)
        {
            this.AddRange(collection);
        }

        public FieldComparison GetCompareField(string fieldA, string fieldB)
        {
            Guard.ArgumentNotNull(fieldA, "fieldA");
            Guard.ArgumentNotNull(fieldB, "fieldB");

            if (string.IsNullOrEmpty(fieldA))
                return this.FirstOrDefault(f => f.FieldB == fieldB);
            if (string.IsNullOrEmpty(fieldB))
                return this.FirstOrDefault(f => f.FieldA == fieldA);

            return this.FirstOrDefault(f => f.FieldA == fieldA && f.FieldB == fieldB);
        }

        public string GetFieldABy(string fieldB)
        {
            FieldComparison col = GetCompareField(null, fieldB);
            if (col != null)
                return col.FieldA;

            return null;
        }

        public string GetFieldBBy(string fieldA)
        {
            FieldComparison col = GetCompareField(fieldA, null);
            if (col != null)
                return col.FieldA;

            return null;
        }

        public ReadOnlyCollection<string> GetAllFieldA()
        {
            return this.Select(f => f.FieldA).ToList().AsReadOnly();
        }

        public ReadOnlyCollection<string> GetAllFieldB()
        {
            return this.Select(f => f.FieldB).ToList().AsReadOnly();
        }

        public new bool Contains(FieldComparison field)
        {
            var found = this.FirstOrDefault(f => f.FieldA == field.FieldA && f.FieldB == field.FieldB);
            return found != null;
        }

        public void Add(string fieldA, string fieldB)
        {
            this.Add(new FieldComparison(fieldA, fieldB));
        }

        public new void Add(FieldComparison item)
        {
            if (item == null)
                return;
            if (this.FirstOrDefault(f => f.FieldA == item.FieldA && f.FieldB == item.FieldB) != null)
                return;

            base.Add(item);
        }

        public new void AddRange(IEnumerable<FieldComparison> collection)
        {
            foreach (var item in collection)
                this.Add(item);
        }

        public static FieldComparisonCollection AutoPopulateByColumnNames(ColumnNamesCollection columnsA, ColumnNamesCollection columnsB, FieldComparison primaryKey = null)
        {
            var fields = new FieldComparisonCollection();

            if (columnsA == null || columnsA.Count == 0
                || columnsB == null || columnsB.Count == 0)
                return fields;

            foreach (var colA in columnsA)
            {
                foreach (var colB in columnsB)
                {
                    if (primaryKey != null
                        && colA == primaryKey.FieldA
                        && colB == primaryKey.FieldB)
                        continue;

                    var basicA = colA.GetAlphabetCharacters();
                    var basicB = colB.GetAlphabetCharacters();

                    if (string.IsNullOrEmpty(basicA) || string.IsNullOrEmpty(basicB))
                        continue;

                    if (basicA.StartsWith(basicB, StringComparison.CurrentCultureIgnoreCase)
                        || basicB.StartsWith(basicA, StringComparison.CurrentCultureIgnoreCase))
                    {
                        fields.Add(colA, colB);
                        break;
                    }
                }
            }

            return fields;
        }
    }
}
