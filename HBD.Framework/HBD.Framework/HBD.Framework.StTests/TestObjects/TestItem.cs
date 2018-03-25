#region using

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace HBD.Framework.Test.TestObjects
{
    public interface ITem
    {
        int Id { get; set; }
        string Name { get; set; }
    }

    public class TestItem : ITem
    {
        public string Details { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestItem2 : ITem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestItem3 : ITem, IDisposable
    {
        public TestItem3()
        {
        }

        public TestItem3(string name)
        {
            Name = name;
        }

        public bool IsDisposed { get; private set; }

        public TestEnum Type { get; set; } = TestEnum.Enum1;

        public string Description { get; set; }

        [Column("Summ")]
        public string Summary { get; set; }

        public void Dispose() => IsDisposed = true;

        public int Id { get; set; }
        public string Name { get; set; }

        private object PrivateObj { get; set; } = new object();
        protected object ProtectedObj { get; set; } = new object();
    }

    public enum TestEnum
    {
        [Description("Enum 1")] Enum1,

        Enum2
    }
}