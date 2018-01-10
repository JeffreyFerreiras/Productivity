using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Tools.Text;

namespace Tools.Test.Text
{
    [TestClass]
    public class EditorTests
    {
        [TestMethod]
        public void Mask_ValidString_ReturnsMaskedString()
        {
            string source = "123456789";
            string result = Editor.Mask(source);

            string maskedChars = result.Substring(0, result.Length - 4);

            Assert.IsTrue(maskedChars.All(c => c == '*'));
        }

        [TestMethod]
        public void Mask_NegativeNumber_ReturnsSame()
        {
            int showlast = -4;
            string source = "123456789";
            string result = Editor.Mask(source, showlast);

            Assert.AreEqual(source, result);
        }

        [TestMethod]
        public void Mask_MaskLongerThanString_ReturnsSame()
        {
            int showlast = 10;
            string source = "12345";
            string result = Editor.Mask(source, showlast);
            
            Assert.IsTrue(source == result);
        }

        [TestMethod]
        public void Mask_Null_ReturnsSame()
        {
            int showlast = 4;
            string source = null;

            string result = Editor.Mask(source, showlast);

            Assert.IsTrue(source == result);
        }

        [TestMethod]
        public void RemoveHtmlXmlTags_ValidHTML_ReturnsStrippedString()
        {
            string html = "";
            string result = Editor.RemoveHtmlXmlTags(html);

            Assert.Fail();
        }
    }
}
