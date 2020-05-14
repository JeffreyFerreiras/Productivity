using Common.Tools.Exceptions;
using Common.Tools.Extensions.Validation;
using Newtonsoft.Json;
using System.IO;

namespace Common.Tools
{
    public static class JSerializer
    {
        /// <summary>
        /// Write object to JSON source type
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
        public static string ToJson<T>(T source)
        {
            Guard.AssertArgs(source != null, nameof(source));

            var ser = JsonSerializer.Create();

            using (var stringWriter = new StringWriter())
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

            using (var stringReader = new StringReader(json))
            using (var reader = new JsonTextReader(stringReader))
            {
                return ser.Deserialize<T>(reader);
            }
        }
    }
}