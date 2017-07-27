namespace HBD.Framework.Data.Csv
{
    public abstract class CsvOption
    {
        public string Delimiter { get; set; } = ",";
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
