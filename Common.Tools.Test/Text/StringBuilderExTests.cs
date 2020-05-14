using Common.Tools.Extensions.Text;
using NUnit.Framework;
using System;
using System.Text;

namespace Common.Tools.Test.Text
{
    [TestFixture]
    public class StringBuilderExTests
    {
        private const string Pangram = "The quick brown fox jumps over the lazy dog.";

        private StringBuilder StringBuilderFactory()
        {
            string content = Pangram + Pangram;

            return new StringBuilder(content);
        }

        [Test]
        public void IndexOf_ValidInput_ReturnsIndex()
        {
            var sb = new StringBuilder("Hello World!");
            int index = sb.IndexOf('o');

            Assert.IsTrue(index == 4);
        }

        [Test]
        public void IndexOf_Empty_ReturnsIndex()
        {
            var sb = new StringBuilder("");
            int index = sb.IndexOf('o');

            Assert.IsTrue(index == -1);
        }

        [Test]
        public void IndexOf_Null_Throws()
        {
            StringBuilder sb = null;

            Assert.Throws<ArgumentException>(() => sb.IndexOf('o'));
        }

        [Test]
        public void IndexOf_Phrase_ReturnsIndex()
        {
            var sb = StringBuilderFactory();
            int index = sb.IndexOf("fox jumps");

            Assert.IsTrue(index == 16);
        }

        [Test]
        public void IndexOf_PhraseStartInMiddle_ReturnsIndex()
        {
            var sb = StringBuilderFactory();
            int index = sb.IndexOf("fox jumps", 10);

            Assert.IsTrue(index == 16);
        }

        [Test]
        public void IndexOf_StartInMiddleWithCount_ReturnsIndex()
        {
            var sb = StringBuilderFactory();
            int index = sb.IndexOf("fox jumps", 10, sb.Length - 10);

            Assert.IsTrue(index == 16);
        }

        [Test]
        public void IndexOf_InvalidCount_Throws()
        {
            var sb = StringBuilderFactory();

            Assert.Throws<IndexOutOfRangeException>(() => sb.IndexOf("fox jumps", 10, sb.Length + 10));
        }

        [Test]
        public void IndexOf_EmptyPhrase_ReturnsIndex()
        {
            var sb = StringBuilderFactory();

            int index = sb.IndexOf("", 10, sb.Length - 10);

            Assert.IsTrue(index == -1);
        }

        [Test]
        public void IndexOf_NullPhrase_Throws()
        {
            var sb = StringBuilderFactory();

            Assert.Throws<ArgumentException>(() => sb.IndexOf(null, 10, 10));
        }

        [Test]
        public void IndexOf_InvalidStartIndex_Throws()
        {
            var sb = StringBuilderFactory();

            Assert.Throws<IndexOutOfRangeException>(() => sb.IndexOf("fox", sb.Length + 1, sb.Length));
        }

        [Test]
        public void IndexOfAny_Chars_ReturnsIndex()
        {
            StringBuilder sb = StringBuilderFactory();
            char[] anyOf = new char[] { 'a', 'b', 'c' };

            int index = sb.IndexOfAny(anyOf);
            Assert.IsTrue(index == 7);
        }

        [Test]
        public void IndexOfAny_ValidStartIndex_ReturnsIndex()
        {
            StringBuilder sb = StringBuilderFactory();
            char[] anyOf = new char[] { 'a', 'b', 'c' };

            int index = sb.IndexOfAny(anyOf, sb.Length / 2);
            Assert.IsFalse(index == -1);
        }

        [Test]
        public void IndexOfAny_InvalidStartIndex_Throws()
        {
            StringBuilder sb = StringBuilderFactory();
            char[] anyOf = new char[] { 'a', 'b', 'c' };

            Assert.Throws<IndexOutOfRangeException>(() => sb.IndexOfAny(anyOf, sb.Length + 1));
        }

        [Test]
        public void IndexOfAny_InvalidCount_Throws()
        {
            StringBuilder sb = StringBuilderFactory();
            char[] anyOf = new char[] { 'a', 'b', 'c' };

            Assert.Throws<IndexOutOfRangeException>(() => sb.IndexOfAny(anyOf, sb.Length / 2, sb.Length + 1));
        }

        [Test]
        public void LastIndexOf_StringBuilder_ReturnsLastIndex()
        {
            StringBuilder sb = StringBuilderFactory();
            char value = 'o';
            int lastIndex = sb.LastIndexOf(value);

            Assert.AreEqual(sb.Length - 3, lastIndex);
        }

        [Test]
        public void LastIndexOf_Null_ReturnsLastIndex()
        {
            StringBuilder sb = null;
            char value = 'o';

            Assert.Throws<ArgumentException>(() => sb.LastIndexOf(value));
        }

        [Test]
        public void LastIndexOf_StartIndex_ReturnsLastIndex()
        {
            StringBuilder sb = StringBuilderFactory();
            char value = 'o';
            int lastIndex = sb.LastIndexOf(value, 8);

            Assert.AreEqual(sb.Length - 3, lastIndex);
        }

        [Test]
        public void LastIndexOf_StartIndexAndCount_ReturnsLastIndex()
        {
            StringBuilder sb = StringBuilderFactory();

            char value = 'o';
            int expected = (sb.Length / 2) - 3;
            int startIndex = 8;
            int count = "The quick brown fox jumps over the lazy dog.".Length - startIndex;
            int lastIndex = sb.LastIndexOf(value, startIndex, count);

            Assert.AreEqual(expected, lastIndex);
        }

