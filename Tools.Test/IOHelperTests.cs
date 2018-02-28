using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Tools.RandomGenerator;

namespace Tools.Test
{
    [TestClass]
    public class IOHelperTests
    {
        [TestMethod]
        public void GetDirectoryStack_ValidDirectory_ValidDirectoryStack()
        {
            string executingDir = Directory.GetCurrentDirectory();
            Stack<DirectoryInfo> stack = IOHelper.GetDirectoryStack(executingDir);

            Assert.IsTrue(stack.Count > 0);
        }

        [TestMethod]
        public void GetDirectoryStack_NonExistingDirectory_ThrowsArgumentException()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string nonExistantDir = Path.Combine(currentDir, "NONEXISTINGDIR");

            if (Directory.Exists(nonExistantDir)) Directory.Delete(nonExistantDir, true);

            Assert.ThrowsException<ArgumentException>( ()=>IOHelper.GetDirectoryStack(nonExistantDir));
        }

        [TestMethod]
        public void GetDirectoryStack_EmptyDirectory_Throws()
        {
            Assert.ThrowsException<ArgumentException>(() => IOHelper.GetDirectoryStack(string.Empty));
        }

        [TestMethod]
        public void GetDirectoryStack_NullDirectory_Throws()
        {
            Assert.ThrowsException<ArgumentException>(() => IOHelper.GetDirectoryStack(null));
        }

        [TestMethod]
        public void GetFiles_ValidPath_ArrayOfFiles()
        {
            string path = GetExecutingAssemblyFolder();

            string[] files = IOHelper.GetFiles(path);
            Assert.IsTrue(files.Length > 0);
        }

        [TestMethod]
        public void GetFiles_InValidPath_ThrowsArgumentException()
        {
            string path = GetExecutingAssemblyFolder() + "\\alskd";

            Assert.ThrowsException<ArgumentException>(() => IOHelper.GetFiles(path));
            Assert.ThrowsException<ArgumentException>(() => IOHelper.GetFiles(""));
            Assert.ThrowsException<ArgumentException>(() => IOHelper.GetFiles(null));
        }

        [TestMethod]
        public void GetFileInfo_InValidPath_Throws()
        {
            string path = GetExecutingAssemblyFolder();

            var fileInfoList = IOHelper.GetFileInfoCollection(path);

            Assert.IsTrue(fileInfoList.Count() > 0);
        }

        [TestMethod]
        public void DeleteFiles_ValidLocation_DeletesAllFiles()
        {
            string path = Path.Combine(GetExecutingAssemblyFolder(), "TestLocation");
            
            Directory.CreateDirectory(path);

            for(int i = 0; i < 10; i++)
            {
                string file = Path.Combine(path, Path.GetFileName(Path.GetTempFileName()));
                File.WriteAllText(file, RandomString.NextAlphabet(100));
            }

            IOHelper.DeleteFiles(path);

            Assert.IsTrue(Directory.EnumerateFiles(path).Count() == 0);

            Directory.Delete(path, true);
        }

        [TestMethod]
        public void DeleteFiles_ValidLocation_DeletesAllFilesFiltered()
        {
            string path = Path.Combine(GetExecutingAssemblyFolder(), "TestLocation");

            Directory.CreateDirectory(path);

            for(int i = 0; i < 10; i++)
            {
                string file = Path.Combine(path, Path.GetFileName(Path.GetTempFileName()));
                File.WriteAllText(file, RandomString.NextAlphabet(100));
            }

            string file2 = Path.Combine(path, "temp.txt");
            File.WriteAllText(file2, RandomString.NextAlphabet(100));

            string file3 = Path.Combine(path, "temp2.txt");
            File.WriteAllText(file3, RandomString.NextAlphabet(100));

            int currentCount = Directory.EnumerateFiles(path).Count();

            IOHelper.DeleteFiles(path, "*.txt");

            int afterDelCount = Directory.EnumerateFiles(path).Count();

            Assert.IsTrue(currentCount - afterDelCount == 2);

            Directory.Delete(path, true);
        }

        private string GetExecutingAssemblyFolder()
        {
            return Path.GetDirectoryName(this.GetType().GetTypeInfo().Assembly.Location);
        }
    }
}