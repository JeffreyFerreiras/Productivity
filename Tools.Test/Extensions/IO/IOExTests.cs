using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Tools.Test.Extensions.IO
{
    using System.Reflection;
    using Tools.Extensions.Collection;
    using Tools.Extensions.IO;

    [TestClass]
    public class IOExTests
    {
        [TestMethod]
        public void CreateDirectory_ValidDirectoryName_CreatesDirectory()
        {
            string dir = @"C:\Test\";
            dir.CreateDirectory();

            Assert.IsTrue(Directory.Exists(dir));
            Directory.Delete(@"C:\Test\", true);
        }

        [TestMethod]
        public void CreateDirectory_InvalidDirectory_ReturnsFalse()
        {
            char invalidChar = Path.GetInvalidPathChars().GetRandomElement();
            string invalidDirectoryName = $@"C:\invlaidFolder{invalidChar}Name";

            Assert.IsFalse(invalidDirectoryName.CreateDirectory());
        }

        [TestMethod]
        public void IsValidPath_ValidPath_ReturnsTrue()
        {
            string validPath = @"c:\\TEST\";

            bool isValidPath = validPath.IsValidPath();

            Assert.IsTrue(isValidPath);
        }

        [TestMethod]
        public void IsValidPath_InvalidPath_ReturnsFalse()
        {
            string invalidPathName = $@"C:\";

            foreach(char invalidPathChar in Path.GetInvalidPathChars())
            {
                invalidPathName += invalidPathChar;
            }

            bool isValidPath = invalidPathName.IsValidPath();
            Assert.IsFalse(isValidPath);
        }

        [TestMethod]
        public void IsValidPath_Null_ReturnsFalse()
        {
            string invalidPathName = null;

            bool isValidPath = invalidPathName.IsValidPath();
            Assert.IsFalse(isValidPath);
        }

        [TestMethod]
        public void IsValidPath_Empty_ReturnsFalse()
        {
            string invalidPathName = string.Empty;

            bool isValidPath = invalidPathName.IsValidPath();

            Assert.IsFalse(isValidPath);
        }

        [TestMethod]
        public void IsValidPath_RandomString_ReturnsFalse()
        {
            bool isValidPath = RandomString.NextAlphabet().IsValidPath();

            Assert.IsTrue(isValidPath);
        }

        [TestMethod]
        public void IsFile_ValidFileName_ReturnsTrue()
        {
            string dir = Path.GetDirectoryName(this.GetType().GetTypeInfo().Assembly.Location);
            string file = Path.Combine(dir, "testFile.txt");
            bool pass = file.IsValidFileName();

            Assert.IsTrue(pass);
        }

        [TestMethod]
        public void IsFile_OnlyFileName_ReturnsTrue()
        {
            //string dir = Path.GetDirectoryName(this.GetType().GetTypeInfo().Assembly.Location);
            string file = "testFile.txt";

            Assert.IsTrue(file.IsValidFileName());
        }

        [TestMethod]
        public void IsFile_Empty_ReturnsFalse()
        {
            Assert.IsFalse(string.Empty.IsValidFileName());
        }

        [TestMethod]
        public void IsFile_Null_ReturnsFalse()
        {
            string n = null;

            Assert.IsFalse(n.IsValidFileName());
        }

        [TestMethod]
        public void IsFile_InvalidFileNameChars_ReturnsFalse()
        {
            string n = new string(Path.GetInvalidFileNameChars());
            Assert.IsFalse(n.IsValidFileName());
        }
    }
}