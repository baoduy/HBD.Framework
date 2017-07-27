namespace HBD.Framework.Data.Excel.Base
{
    public class ExcelAdapterOption
    {
        /// <summary>
        ///     Open file with edit mode
        /// </summary>
        public OpenMode OpenMode { get; set; } = OpenMode.ReadOnly;

        /// <summary>
        ///     Auto create new file if not exisited.
        /// </summary>
        public bool AutoCreateNewFile { get; set; } = true;

        /// <summary>
        ///     Add default sheets {sheet 1, sheet 2, sheet 3} when create the file.
        /// </summary>
        public bool AddDefaultSheets { get; set; } = true;
    }

    public enum OpenMode
    {
        ReadOnly = 0,
        Editable = 1
    }
}