using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Core;
using HBD.Framework.Extension;
using System.Drawing;
using HBD.WinForms.Controls.Core;
using HBD.Framework.Log;
using System.Threading;
using HBD.Framework.Data.Utilities;

namespace HBD.WinForms.Controls.Utilities
{
    public class DataGridViewSearchManager : SearchManagerBase<DataGridView, DataGridViewCell>
    {
        public DataGridViewSearchManager(DataGridView grid) : base(grid) { }

        public override object CurrentItemValue
        {
            get
            {
                if (this.CurrentItem != null)
                    return this.CurrentItem.Value;
                return null;
            }
        }

        protected override void HighLightItem(DataGridViewCell cell)
        {
            cell.Style.BackColor = Color.Yellow;
        }

        protected override void ClearHighLightItem(DataGridViewCell cell)
        {
            cell.Style.BackColor = Color.Empty;
        }

        protected override void DoSearch()
        {
            //Start new thread
            this.CurrentThread = BackgroundThreadManager.StartThread(() =>
            {
                lock (locker)
                {
                    try
                    {
                        foreach (DataGridViewRow row in this.Control.Rows)
                        {
                            if (!row.Visible)
                                continue;

                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                var val = cell.Value;
                                if (val.IsContains(Keyword))
                                    this.AddResult(cell);

                                //Break the cell loop
                                if (this.stopSearching) break;
                            }

                            //Break the row loop
                            if (this.stopSearching) break;
                        }
                    }
                    catch (Exception ex) { LogManager.Write(ex); }
                    finally
                    {
                        this.Status = SearchStatus.Completed;
                    }
                }//End lock
            });
        }

        protected override bool SetFocusToItem(DataGridViewCell item)
        {
            if (!item.Visible)
                return false;

            this.Control.CurrentCell = item;
            return true;
        }
    }
}
