#region

using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.IO.Tests
{
    [TestClass]
    public class DirectoryExTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            DirectoryEx.DeleteDirectories("TestMovingFilesAndFolders");
        }

        [TestMethod]
        public void MoveAllFilesAndFoldersToTest()
        {
            Directory.CreateDirectory("TestMovingFilesAndFolders\\FilesAndFolders\\FileAndFolders\\TestFolder");
            File.WriteAllText("TestMovingFilesAndFolders\\FilesAndFolders\\FileAndFolders\\TestFile.txt",
                "Hoang Bao Duy");

            new DirectoryInfo("TestMovingFilesAndFolders\\FilesAndFolders\\FileAndFolders")
                .MoveAllFilesAndFoldersTo("TestMovingFilesAndFolders\\FilesAndFolders");

            Assert.IsTrue(Directory.Exists("TestMovingFilesAndFolders\\FilesAndFolders\\TestFolder"));
            Assert.IsTrue(Directory.GetFiles("TestMovingFilesAndFolders\\FilesAndFolders\\TestFolder").Length == 0);
            Assert.IsTrue(Directory.GetDirectories("TestMovingFilesAndFolders\\FilesAndFolders\\TestFolder").Length == 0);
            Assert.IsTrue(File.Exists("TestMovingFilesAndFolders\\FilesAndFolders\\TestFile.txt"));

            Assert.IsFalse(Directory.Exists("TestMovingFilesAndFolders\\FilesAndFolders\\FileAndFolders\\TestFolder"));
            Assert.IsFalse(Directory.Exists("TestMovingFilesAndFolders\\FilesAndFolders\\FileAndFolders"));
        }
    }
}