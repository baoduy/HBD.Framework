#region

using System;
using System.IO;
using System.IO.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.IO.Tests
{
    [TestClass]
    public class DirectoryTests
    {
        private IDisposable _context;
        private int _fileDeleteCount;
        private int _fileExistsCount;
        private int _folderDeleteCount;
        private int _folderExistsCount;

        [TestInitialize]
        public void Initializer()
        {
            _fileExistsCount = 0;
            _fileDeleteCount = 0;
            _folderExistsCount = 0;
            _folderDeleteCount = 0;

            _context = ShimsContext.Create();

            ShimFile.ExistsString = f =>
            {
                _fileExistsCount += 1;
                return f == "1";
            };
            ShimFile.DeleteString = f => { _fileDeleteCount += 1; };

            ShimDirectory.ExistsString = f =>
            {
                _folderExistsCount += 1;
                return f == "1";
            };
            ShimDirectory.DeleteString = f => { _folderDeleteCount += 1; };
            ShimDirectory.DeleteStringBoolean = (f, b) => { _folderDeleteCount += 1; };

            ShimDirectory.GetFilesStringString = (f, s) => new[] {"1", "2", "1"};
            ShimDirectory.GetDirectoriesString = f => new[] {"1", "2", "1"};

            ShimDirectoryInfo.AllInstances.GetFiles = d => new FileInfo[]
            {
                new ShimFileInfo
                {
                    ExistsGet = () =>
                    {
                        _fileExistsCount += 1;
                        return true;
                    }
                },
                new ShimFileInfo
                {
                    ExistsGet = () =>
                    {
                        _fileExistsCount += 1;
                        return false;
                    }
                },
                new ShimFileInfo
                {
                    ExistsGet = () =>
                    {
                        _fileExistsCount += 1;
                        return true;
                    }
                }
            };
            ShimDirectoryInfo.AllInstances.GetDirectories = d => new DirectoryInfo[]
            {
                new ShimDirectoryInfo
                {
                    ExistsGet = () =>
                    {
                        _folderExistsCount += 1;
                        return true;
                    }
                },
                new ShimDirectoryInfo
                {
                    ExistsGet = () =>
                    {
                        _folderExistsCount += 1;
                        return false;
                    }
                },
                new ShimDirectoryInfo
                {
                    ExistsGet = () =>
                    {
                        _folderExistsCount += 1;
                        return true;
                    }
                }
            };

            ShimDirectoryInfo.AllInstances.Delete = d => { _folderDeleteCount += 1; };
            ShimFileInfo.AllInstances.Delete = d => { _fileDeleteCount += 1; };
        }

        [TestCleanup]
        public void CleanUp()
        {
            _context.Dispose();
        }

        [TestMethod]
        [TestCategory("Fw.IO")]
        public void Delete_FilesArray_Test()
        {
            DirectoryEx.DeleteFiles("1", "2", "1");

            Assert.AreEqual(3, _fileExistsCount);
            Assert.AreEqual(2, _fileDeleteCount);
        }

        [TestMethod]
        [TestCategory("Fw.IO")]
        public void Delete_DirectoriesArray_Test()
        {
            DirectoryEx.DeleteDirectories("1", "2", "1");

            Assert.AreEqual(3, _folderExistsCount);
            Assert.AreEqual(2, _folderDeleteCount);
        }

        [TestMethod]
        [TestCategory("Fw.IO")]
        public void Delete_FilesInDirectory_Test()
        {
            DirectoryEx.DeleteFiles("1", "");

            Assert.AreEqual(_folderExistsCount, 1);

            Assert.AreEqual(3, _fileExistsCount);
            Assert.AreEqual(2, _fileDeleteCount);
        }

        [TestMethod]
        [TestCategory("Fw.IO")]
        public void Delete_EmptyFilesInDirectory_Test()
        {
            DirectoryEx.DeleteFiles("", "");

            Assert.AreEqual(_folderExistsCount, 1);

            Assert.AreEqual(0, _fileExistsCount);
            Assert.AreEqual(0, _fileDeleteCount);
        }

        [TestMethod]
        [TestCategory("Fw.IO")]
        public void Delete_SubDirectories_Test()
        {
            DirectoryEx.DeleteSubDirectories("1");

            Assert.AreEqual(4, _folderExistsCount);
            Assert.AreEqual(2, _folderDeleteCount);
        }

        [TestMethod]
        [TestCategory("Fw.IO")]
        public void Delete_EmptySubDirectories_Test()
        {
            DirectoryEx.DeleteSubDirectories("");

            Assert.AreEqual(1, _folderExistsCount);
            Assert.AreEqual(0, _folderDeleteCount);
        }

        [TestMethod]
        [TestCategory("Fw.IO")]
        public void ClearDirectoryTest()
        {
            DirectoryEx.CleanupDirectory("1");

            Assert.AreEqual(3, _fileExistsCount);
            Assert.AreEqual(2, _fileDeleteCount);

            Assert.AreEqual(5, _folderExistsCount);
            Assert.AreEqual(2, _folderDeleteCount);
        }

        [TestMethod]
        [TestCategory("Fw.IO")]
        public void Delete_Files_Test()
        {
            new DirectoryInfo("TestData\\").DeleteFiles();

            Assert.AreEqual(3, _fileExistsCount);
            Assert.AreEqual(2, _fileDeleteCount);
        }
    }
}