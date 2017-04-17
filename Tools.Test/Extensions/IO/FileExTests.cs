using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Tools.Test.Extensions.IO
{
    using Tools.Extensions.IO;
    [TestClass]
    public class FileExTests
    {
        [TestMethod]
        public void CreateDirectory_Test()
        {
            string dir = @"C:\Test\Test\test\";
            dir.CreateDirectory();

            Assert.IsTrue(Directory.Exists(dir));

            Directory.Delete(@"C:\Test\Test\test");
            Directory.Delete(@"C:\Test\test");
            Directory.Delete(@"C:\Test\");
        }
    }
}
