using Common.Tools.Extensions.IO;
using Common.Tools.RandomGenerator;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Common.Tools.Test
{
    [TestFixture]
    public class IoHelperTests
    {
        [Test]
        public void GetDirectoryStack_ValidDirectory_ValidDirectoryStack()
        {
            string executingDir = Directory.GetCurrentDirectory();
            Stack<DirectoryInfo> stack = IoHelper.GetDirectoryStack(executingDir);

            Assert.IsTrue(stack.Count > 0);
        }

        [Test]
        public void GetDirectoryStack_NonExistingDirectory_ThrowsArgumentException()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string nonExistantDir = Path.Combine(currentDir, "NONEXISTINGDIR");

            if (Directory.Exists(nonExistantDir)) Directory.Delete(nonExistantDir, true);

            Assert.Throws<ArgumentException>(() => IoHelper.GetDirectoryStack(nonExistantDir));
        }

        [Test]
        public void GetDirectoryStack_EmptyDirectory_Throws()
        {
            Assert.Throws<ArgumentException>(() => IoHelper.GetDirectoryStack(string.Empty));
        }

        [Test]
        public void GetDirectoryStack_NullDirectory_Throws()
        {
            Assert.Throws<ArgumentException>(() => IoHelper.GetDirectoryStack(null));
        }

        [Test]
        public void GetFiles_ValidPath_ArrayOfFiles()
        {
            string path = GetExecutingAssemblyFolder();

            string[] files = IoHelper.GetFiles(path);
            Assert.IsTrue(files.Length > 0);
        }

        [Test]
        [TestCase("\\alskd")]
        [TestCase("")]
        [TestCase(null)]
        public void GetFiles_InValidPath_ThrowsArgumentException(string path)
        {
            Assert.Throws<ArgumentException>(() => IoHelper.GetFiles(path));
        }

        [Test]
        public void GetFileInfo_InValidPath_Throws()
        {
            string path = GetExecutingAssemblyFolder();

            var fileInfoList = IoHelper.GetFileInfoCollection(path);

            Assert.IsTrue(fileInfoList.Count() > 0);
        }

        [Test]
        public void GetFiles_ValidFiles_CallsPredicateOverload_GetsFilesWithCriteria()
        {
            var fileNames = CreateTestFiles();
            string path = fileNames.First().GetDirectoryName();

            //Get all files ending with '.txt' file extensions
            var filesWithTxtEx = IoHelper.GetFiles(path, fi => fi.Name.EndsWith(".txt", StringComparison.OrdinalIgnoreCase));

            Assert.IsTrue(filesWithTxtEx.Count() > 0);
        }

        [Test]
        public void DeleteFiles_ValidLocation_DeletesAllFiles()
        {
            string path = Path.Combine(GetExecutingAssemblyFolder(), "TestLocation");

            Directory.CreateDirectory(path);

            for (int i = 0; i < 10; i++)
            {
                string file = Path.Combine(path, Path.GetFileName(Path.GetTempFileName()));
                File.WriteAllText(file, RandomString.NextAlphabet(100));
            }

            IoHelper.DeleteFiles(path);

            Assert.IsTrue(Directory.EnumerateFiles(path).Count() == 0);

            Directory.Delete(path, true);
        }

        [Test]
        public void DeleteFiles_ValidLocation_DeletesAllFilesFiltered()
        {
            string path = Path.Combine(GetExecutingAssemblyFolder(), "TestLocation");

            Directory.CreateDirectory(path);

            for (int i = 0; i < 10; i++)
            {
                string file = Path.Combine(path, Path.GetFileName(Path.GetTempFileName()));
                File.WriteAllText(file, RandomString.NextAlphabet(100));
            }

            string file2 = Path.Combine(path, "temp.txt");
            File.WriteAllText(file2, RandomString.NextAlphabet(100));

            string file3 = Path.Combine(path, "temp2.txt");
            File.WriteAllText(file3, RandomString.NextAlphabet(100));

            int currentCount = Directory.EnumerateFiles(path).Count();

            IoHelper.DeleteFiles(path, "*.txt");

            int afterDelCount = Directory.EnumerateFiles(path).Count();

            Assert.IsTrue(currentCount - afterDelCount == 2);

            Directory.Delete(path, true);
        }

        [Test]
        public void DeleteFiles_Predicate_DeletesFiles()
        {
            string[] fileNames = CreateTestFiles();
            string folder = Path.GetDirectoryName(fileNames.First());

            IoHelper.DeleteFiles(folder, file => file.Name.EndsWith(".in"));

            foreach (var filename in fileNames)
            {
                if (File.Exists(filename))
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
                () => IoHelper.DeleteFiles(folder, predicate));
        }

        [Test]
        public void DeleteFiles_InvalidFolder_ThrowsArgumentException()
        {
            string folder = "";

            Assert.Throws<ArgumentException>(
                () => IoHelper.DeleteFiles(folder, Predicate));

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

            for (int i = 0; i < fileNames.Length; i++)
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