using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using Tools.Extensions.Text;
using Tools.Extensions.Conversion;
using System.Linq;

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

            Assert.ThrowsException<ArgumentException>(() => sb.IndexOf('o'));
        }

        [TestMethod]
        public void IndexOf_Phrase_ReturnsIndex()
        {
            var sb = StringBuilderFactory();
            int index = sb.IndexOf("fox jumps");

            Assert.IsTrue(index == 16);
        }

        [TestMethod]
        public void IndexOf_PhraseStartInMiddle_ReturnsIndex()
        {
            var sb = StringBuilderFactory();
            int index = sb.IndexOf("fox jumps", 10);

            Assert.IsTrue(index == 16);
        }

        [TestMethod]
        public void IndexOf_StartInMiddleWithCount_ReturnsIndex()
        {
            var sb = StringBuilderFactory();
            int index = sb.IndexOf("fox jumps", 10, sb.Length - 10);

            Assert.IsTrue(index == 16);
        }

        [TestMethod]
        public void IndexOf_InvalidCount_Throws()
        {
            var sb = StringBuilderFactory();

            Assert.ThrowsException<IndexOutOfRangeException>(() => sb.IndexOf("fox jumps", 10, sb.Length + 10));
        }

        [TestMethod]
        public void IndexOf_EmptyPhrase_ReturnsIndex()
        {
            var sb = StringBuilderFactory();

            int index = sb.IndexOf("", 10, sb.Length - 10);

            Assert.IsTrue(index == -1);
        }

        [TestMethod]
        public void IndexOf_NullPhrase_Throws()
        {
            var sb = StringBuilderFactory();

            Assert.ThrowsException<ArgumentException>(() => sb.IndexOf(null, 10, 10));
        }

        [TestMethod]
        public void IndexOf_InvalidStartIndex_ThrowsException()
        {
            var sb = StringBuilderFactory();

            Assert.ThrowsException<IndexOutOfRangeException>(() => sb.IndexOf("fox", sb.Length + 1, sb.Length));
        }

        [TestMethod]
        public void IndexOfAny_Chars_ReturnsIndex()
        {
            StringBuilder sb = StringBuilderFactory();
            char[] anyOf = new char[] { 'a', 'b', 'c' };

            int index = sb.IndexOfAny(anyOf);
            Assert.IsTrue(index == 7);
        }

        [TestMethod]
        public void IndexOfAny_ValidStartIndex_ReturnsIndex()
        {
            StringBuilder sb = StringBuilderFactory();
            char[] anyOf = new char[] { 'a', 'b', 'c' };

            int index = sb.IndexOfAny(anyOf, sb.Length / 2);
            Assert.IsFalse(index == -1);
        }

        [TestMethod]
        public void IndexOfAny_InvalidStartIndex_Throws()
        {
            StringBuilder sb = StringBuilderFactory();
            char[] anyOf = new char[] { 'a', 'b', 'c' };

            Assert.ThrowsException<IndexOutOfRangeException>(()=> sb.IndexOfAny(anyOf, sb.Length +1 ));
        }

        [TestMethod]
        public void IndexOfAny_InvalidCount_Throws()
        {
            StringBuilder sb = StringBuilderFactory();
            char[] anyOf = new char[] { 'a', 'b', 'c' };

            Assert.ThrowsException<IndexOutOfRangeException>(() => sb.IndexOfAny(anyOf, sb.Length / 2, sb.Length +1));
        }

        [TestMethod]
        public void LastIndexOf_StringBuilder_ReturnsLastIndex()
        {
            StringBuilder sb = StringBuilderFactory();
            char value = 'o';
            int lastIndex = sb.LastIndexOf(value);

            Assert.AreEqual(sb.Length - 3, lastIndex);
        }

        [TestMethod]
        public void LastIndexOf_Null_ReturnsLastIndex()
        {
            StringBuilder sb = null;
            char value = 'o';

            Assert.ThrowsException<NullReferenceException>(()=>sb.LastIndexOf(value));
        }

        [TestMethod]
        public void LastIndexOf_StartIndex_ReturnsLastIndex()
        {
            StringBuilder sb = StringBuilderFactory();
            char value = 'o';
            int lastIndex = sb.LastIndexOf(value, 8);

            Assert.AreEqual(sb.Length - 3, lastIndex);
        }

        private StringBuilder StringBuilderFactory()
        {
            string content = 
                "The quick brown fox jumps over the lazy dog."+
                "The quick brown fox jumps over the lazy dog.";

            return new StringBuilder(content);
        }
    }
}