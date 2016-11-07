using Microsoft.VisualBasic.FileIO;

namespace HBD.Framework.Data.Csv.Base
{
    public abstract class CsvOption
    {
        public string[] Dilimiters { get; set; } = { "," };
        public int[] FieldWidths { get; set; }
        internal FieldType TextFieldType => FieldWidths.IsEmpty() ? FieldType.Delimited : FieldType.FixedWidth;
    }

    public class ReadCsvOption : CsvOption
    {
        public bool FirstRowIsHeader { get; set; } = true;
    }

    public class WriteCsvOption : CsvOption
    {
        public bool IgnoreHeader { get; set; } = false;
        public string DateFormat { get; set; }
        public string NumericFormat { get; set; }
    }
}
