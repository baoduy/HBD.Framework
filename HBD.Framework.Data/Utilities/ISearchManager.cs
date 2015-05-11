using System;
namespace HBD.Framework.Data.Utilities
{
    public interface ISearchManager
    {
        object CurrentItemValue { get; }
        string Keyword { get; set; }
        bool Next();
        bool Previous();
        void Reset();
        void Search();
        void Search(string keyWord);
        void Stop();
        void SetCurrentIndex(object item);

        event EventHandler StatusChanged;
        SearchStatus Status { get; }
        int Total { get; }
    }

    public enum SearchStatus
    {
        None, Started, ResultFound, Completed
    }
}
