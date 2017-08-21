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
    }
}
