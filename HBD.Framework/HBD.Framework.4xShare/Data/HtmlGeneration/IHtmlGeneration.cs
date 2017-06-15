namespace HBD.Framework.Data.HtmlGeneration
{
    public interface IHtmlGeneration
    {
        string Generate();

        string ToClipboardFormat();
    }
}