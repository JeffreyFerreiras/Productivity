using NUnit.Framework;
using Productivity.Text;
using System;
using System.Linq;

namespace Productivity.UnitTests.Text
{
    [TestFixture]
    public class EditorTests
    {
        [Test]
        public void Mask_ValidString_ReturnsMaskedString()
        {
            string source = "123456789";
            string result = Editor.Mask(source);

            string maskedChars = result.Substring(0, result.Length - 4);

            Assert.IsTrue(maskedChars.All(c => c == '*'));
        }

        [Test]
        public void Mask_NegativeNumber_ReturnsSame()
        {
            int showlast = -4;
            string source = "123456789";
            string result = Editor.Mask(source, showlast);

            Assert.AreEqual(source, result);
        }

        [Test]
        public void Mask_MaskLongerThanString_ReturnsSame()
        {
            int showlast = 10;
            string source = "12345";
            string result = Editor.Mask(source, showlast);

            Assert.IsTrue(source == result);
        }

        [Test]
        public void Mask_Null_ReturnsSame()
        {
            int showlast = 4;
            string source = null;

            string result = Editor.Mask(source, showlast);

            Assert.IsTrue(source == result);
        }

        [Test]
        public void RemoveHtmlXmlTags_ValidHTML_RemovesPTag()
        {
            string html =
                @"<!DOCTYPE html>
                   <html>
                    <body>

                        <h1>My First Heading</h1>

                        <p style='text - align:right'> My first paragraph.</p>

                      </body>
                   </html>";

            string result = Editor.RemoveHtmlTag(html, "<p>");

            Assert.IsTrue(result.IndexOf("<p>") == -1);
            Assert.IsTrue(result.IndexOf("</p>") == -1);
        }

        [Test]
        public void RemoveHtmlXmlTags_ValidHTML_RemovesBodyTag()
        {
            string html =
                @"<!DOCTYPE html>
                   <html>
                    <body>

                        <h1>My First Heading</h1>

                        <p style='text - align:right'> My first paragraph.</p>

                      </body>
                   </html>";

            string tag = "<body>";
            string result = Editor.RemoveHtmlTag(html, tag);

            Assert.IsTrue(result.IndexOf(tag) == -1);
            Assert.IsTrue(result.IndexOf("</body>") == -1);
        }

        [Test]
        public void RemoveHtmlXmlTags_MissingTag_ReturnsSameHtml()
        {
            string html =
                @"<!DOCTYPE html>
                   <html>
                    <body>

                        <h1>My First Heading</h1>

                        <p style='text - align:right'> My first paragraph.</p>

                      </body>
                   </html>";

            string tag = "<bubahhh>";
            string result = Editor.RemoveHtmlTag(html, tag);

            Assert.IsTrue(result == html);
        }

        [Test]
        public void RemoveHtmlXmlTags_InvalidInput_ReturnsEarly()
        {
            string html = null;
            string tag = null;

            Assert.Throws<ArgumentException>(() => Editor.RemoveHtmlTag(html, tag));
            Assert.Throws<ArgumentException>(() => Editor.RemoveHtmlTag("<body></body>", tag));
        }

        [Test]
        public void Strip_ValidString_StripsChars()
        {
            string text = @"!Hello! World!";
            string result = Editor.Strip(text, '!');

            Assert.IsTrue(result.IndexOf('!') == -1);
        }

        [Test]
        public void Strip_ValidString_StripsMultipleChars()
        {
            string text = @"!Hello World!llll";
            string result = Editor.Strip(text, '!', 'l');

            Assert.IsTrue(result.IndexOf('!') == -1);
            Assert.IsTrue(result.IndexOf('l') == -1);
        }

        [Test]
        public void Strip_CharsNotPresent_ReturnsUnchanged()
        {
            string text = @"!Hello World!llll";
            string result = Editor.Strip(text, 'p', 'k');

            Assert.IsTrue(text == result);
        }

        [Test]
        public void Strip_InvalidInput_ReturnsUnchanged()
        {
            string text = null;
            string result = Editor.Strip(text, 'p', 'k');

            Assert.IsTrue(text == result);
        }

        [Test]
        public void Strip_ValidString_StripsPhrase()
        {
            string text = "The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog";
            string phrase = "fox jumps";
            string result = Editor.Strip(text, phrase);

            Assert.IsTrue(result.IndexOf(phrase) == -1);
        }

        [Test]
        public void Strip_LargeString_StripsPhrase()
        {
            string text = "The quick brown fox jumps over the lazy dog.";
            text = string.Join(" ", Enumerable.Repeat(text, 1000));

            string phrase = "fox jumps";

            string result = Editor.Strip(text, phrase);
            Assert.IsTrue(result.IndexOf(phrase) == -1);
        }

        [Theory]
        public void Strip_MultiplePhrases_StripsText()
        {
            string text = "The quick brown fox jumps over the lazy dog.";
            string[] phrases = { "quick", "brown", "fox" };

            string result = Editor.Strip(text, "quick", "brown", "fox");

            foreach (var phrase in phrases)
            {
                Assert.IsFalse(result.Contains(phrase));
            }
        }
    }
}