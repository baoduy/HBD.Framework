using HBD.Framework.Core;
using System.Collections.Generic;

namespace HBD.Framework.Data.DataReplacements
{
    public abstract class ValueReplacementJob : IJob
    {
        protected ValueReplacementJob()
        {
        }

        protected ValueReplacementJob(Dictionary<string, string> values)
        {
            if (values.IsEmpty()) return;
            Replace(values);
        }

        internal bool IsExecuted { get; set; }
        public IDictionary<string, string> ReplacementItems { get; } = new Dictionary<string, string>();

        /// <summary>
        ///     Execute the job.
        /// </summary>
        /// <returns></returns>
        public bool Execute()
        {
            if (IsExecuted) return IsExecuted;
            IsExecuted = true;
            DoReplacement();
            return IsExecuted;
        }

        /// <summary>
        ///     Cancel Replacement.
        /// </summary>
        public virtual void Cancel() => ReplacementItems.Clear();

        public ValueReplacementItem Replace(string key) => new ValueReplacementItem(this, key);

        public ValueReplacementJob Replace(Dictionary<string, string> values)
        {
            Guard.AllItemsMustNotEmpty(values, nameof(values));
            ReplacementItems.AddRange(values);
            return this;
        }

        protected abstract void DoReplacement();
    }

    public abstract class ValueReplacementJob<T> : ValueReplacementJob
    {
        protected ValueReplacementJob(T value, IDictionary<string, string> replacementValues = null)
        {
            Guard.ArgumentIsNotNull(value, typeof(T).Name);
            Value = value;
            if (replacementValues != null)
                this.Replace(replacementValues);
        }

        public T Value { get; protected internal set; }

        protected abstract override void DoReplacement();
    }
}