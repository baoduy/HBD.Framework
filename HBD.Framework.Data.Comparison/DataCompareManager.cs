using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using HBD.Framework.Data.Utilities;
using HBD.Framework.Extension;
using System;

namespace HBD.Framework.Data.Comparison
{
    public class DataCompareManager
    {
        private static readonly DatatableFilterRender DatatableFilterRender = new DatatableFilterRender();
        /// <summary>
        /// Find Different Columns between rows based on FieldComparisons
        /// </summary>
        /// <param name="rowA">Data row A</param>
        /// <param name="rowB">Data ros B</param>
        /// <param name="Columns">List Field Comparison</param>
        /// <returns></returns>
        private static FieldComparisonCollection FindDifferentColumns( DataRow rowA, DataRow rowB, IList<FieldComparison> Columns )
        {
            var list = new FieldComparisonCollection();

            foreach ( var col in from col in Columns let valA = rowA[col.FieldA] let valB = rowB[col.FieldB] where !valA.IsEquals( valB ) select col )
            {
                list.Add( col );
            }

            return list;
        }

        /// <summary>
        /// Find the least diffrent Fields between rows
        /// </summary>
        /// <param name="rowA"></param>
        /// <param name="rowsB"></param>
        /// <param name="columnCompares"></param>
        /// <returns></returns>
        private static KeyValuePair<DataRow, FieldComparisonCollection> FindLeastDifferentColumns( DataRow rowA, DataRow[] rowsB, FieldComparisonCollection columnCompares )
        {
            if ( rowsB == null || rowsB.Length == 0 )
                return default( KeyValuePair<DataRow, FieldComparisonCollection> );

            FieldComparisonCollection currentDiffCols = null;
            DataRow currentRowB = null;

            foreach ( var rowB in rowsB )
            {
                var cells = FindDifferentColumns( rowA, rowB, columnCompares );
                if ( cells != null && cells.Count > 0 )
                {
                    if ( currentDiffCols != null && cells.Count >= currentDiffCols.Count ) continue;

                    currentDiffCols = cells;
                    currentRowB = rowB;
                }
                else //Found a row the same with rowA
                {
                    currentDiffCols = null;
                    currentRowB = null;
                    break;
                }
            }

            return new KeyValuePair<DataRow, FieldComparisonCollection>( currentRowB, currentDiffCols );
        }

        private static DataRow[] GetRowsByRow( DataTable tableB, DataRow rowA, FieldComparisonCollection columnCompares )
        {
            IFilterClause filer = null;
            foreach ( var ccol in columnCompares )
            {
                var val = rowA[ccol.FieldA];
                if ( val.IsNullOrEmpty() ) continue;
                if ( val.ToString().Contains( "'" ) ) continue;

                if ( filer == null )
                    filer = FilterManager.Contains( ccol.FieldB, val );
                else filer = filer.AndsWith( FilterManager.Contains( ccol.FieldB, val ) );
            }

            return tableB.Select( DatatableFilterRender.RenderFilter( filer ) );
        }

        private static DataRow[] GetRowsByPrimaryKey( DataTable sourceTable, string keyName, object value )
        {
            IFilterClause filer = FilterManager.IsEquals( keyName, value ); ;
            return sourceTable.Select( DatatableFilterRender.RenderFilter( filer ) );
        }

        private static void ReportProgress( BackgroundWorker worker, int percentProgress, object userState )
        {
            if ( worker != null )
                worker.ReportProgress( percentProgress, userState );
        }
        public static CompareResult Compare( CompareInfo compareInfo, BackgroundWorker worker = null )
        {
            CompareResult result = new CompareResult( compareInfo )
                {
                    OriginalTables = compareInfo,
                    IdenticalTables = compareInfo.Clone(),
                    TableA = compareInfo.TableA.Clone(),
                    TableB = compareInfo.TableB.Clone(),
                };

            if ( string.IsNullOrEmpty( result.TableA.TableName ) )
                result.TableA.TableName = "Table_A";
            if ( string.IsNullOrEmpty( result.TableB.TableName ) )
                result.TableB.TableName = "Table_B";

            if ( result.TableA.TableName == result.TableB.TableName )
            {
                result.TableA.TableName += "_A";
                result.TableB.TableName += "_B";
            }

            ReportProgress( worker, 1, "Starting Comparision." );
            DoCompare( compareInfo, result, worker );

            ReportProgress( worker, 100, "Comparision is done." );
            return result;
        }

