using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Tools.Test
{
    [TestClass]
    public class XSerializerTests
    {
        #region ToXml

        [TestMethod]
        public void ToXml_ValidObject_ReturnsXmlString()
        {
            var catalog = new Catalog();
            var book = new Book();
            var book2 = new Book();

            DummyData.PopulateModel(book);
            DummyData.PopulateModel(book2);
            DummyData.PopulateModel(catalog);

            catalog.BookList.Add(book);
            catalog.BookList.Add(book2);

            string xml = XSerializer.ToXml(catalog);

            Assert.IsTrue(xml.Length > 0);
        }

        [TestMethod]
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

        [TestMethod]
        public void ToXml_ValidCollection_ReturnsXmlString()
        {
            ICollection<string> catalog = Helper.GeStringArray();

            string xml = XSerializer.ToXml(catalog);
            Assert.IsTrue(xml.Length > 0);
        }

        [TestMethod]
        public void ToXml_Null_ThrowsArgumentException()
        {
            Catalog catalog = null;
            Action toXml = () => XSerializer.ToXml(catalog);

            Assert.ThrowsException<ArgumentException>(toXml);
        }

        [TestMethod]
        public void ToXml_EmptyString_ThrowsArgumentException()
        {
            string catalog = "";
            Action toXml = () => XSerializer.ToXml(catalog);

            Assert.ThrowsException<ArgumentException>(toXml);
        }

        [TestMethod]
        public void ToXml_String_ThrowsInvalidOperation()
        {
            string catalog = File.ReadAllText("Books.xml");
            Action toXml = () => XSerializer.ToXml(catalog);

            Assert.ThrowsException<InvalidOperationException>(toXml);
        }

        #endregion ToXml

        #region BuildXmlSerializer

        [TestMethod]
        public void BuildXmlSerializer_ObjectType_ReturnsXmlSerializer()
        {
            SimpleFake fake = new SimpleFake();

            XmlSerializer ser = XSerializer.BuildXmlSerializer(fake);
            Assert.IsNotNull(ser);
        }

        [TestMethod]
        public void BuildXmlSerializer_CollectionType_ReturnsXmlSerializer()
        {
            ICollection<string> col = Helper.GeStringArray();

            XmlSerializer ser = XSerializer.BuildXmlSerializer(col);
            Assert.IsNotNull(ser);
        }


        [TestMethod]
        public void BuildXmlSerializer_ArrayType_ReturnsXmlSerializer()
        {
            var arr = Helper.GeStringArray();
            XmlSerializer ser = XSerializer.BuildXmlSerializer(arr);
            Assert.IsNotNull(ser);
        }

        [TestMethod]
        public void BuildXmlSerializer_StructType_ReturnsXmlSerializer()
        {
            var stru = new SampleStruct();

            XmlSerializer ser = XSerializer.BuildXmlSerializer(stru);
            Assert.IsNotNull(ser);
        }

        #endregion BuildXmlSerializer

        [TestMethod]
        public void FromXml_Xml_ReturnsObjectFromXml()
        {
            string xml = File.ReadAllText("books.xml");
            var catalog = XSerializer.FromXml<Catalog>(xml);

            Assert.IsNotNull(catalog);
        }
    }
}