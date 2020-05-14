using Common.Tools.Extensions.Collection;
using Common.Tools.Extensions.IO;
using Common.Tools.RandomGenerator;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace Common.Tools.Test.Extensions.IO
{
    [TestFixture]
    public class IoExTests
    {
        [Test]
        public void CreateDirectory_ValidDirectoryName_CreatesDirectory()
        {
            string dir = @"C:\Test\";
            dir.CreateDirectory();

            Assert.IsTrue(Directory.Exists(dir));
            Directory.Delete(@"C:\Test\", true);
        }

        [Test]
        public void CreateDirectory_InvalidDirectory_ReturnsFalse()
        {
            char invalidChar = Path.GetInvalidPathChars().GetRandomElement();
            string invalidDirectoryName = $@"C:\invlaidFolder{invalidChar}Name";

            Assert.IsFalse(invalidDirectoryName.CreateDirectory());
        }

        [Test]
        public void IsValidPath_ValidPath_ReturnsTrue()
        {
            string validPath = @"c:\\TEST\";

            bool isValidPath = validPath.IsValidPath();

            Assert.IsTrue(isValidPath);
        }

        [Test]
        public void IsValidPath_InvalidPath_ReturnsFalse()
        {
            string invalidPathName = $@"C:\";

            foreach (char invalidPathChar in Path.GetInvalidPathChars())
            {
                invalidPathName += invalidPathChar;
            }

            bool isValidPath = invalidPathName.IsValidPath();
            Assert.IsFalse(isValidPath);
        }

        [Test]
        public void IsValidPath_Null_ReturnsFalse()
        {
            string invalidPathName = null;

            bool isValidPath = invalidPathName.IsValidPath();
            Assert.IsFalse(isValidPath);
        }

        [Test]
        public void IsValidPath_Empty_ReturnsFalse()
        {
            string invalidPathName = string.Empty;

            bool isValidPath = invalidPathName.IsValidPath();

            Assert.IsFalse(isValidPath);
        }

        [Test]
        public void IsValidPath_RandomString_ReturnsFalse()
        {
            bool isValidPath = RandomString.NextAlphabet().IsValidPath();

            Assert.IsTrue(isValidPath);
        }

        [Test]
        public void IsFile_ValidFileName_ReturnsTrue()
        {
            bool pass = "testFile.txt".IsValidFileName();

            Assert.IsTrue(pass);
        }

        [Test]
        public void IsFile_ValidLongFileName_ReturnsTrue()
        {
            bool pass = "testFileasdfasdfasdfsadfasdfkljhaslkdjfhlaskdjhgflaskhjdgflakshjdgflkashjgdflkasjhgdflkjashdflkjashdflkasjhdflksajdhf.txt".IsValidFileName();

            Assert.IsTrue(pass);
        }

        [Test]
        public void IsFile_Empty_ReturnsFalse()
        {
            Assert.IsFalse(string.Empty.IsValidFileName());
        }

        [Test]
        public void IsFile_Null_ReturnsFalse()
        {
            string n = null;

            Assert.IsFalse(n.IsValidFileName());
        }

        [Test]
        public void IsFile_InvalidFileNameChars_ReturnsFalse()
        {
            string n = new string(Path.GetInvalidFileNameChars());

            Assert.IsFalse(n.IsValidFileName());
        }

        [Test]
        public void BuildFileShareSafeStreamWriter_ValidFile_CannotOpenSecondStreamForReads()
        {
            string testFile = Path.Combine(Directory.GetCurrentDirectory(), "UnitTesting", "TESTFILE.txt");

            Directory.CreateDirectory(Path.GetDirectoryName(testFile));

            using (StreamWriter sw = IoHelper.BuildFileShareSafeStreamWriter(testFile))
            {
                Assert.Throws<IOException>(() => File.OpenRead(testFile));
            }
        }

        [Test]
        public void BuildFileShareSafeStreamWriter_InvalidDir_ThrowsArgumentException()
        {
            string testFile = Path.Combine(Directory.GetCurrentDirectory(), "UnitTesting", "TESTFILE.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(testFile));
            Directory.Delete(Path.GetDirectoryName(testFile), true);

            Assert.Throws<ArgumentException>(() => IoHelper.BuildFileShareSafeStreamWriter(testFile));
        }

        [Test]
        public void BuildFileShareSafeStreamWriter_InvalidFileName_ThrowsArgumentException()
        {
            string testFile = Path.Combine(Directory.GetCurrentDirectory(), "UnitTesting\\");
            Directory.CreateDirectory(Path.GetDirectoryName(testFile));

            testFile += new string(Path.GetInvalidFileNameChars().Take(6).ToArray());

            Assert.Throws<ArgumentException>(() => IoHelper.BuildFileShareSafeStreamWriter(testFile));
        }
    }
}