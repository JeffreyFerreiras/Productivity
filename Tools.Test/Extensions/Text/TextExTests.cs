using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Tools.Extensions.Text;

namespace Tools.Test.Extensions.Text
{
    [TestClass]
    public class TextExTests
    {
        [TestMethod]
        public void ToCamelCase_Phrase_ReturnsCamelCase()
        {
            
        }

        [TestMethod]
        public void Tab_Word_AppendsTabs()
        {
            int tabCount = 4;
            string text = "Hello";
            string result = text.Tab(tabCount);

            Assert.IsTrue(result.Length == text.Length + tabCount);
        }

        [TestMethod]
        public void Tab_Empty_AppendsTabs()
        {
            int tabCount = 4;
            string text = "";
            string result = text.Tab(tabCount);

            Assert.IsTrue(result.Length == text.Length + tabCount);
        }

        [TestMethod]
        public void Tab_NegativeTabCount_ReturnsText()
        {
            int tabCount = -4;
            string text = "Hello";
            string result = text.Tab(tabCount);

            Assert.AreEqual(text, result);
        }

        [TestMethod]
        public void Tab_InvalidText_ReturnsText()
        {
            string text = null;
            string result = text.Tab();

            Assert.AreEqual(text, result);
        }
    }
}
