#region using

using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Data.DataReplacements
{
    public class ValueReplacementItem
    {
        public ValueReplacementItem(ValueReplacementJob job, string key)
        {
            Guard.ArgumentIsNotNull(job, nameof(job));

            Job = job;
            ReplaceKey = key;
        }

        private ValueReplacementJob Job { get; }
        internal string ReplaceKey { get; }

        public ValueReplacementJob WithValue(string value)
        {
            Job.ReplacementItems[ReplaceKey] = value;
            return Job;
        }
    }
}