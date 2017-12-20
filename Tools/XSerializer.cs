using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Tools.Extensions.Reflection;

namespace Tools
{
    public static class XSerializer
    {
        /// <summary>
        /// Serializes <typeparamref name="T"/> into xml string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ToXml<T>(T model) where T : class
        {
            using(var writer = new System.IO.StringWriter())
            {
                XmlSerializer ser = BuildXmlSerializer<T>();

                ser.Serialize(writer, model);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Creates <typeparamref name="T"/> XmlSerializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static XmlSerializer BuildXmlSerializer<T>()
        {
            Type type = typeof(T);
            Type[] extraTypes = type.HasCollection() 
                ? type.GetCollectionTypes().ToArray() 
                : null;
            
            XmlSerializer ser;

            if(extraTypes == null)
                ser = new XmlSerializer(type);
            else
                ser = new XmlSerializer(type, extraTypes);

            return ser;
        }

        /// <summary>
        /// De-serializes an XML string into specified type.
        /// Parent object name must fully match with XML root attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T FromXml<T>(string xml) where T : class
        {
            using(var stringReader = new System.IO.StringReader(xml))
            using(var xmlReader = XmlReader.Create(stringReader))
            {
                var xRoot = new XmlRootAttribute
                {
                    ElementName = typeof(T).Name,
                    IsNullable = true
                };

                var ser = new XmlSerializer(typeof(T), xRoot);
                return (T)ser.Deserialize(xmlReader);
            }
        }
    }
}