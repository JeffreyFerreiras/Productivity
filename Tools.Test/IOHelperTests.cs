using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
        public void GetDirectoryStack_NonExistingDirectory_ValidDirectoryStack()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string nonExistantDir = Path.Combine(currentDir, "NONEXISTINGDIR");

            if (Directory.Exists(nonExistantDir)) Directory.Delete(nonExistantDir, true);

            Stack<DirectoryInfo> dirStack = IOHelper.GetDirectoryStack(nonExistantDir);

            Assert.IsTrue(dirStack.Count > 0);
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
            string path = this.GetType().GetTypeInfo().Assembly.Location;

            string[] files = IOHelper.GetFiles(path);
            Assert.IsTrue(files.Length > 0);
        }
    }
}