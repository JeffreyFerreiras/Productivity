using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tools.Attributes;

namespace Tools.Test.Attributes
{
    [TestClass]
    public class DirectoryAttributeTests
    {
        
        [TestMethod()]
        public void Directory_Test()
        {
            var dirAttribute = new Directory();
            bool valid = dirAttribute.IsValid(@"C:\Test\Test");
            Assert.IsTrue(valid);
        }
    }
}
