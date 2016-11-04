using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.Data.SSDT
{
    public interface IDataTableMergeScriptGeneration
    {
        string OutputFolder { get; set; }

        void Generate(IEnumerable<DataTable> tables, MergeScriptOption option = MergeScriptOption.Default,
            Action<string> updateStatus = null);

        Task GenerateAsync(IEnumerable<DataTable> tables, MergeScriptOption option = MergeScriptOption.Default,
            Action<string> updateStatus = null);

        /// <summary>
        ///     Generate Static Data Migration Script From DataTable
        /// </summary>
        /// <param name="table"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        string Generate(DataTable table, MergeScriptOption option = MergeScriptOption.Default);

        void AddHeader(StringBuilder builder);
    }
}