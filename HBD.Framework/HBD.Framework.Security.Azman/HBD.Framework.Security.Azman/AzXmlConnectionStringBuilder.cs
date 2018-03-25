#region

using System.IO;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Security.Azman
{
    /// <summary>
    ///     msxml://[FilePath]
    /// </summary>
    public class AzXmlConnectionStringBuilder : BaseAzConnectionStringBuilder
    {
        public AzXmlConnectionStringBuilder(string connectionString = null)
        {
            Parse(connectionString);
        }

        public string Driver => "msxml://";
        public string FileName { get; set; }

        public override void Validate()
        {
            Guard.ArgumentIsNotNull(FileName, nameof(FileName));
            if (!File.Exists(FileName))
                throw new FileNotFoundException(FileName);
        }

        protected override string BuildConnectionString()
        {
            Validate();
            return $"{Driver}{Path.GetFullPath(FileName)}";
        }

        protected override void Parse(string connectionString)
        {
            if (connectionString.IsNullOrEmpty())
                FileName = string.Empty;
            else
            {
                FileName = connectionString.Replace(Driver, string.Empty);
                Validate();
            }
        }
    }
}