using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Tools.Test.Extensions.IO
{
    using Tools.Extensions;
    using Tools.Extensions.IO;
    
    [TestClass]
    public class FileExTests
    {
        [TestMethod]
        public void CreateDirectory_ValidDirectoryName_CreatesDirectory()
        {
            string dir = @"C:\Test\";
            dir.CreateDirectory();

            Assert.IsTrue(Directory.Exists(dir));
            Directory.Delete(@"C:\Test\");
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

            foreach (char invalidPathChar in Path.GetInvalidPathChars())
            {
                invalidPathName += invalidPathChar;
            }

            bool isValidPath = invalidPathName.IsValidPath();
            Assert.IsFalse(isValidPath);
        }
    }
}
