using System;
using System.Reflection;
using Tools.RandomGenerator;

namespace Tools.Test
{
    /// <summary>
    /// Supports creation of randomized dummy test data.
    /// </summary>
    public static class Dummy
    {
        public static readonly object s_syncLock = new object();
        public static readonly Random s_random = new Random();

        /// <summary>
        /// Creates a test dummy of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>() where T : class
        {
            T model = Activator.CreateInstance<T>();

            return Populate(model);
        }

        /// <summary>
        /// Populate the properties of an object with random test data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T Populate<T>(T model) where T : class
        {
            foreach(PropertyInfo propInfo in model.GetType().GetProperties())
            {
                var type = propInfo.PropertyType;

                if(type.GetTypeInfo().IsEnum) continue;
                if(!propInfo.CanWrite) continue;

                if(type.GetTypeInfo().IsValueType)
                {
                    propInfo.SetValue(model, GetRandomOrDefault(type));
                }
                else if(type == typeof(string))
                {
                    string randString = RandomString.NextAlphabet(s_random.Next(5, 32));
                    propInfo.SetValue(model, randString);
                }
            }

            return model;
        }

        private static dynamic GetRandomOrDefault(Type type)
        {
            try
            {
                lock(s_syncLock)
                {
                    int rand = s_random.Next(1, 100);

                    return Convert.ChangeType(rand, type);
                }
            }
            catch
            {
                return Activator.CreateInstance(type);
            }
        }
    }
}