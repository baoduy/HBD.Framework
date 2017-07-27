namespace HBD.Framework.Data.Excel.Base
{
    public class SheetInfo
    {
        public SheetInfo(string name, bool isHidden = false)
        {
            Name = name;
            IsHidden = isHidden;
        }

        public string Name { get; }
        public bool IsHidden { get; }
    }
}