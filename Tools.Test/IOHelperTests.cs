using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Tools;

namespace Tools.Test
{
    [TestClass]
    public class IOHelperTests
    {
        [TestMethod]
        public void GetDirectoryStack_ValidDirectory_ValidDirectoryStack()
        {
            var executingDir = Directory.GetCurrentDirectory();
            var stack = IOHelper.GetDirectoryStack(executingDir);

            Assert.IsTrue(stack.Count > 0);
        }

        [TestMethod]
        public void GetDirectoryStack_NonExistingDirectory_ValidDirectoryStack()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string nonExistantDir = Path.Combine(currentDir, "NONEXISTINGDIR");

            var dirStack = IOHelper.GetDirectoryStack(nonExistantDir);
            Assert.IsTrue(dirStack.Count > 0);
        }

        [TestMethod]
        public void GetDirectoryStack_EmptyDirectory_Throws()
        {
            Assert.ThrowsException<ArgumentException>(()=>IOHelper.GetDirectoryStack(string.Empty));
        }

        [TestMethod]
        public void GetDirectoryStack_NullDirectory_Throws()
        {
            Assert.ThrowsException<ArgumentException>(() => IOHelper.GetDirectoryStack(null));
        }
    }
}
