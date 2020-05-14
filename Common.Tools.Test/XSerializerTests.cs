using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Common.Tools.Test
{
    [TestFixture]
    public class XSerializerTests
    {
        #region ToXml

        [Test]
        public void ToXml_ValidObject_ReturnsXmlString()
        {
            var catalog = new Catalog();
            var book = new Book();
            var book2 = new Book();

            Dummy.Populate(book);
            Dummy.Populate(book2);
            Dummy.Populate(catalog);

            catalog.BookList.Add(book);
            catalog.BookList.Add(book2);

            string xml = XSerializer.ToXml(catalog);

            Assert.IsTrue(xml.Length > 0);
        }

        [Test]
        public void ToXml_ValidStruct_ReturnsXmlString()
        {
            var catalog = new SampleStruct
            {
                Count = 50,
                FirstName = "Pablo",
                LastName = "Duartes",
                Time = DateTime.Now
            };

            string xml = XSerializer.ToXml(catalog);
            Assert.IsTrue(xml.IndexOf("SampleStruct") > 0);
        }

        [Test]
        public void ToXml_ValidCollection_ReturnsXmlString()
        {
            ICollection<string> catalog = Helper.GeStringArray();

            string xml = XSerializer.ToXml(catalog);
            Assert.IsTrue(xml.Length > 0);
        }

        [Test]
        public void ToXml_Null_ThrowsArgumentException()
        {
            Catalog catalog = null;

            Assert.Throws<ArgumentException>(() => XSerializer.ToXml(catalog));
        }

        [Test]
        public void ToXml_EmptyString_ThrowsArgumentException()
        {
            string catalog = "";

            Assert.Throws<ArgumentException>(() => XSerializer.ToXml(catalog));
        }

        [Test]
        public void ToXml_String_ThrowsInvalidOperation()
        {
            string catalog = File.ReadAllText("Books.xml");

            Assert.Throws<InvalidOperationException>(() => XSerializer.ToXml(catalog));
        }

        #endregion ToXml

        #region BuildXmlSerializer

        [Test]
        public void BuildXmlSerializer_ObjectType_ReturnsXmlSerializer()
        {
            SimpleFake fake = new SimpleFake();

            XmlSerializer ser = XSerializer.BuildXmlSerializer(fake);
            Assert.IsNotNull(ser);
        }

        [Test]
        public void BuildXmlSerializer_CollectionType_ReturnsXmlSerializer()
        {
            ICollection<string> col = Helper.GeStringArray();

            XmlSerializer ser = XSerializer.BuildXmlSerializer(col);
            Assert.IsNotNull(ser);
        }

        [Test]
        public void BuildXmlSerializer_ArrayType_ReturnsXmlSerializer()
        {
            var arr = Helper.GeStringArray();
            XmlSerializer ser = XSerializer.BuildXmlSerializer(arr);
            Assert.IsNotNull(ser);
        }

        [Test]
        public void BuildXmlSerializer_StructType_ReturnsXmlSerializer()
        {
            var stru = new SampleStruct
            {
                Count = 5,
                FirstName = "Bob",
                LastName = "Marly",
                Time = DateTime.Now,
            };

            XmlSerializer ser = XSerializer.BuildXmlSerializer(stru);
            Assert.IsNotNull(ser);
        }

        [Theory]
        [TestCase("")]
        [TestCase(null)]
        public void BuildXmlSerializer_InvalidArgs_ThrowsArgumentException(object arg)
        {
            Assert.Throws<ArgumentException>(() => XSerializer.BuildXmlSerializer(arg));
        }

        #endregion BuildXmlSerializer

        #region FromXml

        [Test]
        public void FromXml_Xml_ReturnsObjectFromXml()
        {
            string xml = File.ReadAllText("books.xml");
            var catalog = XSerializer.FromXml<Catalog>(xml);

            Assert.IsNotNull(catalog);
        }

        [Theory]
        [TestCase("")]
        [TestCase(null)]
        public void FromXml_InvalidArgs_ThrowsArgumentException(string xml)
        {
            Assert.Throws<ArgumentException>(() => XSerializer.FromXml<Catalog>(xml));
        }

        #endregion FromXml
    }
}