#region

using System;
using System.Threading.Tasks;

#endregion

namespace HBD.Framework.Data.SSDT
{
    public interface ISqlMergeScriptGeneration : IDataTableMergeScriptGeneration, IDisposable
    {
        /// <summary>
        ///     Generate Static Data Migration Script for all Tables in DB.
        /// </summary>
        /// <param name="option"></param>
        void GenerateAll(MergeScriptOption option = MergeScriptOption.Default);

        void Generate(MergeScriptOption option = MergeScriptOption.Default, params string[] tables);

        /// <summary>
        ///     Generate Static Data Migration Script for the list of Tables.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="updateStatus"></param>
        /// <param name="tables">The list of Table Name</param>
        void Generate(MergeScriptOption option = MergeScriptOption.Default, Action<string> updateStatus = null,
            params string[] tables);

        Task GenerateAsync(MergeScriptOption option = MergeScriptOption.Default, params string[] tables);

        Task GenerateAsync(MergeScriptOption option = MergeScriptOption.Default, Action<string> updateStatus = null,
            params string[] tables);
    }
}