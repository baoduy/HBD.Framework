using System;
using System.Data;

namespace HBD.Framework.Data.Comparison
{
    [Serializable]
    public class CompareInfoBase
    {
        DataTable tableA;
        public DataTable TableA
        {
            get { return tableA; }
            set { tableA = value; }
        }

        DataTable tableB;
        public DataTable TableB
        {
            get { return tableB; }
            set { tableB = value; }
        }

        public virtual CompareInfoBase Clone()
        {
            var com = new CompareInfoBase();
            com.TableA = this.TableA.Clone();
            com.TableB = this.TableB.Clone();

            return com;
        }

        public virtual CompareInfoBase Copy()
        {
            var com = new CompareInfoBase();
            com.TableA = this.TableA.Copy();
            com.TableB = this.TableB.Copy();

            return com;
        }
    }
}
