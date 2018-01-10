using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Tools.Extensions.Text;

namespace Tools.Test.Text
{
    [TestClass]
    public class StringBuilderExTests
    {

        [TestMethod]
        public void IndexOf_ValidInput_ReturnsIndex()
        {
            var sb = new StringBuilder("Hello World!");
            int index = sb.IndexOf('o');

            Assert.IsTrue(index == 4);
        }

        [TestMethod]
        public void IndexOf_Empty_ReturnsIndex()
        {
            var sb = new StringBuilder("");
            int index = sb.IndexOf('o');

            Assert.IsTrue(index == -1);
        }

        [TestMethod]
        public void IndexOf_Null_Throws()
        {
            StringBuilder sb = null;
            Assert.ThrowsException<ArgumentException>(()=> sb.IndexOf('o'));
        }

        [TestMethod]
        public void IndexOf_ValidInputPhrase_ReturnsIndex()
        {
            var sb = new StringBuilder("The quick brown fox jumps over the lazy dog.");
            int index = sb.IndexOf("fox jumps");

            Assert.IsTrue(index == 16);
        }

        [TestMethod]
        public void IndexOf_ValidInputPhraseOverload_ReturnsIndex()
        {
            var sb = new StringBuilder("The quick brown fox jumps over the lazy dog.");
            int index = sb.IndexOf("fox jumps", 10);

            Assert.IsTrue(index == 16);
        }

        [TestMethod]
        public void IndexOf_ValidInputPhraseOverloadCount_ReturnsIndex()
        {
            var sb = new StringBuilder("The quick brown fox jumps over the lazy dog.");
            int index = sb.IndexOf("fox jumps", 10, sb.Length - 10);

            Assert.IsTrue(index == 16);
        }

        [TestMethod]
        public void IndexOf_PhraseOverloadCountOffSet_ReturnsIndex()
        {
            var sb = StringBuilderFactory();
            Assert.ThrowsException<IndexOutOfRangeException>(
                () => sb.IndexOf("fox jumps", 10, sb.Length + 10));
        }

        [TestMethod]
        public void IndexOf_EmptyPhrase_ReturnsIndex()
        {
            var sb = new StringBuilder();
            int index = sb.IndexOf("fox jumps", 10, sb.Length - 10);
            Assert.IsTrue(index == -1);
        }

        [TestMethod]
        public void IndexOf_NullPhrase_Throws()
        {
            var sb = StringBuilderFactory();
            Assert.ThrowsException<ArgumentException>(
                () => sb.IndexOf(null, 10, sb.Length + 10));
        }

        [TestMethod]
        public void IndexOf_IndexCount_ReturnsIndex()
        {
            var sb = StringBuilderFactory();
            Assert.ThrowsException<IndexOutOfRangeException>(
                () => sb.IndexOf("fox", sb.Length +1, sb.Length));
        }

        private StringBuilder StringBuilderFactory()
        {
            string content = "The quick brown fox jumps over the lazy dog.";
            return new StringBuilder(content);
        }



    }
}
