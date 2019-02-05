using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

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
            //Create a stream to serialize the object to
            MemoryStream ms = new MemoryStream();

            // Serializer the User object to the stream
            var ser = new DataContractJsonSerializer(typeof(T));

            ser.WriteObject(ms, source);
           
            byte[] json = ms.ToArray();

            ms.Close();

            return Encoding.UTF8.GetString(json, 0, json.Length);
        }
    }
}
