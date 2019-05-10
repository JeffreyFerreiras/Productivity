using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;
using Tools.Exceptions;
using Tools.Extensions.Validation;

namespace Tools
{
    public static class JSerializer
    {
        /// <summary>
        /// Write object to JSON source type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToJson<T>(T source)
        {
            Guard.AssertArgs(source!=null, nameof(source));
            
            var ser = JsonSerializer.Create();

            using(var stringWriter = new StringWriter())
            using (var writer = new JsonTextWriter(stringWriter))
            {
                ser.Serialize(writer, source);
                writer.Flush();

                string json = stringWriter.ToString();
                return json;
            }
        }

        public static T FromJson<T>(string json)
        {
            Guard.AssertArgs(json.IsValid(), nameof(json));

            var ser = JsonSerializer.CreateDefault();

            using (var reader = new JsonTextReader(new StringReader(json)))
            {
                return ser.Deserialize<T>(reader);
            }
        }
    }
}
