using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Tools.Exceptions;
using Tools.Extensions.Validation;

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
        public static string ToXml<T>(T model)
        {
            Guard.AssertArgs(model.IsValid(), nameof(model));
            Guard.AssertOperation(model.GetType() != typeof(string), "Cannot serialize type string");

            using(var writer = new System.IO.StringWriter())
            {
                XmlSerializer ser = BuildXmlSerializer(model);

                ser.Serialize(writer, model);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Creates <typeparamref name="T"/> XmlSerializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static XmlSerializer BuildXmlSerializer<T>(T model)
        {
            Type type = model.GetType();

            List<Type> extraTypes = GetCollectionTypes(model);

            extraTypes.AddRange(type.GenericTypeArguments);

            if(extraTypes.Any())
                return new XmlSerializer(type, extraTypes.ToArray());
            else
                return new XmlSerializer(type);
        }

        private static List<Type> GetCollectionTypes<T>(T model)
        {
            var types = new List<Type>();

            foreach(PropertyInfo prop in model.GetType().GetProperties())
            {
                if(!prop.CanRead) continue;

                object value = prop.GetValue(model);

                if(value is ICollection)
                {
                    types.Add(prop.PropertyType);
                }
            }

            return types;
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