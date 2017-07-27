#region using

using System.Collections.Generic;

#endregion

namespace HBD.Framework.Text
{
    public interface IPatternExtractor<TPattern> : IEnumerable<TPattern> where TPattern : IPattern
    {
    }
}