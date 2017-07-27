namespace HBD.Framework.Compress
{
    public abstract class ZipOption
    {
        protected internal IZipAdapter ZipAdapter { get; set; }
        protected internal string Password { get; set; }

        protected IZipAdapter GetOrCreateAdapter()
            => ZipAdapter ?? CreateAdapter();

        protected virtual IZipAdapter CreateAdapter() => new ZipAdapter();
    }
}
