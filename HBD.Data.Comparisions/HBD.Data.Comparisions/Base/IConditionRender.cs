#region

using System.Collections.Generic;

#endregion

namespace HBD.Data.Comparisons.Base
{
    public interface IConditionRender
    {
        string BuildCondition(ICondition condition, IDictionary<string, object> outPutParameters = null);
    }
}