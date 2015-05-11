using System;
namespace HBD.Framework.Data.Utilities
{
    public interface IFilterClause { }

    public enum CompareOperation
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan,
        GreaterThanOrEquals,
        LessThanOrEquals,
        Contains,
        NotContains,
        StartsWith,
        EndsWith,
        In,
        NotIn,
        IsNull,
        NotNull
    }

    public enum BinaryOperation { AND, OR }
}
