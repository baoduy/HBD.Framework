using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Linq;
using HBD.Framework.Data.Comparison;
using HBD.Framework.Data.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using HBD.Framework.OpenXML;

namespace HBD.WinForms.Controls.Comparison.Helpers
{
    public class DataExporter
    {
        private IDictionary<string, CellStyleCollection> GetCellStyles(CompareResult result)
        {
            var cellStyleA = new CellStyleCollection();
            var cellStyleB = new CellStyleCollection();

            //Excel first row index = 1 and exclude Row header
            foreach (var diff in result.DifferenceCells)
            {
                cellStyleA.Add(diff.RowIndex + 2, result.TableA.Columns.IndexOf(diff.ColumnA), System.Drawing.Color.Empty, System.Drawing.Color.Yellow);
                cellStyleB.Add(diff.RowIndex + 2, result.TableB.Columns.IndexOf(diff.ColumnB), System.Drawing.Color.Empty, System.Drawing.Color.Yellow);
            }

            foreach (var index in result.TableANotFoundRowsIndexs)
                for (var colIndex = 0; colIndex < result.TableA.Columns.Count; colIndex++)
                    cellStyleA.Add(index + 2, colIndex, System.Drawing.Color.Empty, System.Drawing.Color.Red);

            foreach (var index in result.TableBNotFoundRowsIndexs)
                for (var colIndex = 0; colIndex < result.TableB.Columns.Count; colIndex++)
                    cellStyleB.Add(index + 2, colIndex, System.Drawing.Color.Empty, System.Drawing.Color.Red);

            var dic = new Dictionary<string, CellStyleCollection>();
            dic.Add(result.TableA.TableName, cellStyleA);
            dic.Add(result.TableB.TableName, cellStyleB);

            return dic;
        }

        #region Data Table
        public void ExportToXLS(CompareResult result, string fullNameFile)
        {
            using (var spread = OpenXMLHelper.CreateExcelFile(fullNameFile))
            {
                var cellStyles = GetCellStyles(result);

                var sheetA = OpenXMLHelper.AddSheet(spread, result.TableA.TableName);
                OpenXMLHelper.FillDataToWorksheet(sheetA, result.TableA, spread, cellStyles[result.TableA.TableName]);

                var sheetB = OpenXMLHelper.AddSheet(spread, result.TableB.TableName);
                OpenXMLHelper.FillDataToWorksheet(sheetB, result.TableB, spread, cellStyles[result.TableB.TableName]);

                spread.WorkbookPart.Workbook.Save();
            }
        }
        #endregion

        //#region DataGridView
        //public void ExportToXLS(DataGridView[] dataGrids, string fullNameFile)
        //{
        //    using (var spread = OpenXMLHelper.CreateExcelFile(fullNameFile))
        //    {
        //        foreach (DataGridView grid in dataGrids)
        //        {
        //            var sheetA = OpenXMLHelper.AddSheet(spread, grid.Name);
        //            PlushDataToSheet(grid, sheetA, spread);
        //        }
        //        spread.WorkbookPart.Workbook.Save();
        //    }
        //}

        //private void PlushDataToSheet(DataGridView dataGrid, WorksheetPart workSheet, SpreadsheetDocument spreedsheet)
        //{
        //    var data = ((DataView)dataGrid.DataSource).ToTable();
        //    OpenXMLHelper.FillDataToWorksheet(data, workSheet);

        //    var rows = workSheet.Worksheet.Descendants<Row>().ToList();

        //    foreach (DataGridViewRow row in dataGrid.Rows)
        //    {
        //        var col = dataGrid.Columns.GetFirstColumn(DataGridViewElementStates.None);
        //        do
        //        {
        //            if (col.Visible)
        //            {
        //                DataGridViewCell cell = row.Cells[col.Name];
        //                if (!cell.Style.BackColor.IsEmpty)
        //                {
        //                    rows[cell.RowIndex].Descendants<Cell>().ToList()[cell.ColumnIndex].StyleIndex = OpenXMLHelper.AddNewCellFormat(spreedsheet, cell.Style.BackColor);
        //                }
        //                else if (!cell.OwningRow.DefaultCellStyle.BackColor.IsEmpty)
        //                {
        //                    rows[cell.RowIndex].Descendants<Cell>().ToList()[cell.ColumnIndex].StyleIndex = OpenXMLHelper.AddNewCellFormat(spreedsheet, cell.OwningRow.DefaultCellStyle.BackColor);
        //                }
        //            }
        //            col = dataGrid.Columns.GetNextColumn(col, DataGridViewElementStates.None, DataGridViewElementStates.None);

        //        } while (col != null);
        //    }
        //}
        //#endregion
    }
}
