using System;
using System.Collections.Generic;

namespace HBD.Framework.Data.Comparison
{
    [Serializable]
    public class CompareResult : CompareInfoBase
    {
        public CompareInfo CompareInfo { get; private set; }
        private bool IsDifferenceRowsOnly { get; set; }
        private bool IsNotFoundRowsOnly { get; set; }

        public CompareInfoBase OriginalTables { get; set; }
        public CompareInfoBase IdenticalTables { get; set; }
        public IList<DifferenceCell> DifferenceCells { get; set; }
        public IList<int> TableANotFoundRowsIndexs { get; set; }
        public IList<int> TableBNotFoundRowsIndexs { get; set; }

        public CompareResult(CompareInfo compareInfo)
        {
            this.CompareInfo = compareInfo;
            this.IsDifferenceRowsOnly = false;
            this.IsNotFoundRowsOnly = false;
            this.DifferenceCells = new List<DifferenceCell>();
            this.TableANotFoundRowsIndexs = new List<int>();
            this.TableBNotFoundRowsIndexs = new List<int>();
        }

        public CompareResult GetDifferenceRowsOnly()
        {
            if (this.IsDifferenceRowsOnly)
                return this;

            var result = new CompareResult(this.CompareInfo)
            {
                IsDifferenceRowsOnly = true,
                OriginalTables = this.OriginalTables,
                TableA = this.TableA.Clone(),
                TableB = this.TableB.Clone()
            };

            foreach (DifferenceCell cell in this.DifferenceCells)
            {
                result.TableA.Rows.Add(this.TableA.Rows[cell.RowIndex].ItemArray);
                result.TableB.Rows.Add(this.TableB.Rows[cell.RowIndex].ItemArray);

                DifferenceCell newCell = new DifferenceCell();
                newCell.ColumnA = cell.ColumnA;
                newCell.ColumnB = cell.ColumnB;
                newCell.RowIndex = result.TableA.Rows.Count - 1;

                result.DifferenceCells.Add(newCell);
            }

            return result;
        }

        public CompareResult GetNotFoundRowsOnly()
        {
            if (this.IsNotFoundRowsOnly)
                return this;

            var result = new CompareResult(this.CompareInfo)
            {
                IsNotFoundRowsOnly = true,
                OriginalTables = this.OriginalTables,
                TableA = this.TableA.Clone(),
                TableB = this.TableB.Clone()
            };
            
            foreach (int i in this.TableANotFoundRowsIndexs)
            {
                result.TableA.Rows.Add(this.TableA.Rows[i].ItemArray);
                result.TableB.Rows.Add();

                result.TableANotFoundRowsIndexs.Add(result.TableA.Rows.Count - 1);
            }

            foreach (int i in this.TableBNotFoundRowsIndexs)
            {
                result.TableA.Rows.Add();
                result.TableB.Rows.Add(this.TableB.Rows[i].ItemArray);

                result.TableBNotFoundRowsIndexs.Add(result.TableB.Rows.Count - 1);
            }

            return result;
        }
    }
}