        private static void DoCompare( CompareInfo compareInfo, CompareResult result, BackgroundWorker worker = null )
        {
            var tableA = compareInfo.TableA;
            var tableB = compareInfo.TableB;

            var notFoundRowsA = new List<DataRow>();
            var notFoundRowsB = new List<DataRow>();

            var comparedKeys = new List<string>();

            if ( compareInfo.PrimaryField != null && !compareInfo.PrimaryField.IsEmpty() )
            {
                int index = 0;
                var total = tableA.Rows.Count;

                ReportProgress( worker, 2, "Compare with Primary key." );
                #region Compare with Primary key

                foreach ( DataRow rowA in tableA.Rows )
                {
                    index++;
                    ReportProgress( worker, index * 100 / total, string.Format( "Comparing: {0}/{1}.", index, total ) );

                    object AprimaryValue = rowA[compareInfo.PrimaryField.FieldA];

                    if ( AprimaryValue == null || AprimaryValue == DBNull.Value )
                        continue;

                    string priStr = AprimaryValue.ToString().Replace( compareInfo.IgnoreStrings, string.Empty );
                    if ( !string.IsNullOrEmpty( priStr ) )
                        priStr = priStr.Trim().ToLower();

                    //Collect compared keys for not found rows below
                    if ( !comparedKeys.Contains( priStr ) )
                        comparedKeys.Add( priStr );

                    #region Find in TableA
                    var rowsB = GetRowsByPrimaryKey( tableB, compareInfo.PrimaryField.FieldB, AprimaryValue );

                    //Found Row
                    if ( rowsB != null && rowsB.Length > 0 )
                    {
                        var dataRowCell = FindLeastDifferentColumns( rowA, rowsB, compareInfo.CompareFields );

                        if ( dataRowCell.Key != null )
                        {
                            result.TableA.Rows.Add( rowA.ItemArray );
                            result.TableB.Rows.Add( dataRowCell.Key.ItemArray );

                            foreach ( var cell in dataRowCell.Value )
                            {
                                var diff = new DifferenceCell();
                                diff.RowIndex = result.TableA.Rows.Count - 1;
                                diff.ColumnA = cell.FieldA;
                                diff.ColumnB = cell.FieldB;
                                result.DifferenceCells.Add( diff );
                            }
                        }
                        else //there are Identical
                        {
                            result.IdenticalTables.TableA.Rows.Add( rowA.ItemArray );

                            foreach ( var rB in rowsB )
                                result.IdenticalTables.TableB.Rows.Add( rB.ItemArray );
                        }
                    }
                    //Not Found Row
                    else notFoundRowsA.Add( rowA );
                    #endregion
                }

                #region Not found TableB.
                foreach ( DataRow rowB in tableB.Rows )
                {
                    index++;
                    ReportProgress( worker, index * 100 / total, string.Format( "Collect not found items: {0}/{1}.", index, total ) );

                    object priBval = rowB[compareInfo.PrimaryField.FieldB];

                    if ( priBval == null )
                        continue;

                    string priBStr = priBval.ToString().Replace( compareInfo.IgnoreStrings, string.Empty );
                    if ( !string.IsNullOrEmpty( priBStr ) )
                        priBStr = priBStr.Trim().ToLower();

                    if ( comparedKeys.Contains( priBStr ) )
                        continue;

                    notFoundRowsB.Add( rowB );
                }
                #endregion
                #endregion
            }
            else
            {
                ReportProgress( worker, 2, "Compare row by row." );
                #region Comare Row by Row

                int rowsCount = tableA.Rows.Count > tableB.Rows.Count ?
                    tableB.Rows.Count : tableA.Rows.Count;

                var comparedRowsB = new List<DataRow>();

                for ( int i = 0; i < rowsCount; i++ )
                {
                    ReportProgress( worker, i * 100 / rowsCount, string.Format( "Comparing: {0}/{1}.", i, rowsCount ) );

                    DataRow rowA = tableA.Rows[i];

                    var rowsB = GetRowsByRow( tableB, rowA, compareInfo.CompareFields );
                    var rB = FindLeastDifferentColumns( rowA, rowsB, compareInfo.CompareFields );

                    if ( !rB.IsNull() )//Different
                    {
                        result.TableA.Rows.Add( rowA.ItemArray );
                        result.TableB.Rows.Add( rB.Key.ItemArray );

                        foreach ( FieldComparison cell in rB.Value )
                        {
                            DifferenceCell diff = new DifferenceCell();
                            diff.RowIndex = result.TableA.Rows.Count - 1;
                            diff.ColumnA = cell.FieldA;
                            diff.ColumnB = cell.FieldB;

                            result.DifferenceCells.Add( diff );
                        }

                        comparedRowsB.Add( rB.Key );
                    }
                    else if ( rowsB != null && rowsB.Length == 1 )//Rows are Identical
                    {
                        result.IdenticalTables.TableA.Rows.Add( rowA.ItemArray );
                        result.IdenticalTables.TableB.Rows.Add( rowsB[0].ItemArray );
                        comparedRowsB.Add( rowsB[0] );
                    }
                    else //Not Found
                        notFoundRowsA.Add( rowA );
                }

                //Collect Not found row in TableB
                foreach ( DataRow row in tableB.Rows )
                {
                    ReportProgress( worker, 95, "Collecting not found items..." );

                    if ( comparedRowsB.Contains( row ) ) continue;
                    notFoundRowsB.Add( row );
                }
                #endregion
            }

            #region Add Not Found Rows to Results
            result.TableANotFoundRowsIndexs = new List<int>();
            result.TableBNotFoundRowsIndexs = new List<int>();

            foreach ( DataRow row in notFoundRowsA )
            {
                result.TableANotFoundRowsIndexs.Add( result.TableA.Rows.Count );
                result.TableA.Rows.Add( row.ItemArray );
                result.TableB.Rows.Add();

            }

            foreach ( DataRow row in notFoundRowsB )
            {
                result.TableBNotFoundRowsIndexs.Add( result.TableB.Rows.Count );
                result.TableB.Rows.Add( row.ItemArray );
                result.TableA.Rows.Add();
            }
            #endregion
        }
    }
}
