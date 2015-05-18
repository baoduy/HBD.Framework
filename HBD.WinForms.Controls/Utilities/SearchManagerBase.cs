using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Core;
using HBD.Framework.Extension;
using System.Drawing;
using HBD.WinForms.Controls.Core;
using System.Threading;
using HBD.Framework.Data.Utilities;

namespace HBD.WinForms.Controls.Utilities
{
    public abstract class SearchManagerBase<TControl, TItem> : ISearchManager
        where TControl : Control
        where TItem : class
    {
        public SearchManagerBase(TControl control)
        {
            this.Control = control;
            this.Result = new List<TItem>();
            this.Status = SearchStatus.None;
            this.currentIndex = -1;
            this.stopSearching = false;
        }

        protected static readonly object locker = new object();

        private volatile SearchStatus _status = SearchStatus.None;
        protected volatile int currentIndex = -1;
        protected volatile bool stopSearching = false;

        public abstract object CurrentItemValue { get; }
        public TControl Control { get; protected set; }
        public string Keyword { get; set; }
        public TItem CurrentItem
        {
            get
            {
                this.EnsureSearched();
                if (currentIndex == -1)
                    return null;
                if (this.Result.Count == 0)
                    return null;
                return this.Result[currentIndex];
            }
        }
        public SearchStatus Status
        {
            get { return this._status; }
            protected set
            {
                if (this._status != value)
                {
                    this._status = value;
                    this.OnStatusChanged(EventArgs.Empty);
                }
            }
        }
        public int Total { get { return this.Result.Count; } }
        protected IList<TItem> Result { get; set; }
        //protected Thread CurrentThread { get; set; }
        protected Thread CurrentThread { get; set; }

        public event EventHandler StatusChanged;
        protected virtual void OnStatusChanged(EventArgs e)
        {
            if (this.Control.InvokeRequired)
            {
                this.Control.Invoke((Action<EventArgs>)this.OnStatusChanged, e);
                return;
            }

            if (this.Status == SearchStatus.Completed)
                this.HighLightResult();
            if (StatusChanged != null)
                this.StatusChanged(this, e);
        }

        /// <summary>
        /// Highlight item with special color
        /// </summary>
        /// <param name="item"></param>
        protected abstract void HighLightItem(TItem item);
        protected abstract void ClearHighLightItem(TItem item);

        /// <summary>
        /// Set current item or focus to this item on the UI layer.
        /// </summary>
        /// <param name="item">The item will be focused</param>
        /// <returns>The indicate whether item can be focused</returns>
        protected abstract bool SetFocusToItem(TItem item);

        /// <summary>
        /// Implement the search method when search is done call 'OnSearchCompleted'
        /// </summary>
        protected abstract void DoSearch();

        private void HighLightResult()
        {
            foreach (var cell in this.Result)
                this.HighLightItem(cell);
        }

        private void ClearResult()
        {
            if (this.Result.Count > 0)
            {
                foreach (var item in this.Result)
                    this.ClearHighLightItem(item);
                this.Result.Clear();
                this.currentIndex = -1;
            }
        }

        private void ForceStopCurrentThread()
        {
            if (this.CurrentThread != null)
            {
                //Froce to stop the old thread
                try { this.CurrentThread.Abort(); }
                finally { this.CurrentThread = null; }
            }
        }

        private void EnsureSearched()
        {
            if (this.Status == SearchStatus.None)
                throw new Exception("Please call search method first.");
        }

        public virtual bool Next()
        {
            this.EnsureSearched();

            if (this.Result.Count == 0)
                return false;

            if (this.currentIndex < this.Result.Count - 1)
            {
                var item = this.Result[++this.currentIndex];
                //Find next visible item
                while (!this.SetFocusToItem(item) && this.currentIndex < this.Result.Count - 1)
                {
                    if (this.currentIndex == this.Result.Count)
                        return false;
                    item = this.Result[++this.currentIndex];
                }

                return true;
            }
            return false;
        }

        public virtual bool Previous()
        {
            this.EnsureSearched();

            if (this.Result.Count == 0)
                return false;

            if (this.currentIndex > 0)
            {
                var item = this.Result[--this.currentIndex];
                //Find next visible item
                while (!this.SetFocusToItem(item) && this.currentIndex < this.Result.Count - 1)
                {
                    if (this.currentIndex == -1)
                        return false;
                    item = this.Result[--this.currentIndex];

                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clear result and reset the Search
        /// </summary>
        public virtual void Reset()
        {
            this.ForceStopCurrentThread();
            this.Status = SearchStatus.None;
            this.stopSearching = false;
            this.ClearResult();
        }

        public virtual void Search()
        {
            Guard.ArgumentNotNull(this.Control, "DataGridView");
            this.Reset();
            this.Status = SearchStatus.Started;

            //If keyword is empty that mean just clear and reset the Search result.
            if (string.IsNullOrEmpty(this.Keyword))
            {
                this.Status = SearchStatus.Completed;
                return;
            }
            else this.DoSearch();
        }

        public virtual void Search(string keyword)
        {
            this.Keyword = keyword;
            this.Search();
        }

        public virtual void Stop()
        {
            this.stopSearching = true;
        }

        protected virtual void AddResult(TItem item)
        {
            this.Result.Add(item);
            this.Status = SearchStatus.ResultFound;
        }

        public virtual void SetCurrentIndex(object item)
        {
            if (!(item is TItem))
                throw new ArgumentException(string.Format("Item must be an {0} instance", typeof(TItem).Name));
            this.currentIndex = this.Result.IndexOf(item as TItem);
        }
    }
}
