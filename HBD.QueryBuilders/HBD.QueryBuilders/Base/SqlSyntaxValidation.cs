#region

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;

#endregion

namespace HBD.QueryBuilders.Base
{
    public static class SqlSyntaxValidation
    {
        public static List<string> Parse(string sql)
        {
            var parser = new TSql100Parser(false);
            IList<ParseError> errors;
            var fragment = parser.Parse(new StringReader(sql), out errors);
            return errors.Select(e => e.Message).ToList();
        }
    }
}