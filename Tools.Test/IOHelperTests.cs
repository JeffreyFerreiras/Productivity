using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Tools.RandomGenerator;

namespace Tools.Test
{
    [TestFixture]
    public class IOHelperTests
    {
        [Test]
        public void GetDirectoryStack_ValidDirectory_ValidDirectoryStack()
        {
            string executingDir = Directory.GetCurrentDirectory();
            Stack<DirectoryInfo> stack = IOHelper.GetDirectoryStack(executingDir);

            Assert.IsTrue(stack.Count > 0);
        }

        [Test]
        public void GetDirectoryStack_NonExistingDirectory_ThrowsArgumentException()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string nonExistantDir = Path.Combine(currentDir, "NONEXISTINGDIR");

            if(Directory.Exists(nonExistantDir)) Directory.Delete(nonExistantDir, true);

            Assert.Throws<ArgumentException>(() => IOHelper.GetDirectoryStack(nonExistantDir));
        }

        [Test]
        public void GetDirectoryStack_EmptyDirectory_Throws()
        {
            Assert.Throws<ArgumentException>(() => IOHelper.GetDirectoryStack(string.Empty));
        }

        [Test]
        public void GetDirectoryStack_NullDirectory_Throws()
        {
            Assert.Throws<ArgumentException>(() => IOHelper.GetDirectoryStack(null));
        }

        [Test]
        public void GetFiles_ValidPath_ArrayOfFiles()
        {
            string path = GetExecutingAssemblyFolder();

            string[] files = IOHelper.GetFiles(path);
            Assert.IsTrue(files.Length > 0);
        }

        [Test]
        public void GetFiles_InValidPath_ThrowsArgumentException()
        {
            string path = GetExecutingAssemblyFolder() + "\\alskd";

            Assert.Throws<ArgumentException>(() => IOHelper.GetFiles(path));
            Assert.Throws<ArgumentException>(() => IOHelper.GetFiles(""));
            Assert.Throws<ArgumentException>(() => IOHelper.GetFiles(null));
        }

        [Test]
        public void GetFileInfo_InValidPath_Throws()
        {
            string path = GetExecutingAssemblyFolder();

            var fileInfoList = IOHelper.GetFileInfoCollection(path);

            Assert.IsTrue(fileInfoList.Count() > 0);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void DeleteFiles_Predicate_DeletesFiles()
        {
            string[] fileNames = CreateTestFiles();
            string folder = Path.GetDirectoryName(fileNames.First());

            IOHelper.DeleteFiles(folder, file => file.Name.EndsWith(".in"));

            foreach(var filename in fileNames)
            {
                if(File.Exists(filename))
                {
                    Assert.IsFalse(filename.EndsWith(".in"));
                }
            }

            Directory.Delete(folder, true);
        }

        [Test]
        public void DeleteFiles_NullPredicate_ThrowsArgumentException()
        {
            string folder = Directory.GetCurrentDirectory();

            Func<FileInfo, bool> predicate = null;

            Assert.Throws<ArgumentException>(
                () => IOHelper.DeleteFiles(folder, predicate));
        }

        [Test]
        public void DeleteFiles_InvalidFolder_ThrowsArgumentException()
        {
            string folder = "";

            Assert.Throws<ArgumentException>(
                () => IOHelper.DeleteFiles(folder, Predicate));

            bool Predicate(FileInfo f) => f.LastWriteTime < DateTime.Today;
        }

        private static string[] CreateTestFiles()
        {
            string localPath = Directory.GetCurrentDirectory() + "\\UnitTests\\";

            Directory.CreateDirectory(localPath);

            string[] fileNames = new string[]
            {
                "temp.in",
                "temp.txt",
                "test.in",
                "test.txt"
            };

            for(int i = 0; i < fileNames.Length; i++)
            {
                fileNames[i] = Path.Combine(localPath, fileNames[i]);
                File.WriteAllText(fileNames[i], RandomString.NextAlphabet());
            }

            return fileNames;
        }

        private string GetExecutingAssemblyFolder()
        {
            return Path.GetDirectoryName(this.GetType().GetTypeInfo().Assembly.Location);
        }
    }
}