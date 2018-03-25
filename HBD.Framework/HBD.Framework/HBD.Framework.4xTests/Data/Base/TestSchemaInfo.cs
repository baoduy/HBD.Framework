#region using

using HBD.Framework.Data.SqlClient.Base;

#endregion

namespace HBD.Framework.Data.Base.Tests
{
    class TestSchemaInfo : SchemaInfo
    {
        public TestSchemaInfo(string name) : base(name)
        {
        }
    }
}