        [Test]
        public void LastIndexOf_IndexOutOfBounds_Throws()
        {
            StringBuilder sb = StringBuilderFactory();

            int start = sb.Length - 10;
            int count = 12;

            Assert.Throws<IndexOutOfRangeException>(() => sb.LastIndexOf('b', start, count));
        }

        [Test]
        public void LastIndexOf_StartOutOfBounds_Throws()
        {
            StringBuilder sb = StringBuilderFactory();

            int start = 0;
            int count = sb.Length + 1;

            Assert.Throws<IndexOutOfRangeException>(() => sb.LastIndexOf('b', start, count));
        }

        [Test]
        public void LastIndexOfAny_Chars_ReturnsIndex()
        {
            StringBuilder sb = StringBuilderFactory();

            char[] anyOf = new char[] { 'x', 'y', 'z' };
            int index = sb.LastIndexOfAny(anyOf);

            Assert.AreEqual(index, sb.Length - 6);
        }

        [Test]
        public void LastIndexOfAny_StartPosMiddle_ReturnsIndex()
        {
            StringBuilder sb = StringBuilderFactory();

            char[] anyOf = new char[] { 'x', 'y', 'z' };

            int index = sb.LastIndexOfAny(anyOf, Pangram.Length - 1);
            int expected = (sb.Length) - 6;

            Assert.AreEqual(index, expected);
        }

        [Test]
        public void LastIndexOfAny_StartPosAndCount_ReturnsIndex()
        {
            StringBuilder sb = StringBuilderFactory();

            char[] anyOf = new char[] { 'x', 'y', 'z' };

            int index = sb.LastIndexOfAny(anyOf, 0, Pangram.Length - 1);
            int expected = (sb.Length / 2) - 6;

            Assert.AreEqual(index, expected);
        }

        [Test]
        public void LastIndexOfAny_Null_Throws()
        {
            StringBuilder sb = null;

            char[] anyOf = new char[] { 'x', 'y', 'z' };

            Assert.Throws<ArgumentException>(() => sb.LastIndexOfAny(anyOf));
        }

        [Test]
        public void LastIndexOfAny_StartOutOfBounds_Throws()
        {
            StringBuilder sb = StringBuilderFactory();
            char[] anyOf = new char[] { 'x', 'y', 'z' };

            Assert.Throws<IndexOutOfRangeException>(() => sb.LastIndexOfAny(anyOf, sb.Length + 1));
        }

        [Test]
        public void LastIndexOfAny_CountOutOfBounds_Throws()
        {
            StringBuilder sb = StringBuilderFactory();
            char[] anyOf = new char[] { 'x', 'y', 'z' };

            Assert.Throws<IndexOutOfRangeException>(() => sb.LastIndexOfAny(anyOf, 0, sb.Length + 1));
        }

        [Test]
        public void LastIndexOfAny_StartCountOutOfBounds_Throws()
        {
            StringBuilder sb = StringBuilderFactory();
            char[] anyOf = new char[] { 'x', 'y', 'z' };

            Assert.Throws<IndexOutOfRangeException>(() => sb.LastIndexOfAny(anyOf, sb.Length - 15, 20));
        }

        [Test]
        public void LastIndexOf_PhraseStartCount_ReturnsIndex()
        {
            StringBuilder sb = StringBuilderFactory();
            string phrase = "brown";

            int index = sb.LastIndexOf(phrase, "The quick".Length - 1, " brown fox".Length);
            int expected = "The quick ".Length;

            Assert.AreEqual(index, expected);
        }

        [Test]
        public void LastIndexOf_PhraseStart_ReturnsIndex()
        {
            StringBuilder sb = new StringBuilder(Pangram);
            string phrase = "brown";

            int index = sb.LastIndexOf(phrase, "The quick ".Length);
            int expected = "The quick ".Length;

            Assert.AreEqual(index, expected);
        }

        [Test]
        public void LastIndexOf_Phrase_ReturnsIndex()
        {
            StringBuilder sb = new StringBuilder(Pangram);
            string phrase = "brown";

            int index = sb.LastIndexOf(phrase);
            int expected = "The quick ".Length;

            Assert.AreEqual(index, expected);
        }

        [Test]
        public void LastIndexOf_PhraseNull_ReturnsIndex()
        {
            StringBuilder sb = null;
            string phrase = "brown";

            Assert.Throws<ArgumentException>(() => sb.LastIndexOf(phrase));
        }

        [Test]
        public void LastIndexOf_PhraseStartOutOfBounds_ReturnsIndex()
        {
            StringBuilder sb = StringBuilderFactory();
            string phrase = "brown";

            Assert.Throws<IndexOutOfRangeException>(() => sb.LastIndexOf(phrase, sb.Length + 5));
        }

        [Test]
        public void LastIndexOf_PhraseCountOutOfBounds_ReturnsIndex()
        {
            StringBuilder sb = StringBuilderFactory();
            string phrase = "brown";

            Assert.Throws<IndexOutOfRangeException>(() => sb.LastIndexOf(phrase, 0, sb.Length + 5));
        }

        [Test]
        public void LastIndexOf_PhraseStartCountOutOfBounds_ReturnsIndex()
        {
            StringBuilder sb = StringBuilderFactory();
            string phrase = "brown";

            Assert.Throws<IndexOutOfRangeException>(() => sb.LastIndexOf(phrase, sb.Length - 5, 10));
        }
    }
}