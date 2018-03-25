using FluentAssertions;
using HBD.Framework.Compress;
using HBD.Framework.IO;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace HBD.Framework.StTests.Compress
{
    [TestClass]
    public class ZipCompressTests
    {
        [TestInitialize]
        public void Setup()
        {
            DirectoryEx.DeleteFiles("TestData", "*.zip");
        }

        [TestMethod]
        public void Zip_File()
        {
            var file = Zip.Compress("TestData\\2015 Weekly Calendar.xlsx")
                .Build();

            File.Exists(file);

            Path.GetDirectoryName(file).Contains("TestData")
                .Should().BeTrue();

            Path.GetExtension(file).Should().Be(".zip");

            //Test Exception Extract.
            Action a = () => Zip.Extract(file)
                 .ToTheSameFolder();

            a.Should().Throw<InvalidOperationException>();

            var files = Zip.Extract(file)
                .OverwriteFilesIfExisted()
                .ToTheSameFolder();

            files.Length.Should().Be(1);
            files.All(i => PathEx.IsDirectory(i) ? Directory.Exists(i) : File.Exists(i))
                .Should().BeTrue();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Zip_Twice_Exception()
        {
            Zip.Compress("TestData\\2015 Weekly Calendar.xlsx")
                .Build();

            Zip.Compress("TestData\\2015 Weekly Calendar.xlsx")
                .Build();
        }

        [TestMethod]
        public void Zipped_File_CanBe_Extracted()
        {
            var file = Zip.Compress("TestData\\2015 Weekly Calendar.xlsx")
                .Build();

            DirectoryEx.DeleteDirectories("TestData\\temp\\");
            System.IO.Compression.ZipFile.ExtractToDirectory(file, "TestData\\temp\\");
            File.Exists("TestData\\temp\\2015 Weekly Calendar.xlsx").Should().BeTrue();
        }

        [TestMethod]
        public void Zip_File_With_Password()
        {
            var file = Zip.Compress("TestData\\2015 Weekly Calendar.xlsx")
                .WithPassword("123")
                .Build();

            Action a = () => System.IO.Compression.ZipFile.ExtractToDirectory(file, "TestData\\temp\\");
            a.Should().Throw<Exception>("Protected file cannot extracted without password.");

            Action b = () => Zip.Extract(file)
                .OverwriteFilesIfExisted()
                .ToTheSameFolder();

            b.Should().Throw<ZipException>();

            var files = Zip.Extract(file)
                .WithPassword("123")
                .OverwriteFilesIfExisted()
                .ToTheSameFolder();

            files.Length.Should().Be(1);
            files.All(i => PathEx.IsDirectory(i) ? Directory.Exists(i) : File.Exists(i))
                .Should().BeTrue();
        }

        [TestMethod]
        public void Zip_Folder()
        {
            var file = Zip.Compress("TestData\\DataBaseInfo")
                .Build();

            File.Exists(file);

            file.Contains("DataBaseInfo")
                .Should().BeTrue();

            Path.GetExtension(file).Should().Be(".zip");
            File.Exists(file).Should().BeTrue();

            var files = Zip.Extract(file)
               .OverwriteFilesIfExisted()
               .ToTheSameFolder();

            files.Length.Should().BeGreaterOrEqualTo(4);
            files.All(a => PathEx.IsDirectory(a) ? Directory.Exists(a) : File.Exists(a))
                .Should().BeTrue();
        }

        [TestMethod]
        public void Zip_Folder_With_SubEmptyFolder()
        {
            Directory.CreateDirectory("TestData\\DataBaseInfo\\EmptyFolder");
            var file = Zip.Compress("TestData\\DataBaseInfo")
                .Build();

            File.Exists(file);

            var entryCount = 0;
            using (var o = new System.IO.Compression.ZipArchive(File.OpenRead(file), System.IO.Compression.ZipArchiveMode.Read))
            {
                entryCount = o.Entries.Count;

                o.Entries.Any(i => i.FullName.Contains("DataBaseInfo/EmptyFolder"))
                    .Should().BeTrue();
            }

            var files = Zip.Extract(file)
              .OverwriteFilesIfExisted()
              .To("TestData\\TestExtraction\\");

            files.Length.Should().BeGreaterOrEqualTo(entryCount);
            files.All(a=>PathEx.IsDirectory(a)?Directory.Exists(a):File.Exists(a))
                .Should().BeTrue();
        }

        [TestMethod]
        public void Zip_Folder_With_SubFolder()
        {
            Directory.CreateDirectory("TestData\\DataBaseInfo\\SubFolder");
            File.Copy("TestData\\baoduy2412@yahoo.com.png",
                "TestData\\DataBaseInfo\\SubFolder\\baoduy2412@yahoo.com.png", true);

            var file = Zip.Compress("TestData\\DataBaseInfo")
                .Build();

            File.Exists(file);

            using (var o = new System.IO.Compression.ZipArchive(File.OpenRead(file), System.IO.Compression.ZipArchiveMode.Read))
            {
                o.Entries.Any(i => i.FullName.Contains("DataBaseInfo/SubFolder/baoduy2412@yahoo.com.png"))
                    .Should().BeTrue();
            }
        }

        [TestMethod]
        public void Zip_OvewriteIfExisted_File()
        {
            var file = Zip.Compress("TestData\\DataBaseInfo")
               .Build();

            File.Exists(file);

            Zip.Compress("TestData\\FixedLenghTextFile.txt")
                .OverwriteIfExisted()
                .BuildTo(file);

            File.Exists(file);

            using (var o = new System.IO.Compression.ZipArchive(File.OpenRead(file), System.IO.Compression.ZipArchiveMode.Read))
            {
                o.Entries.Any(i => i.Name.Contains("FixedLenghTextFile.txt"))
                    .Should().BeTrue();
                o.Entries.Count.Should().Be(1);
            }
        }

        [TestMethod]
        public void Zip_Expand_To_Existed_File()
        {
            var file = Zip.Compress("TestData\\DataBaseInfo")
               .Build();

            File.Exists(file);

            Zip.Compress("TestData\\FixedLenghTextFile.txt")
                .Include("TestData\\TestZip")
                .AppendIfExisted()
                .BuildTo(file);

            File.Exists(file);

            using (var o = new System.IO.Compression.ZipArchive(File.OpenRead(file), System.IO.Compression.ZipArchiveMode.Read))
            {
                o.Entries.Any(i => i.Name.Contains("FixedLenghTextFile.txt"))
                    .Should().BeTrue();

                o.Entries.Any(i => i.FullName.Contains("TestZip/XMLFile1.xml"))
                   .Should().BeTrue();

                o.Entries.Any(i => i.FullName.Contains("TestZip/XMLFile2.xml"))
                .Should().BeTrue();

                o.Entries.Count.Should().BeGreaterOrEqualTo(5);
            }
        }

        [TestMethod]
        public void Zip_DefaultOption_File()
        {
            var file = Zip.Compress("TestData\\DataBaseInfo")
               .Build();

            File.Exists(file);

            Action a = () => Zip.Compress("TestData\\FixedLenghTextFile.txt")
                  .BuildTo(file);

            a.Should().Throw<InvalidOperationException>();
        }
    }
}
