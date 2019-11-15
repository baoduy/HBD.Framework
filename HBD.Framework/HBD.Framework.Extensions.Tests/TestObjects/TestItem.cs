using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBD.Framework.Extensions.Tests.TestObjects
{
    [DisplayName("HBD")]
    public enum TestEnum
    {
        [Description("Enum 1")] Enum1,

        Enum2
    }

    public interface ITem
    {
        #region Public Properties

        int Id { get; set; }
        string Name { get; set; }

        #endregion Public Properties
    }

    public class TestItem : ITem
    {
        #region Public Properties

        public string Details { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        #endregion Public Properties
    }

    public class TestItem2 : ITem
    {
        #region Public Properties

        public int Id { get; set; }
        public string Name { get; set; }

        #endregion Public Properties
    }

    public class TestItem3 : ITem, IDisposable
    {
        #region Protected Properties

        protected object ProtectedObj { get; set; } = new object();

        #endregion Protected Properties

        #region Private Properties

        private object PrivateObj { get; set; } = new object();

        #endregion Private Properties

        #region Public Methods

        public void Dispose() => IsDisposed = true;

        #endregion Public Methods

        #region Public Constructors

        public TestItem3()
        {
        }

        public TestItem3(string name)
        {
            Name = name;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Description { get; set; }
        public int Id { get; set; }
        public bool IsDisposed { get; private set; }

        public string Name { get; set; }

        [Column("Summ")] public string Summary { get; set; }

        public TestEnum Type { get; set; } = TestEnum.Enum1;

        #endregion Public Properties
    }
}