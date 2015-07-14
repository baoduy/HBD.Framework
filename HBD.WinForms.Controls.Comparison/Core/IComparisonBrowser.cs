using System;
namespace HBD.WinForms.Controls.Comparison.Core
{
    public interface IComparisonBrowser
    {
        //System.Collections.Generic.IList<System.Data.DataColumn> ListColumnA { get; }
        //System.Collections.Generic.IList<System.Data.DataColumn> ListColumnB { get; }
        string OriginalSourceA { get; }
        string OriginalSourceB { get; }
        event EventHandler<HBD.WinForms.Controls.Comparison.Events.FileSelectedEventArgs> SelectionChanged;
        System.Data.DataTable TableA { get; }
        System.Data.DataTable TableB { get; }
    }
}
