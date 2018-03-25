namespace HBD.Framework.Security.Azman
{
    public interface IAzConnectionStringBuilder
    {
        string ConnectionString { get; set; }

        void Validate();
    }

    public abstract class BaseAzConnectionStringBuilder : IAzConnectionStringBuilder
    {
        public abstract void Validate();

        public string ConnectionString
        {
            get { return BuildConnectionString(); }
            set { Parse(value); }
        }

        protected abstract void Parse(string connectionString);

        protected abstract string BuildConnectionString();

        public override string ToString() => ConnectionString;
    }
}