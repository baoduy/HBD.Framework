using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBD.Framework.Test.TestObjects
{
    public interface ITem
    {
        int Id { get; set; }
        string Name { get; set; }
    }

    public class TestItem : ITem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Details { get; set; }
    }

    public class TestItem2 : ITem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestItem3 : ITem
    {
        public TestItem3()
        {
        }

        public TestItem3(string name)
        {
            this.Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public TestEnum Type { get; set; } = TestEnum.Enum1;

        public string Description { get; set; }

        [Column("Summ")]
        public string Summary { get; set; }
    }

    public enum TestEnum
    {
        [Description("Enum 1")]
        Enum1,

        Enum2
    }
}