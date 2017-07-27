using FluentAssertions;
using HBD.Framework.Data.Csv;
using HBD.Framework.Data.GetSetters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace HBD.Framework.StTests.Data.Csv
{
    [TestClass]
    public class CsvTests
    {
        [TestMethod]
        public void Read_Csv_File_With_Header()
        {
            var data = new CsvAdapter("TestData\\DataBaseInfo\\TableRowCounts.csv").Read();

            data.Count.Should().Be(13);
            data.Header.Should().NotBeNull();
            data.Header.Count.Should().Be(3);
        }

        [TestMethod]
        public void Read_Csv_File_Without_Header()
        {
            var data = new CsvAdapter("TestData\\DataBaseInfo\\TableRowCounts.csv").Read(false);

            data.Count.Should().Be(14);
            data.Header.Should().BeNull();
        }

        [TestMethod]
        public void Read_PipleLine_File_With_Header()
        {
            var data = new CsvAdapter("TestData\\TableRowPipleLine.csv").Read(new ReadCsvOption { Delimiter = "|", FirstRowIsHeader = true });

            data.Count.Should().Be(13);
            data.Header.Should().NotBeNull();
            data.Header.Count.Should().Be(3);
        }

        [TestMethod]
        public void Write_Csv_File_With_Header()
        {
            var data = new CsvAdapter("TestData\\DataBaseInfo\\TableRowCounts.csv").Read();

            var writeFile = "TestData\\TableRowCounts_Tests.csv";

            if (System.IO.File.Exists(writeFile))
                System.IO.File.Delete(writeFile);

            new CsvAdapter(writeFile).Write(data);

            var txt = System.IO.File.ReadAllText(writeFile);

            foreach (var item in data.Header)
                txt.Should().Contain(item as string);
        }

        [TestMethod]
        public void Write_PipleLine_File_With_Date_and_NumberFormat()
        {
            var list = new List<object> {
                new{ Id="1", CreatedOn=DateTime.Now, UpdatedOn=DateTimeOffset.Now, Val=123456},
                new{ Id="2", CreatedOn=DateTime.Now, UpdatedOn=DateTimeOffset.Now, Val=444444}
            };

            var writeFile = "TestData\\Objects_PipleLine_Tests.csv";

            if (System.IO.File.Exists(writeFile))
                System.IO.File.Delete(writeFile);

            new CsvAdapter(writeFile).Write(new ObjectGetSetterCollection<object>(list),
                new WriteCsvOption { Delimiter = "|", DateFormat = "{0:dd.MM.yyyy}", NumericFormat = "{0:###,##0}", IgnoreHeader = false });

            var txt = System.IO.File.ReadAllText(writeFile);
            txt.Should().NotBeNullOrEmpty();
            txt.Should().Contain("|")
                .And.Contain("1|")
                .And.Contain("2|")
                .And.Contain("123,456")
                .And.Contain("444,444")
                .And.Contain(string.Format("{0:dd.MM.yyyy}", DateTime.Now));
        }
    }
}
