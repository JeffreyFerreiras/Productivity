using System;
using System.Collections;
using System.Reflection;

namespace Tools.Test

{
    public static class DummyData
    {
        public static readonly object s_syncLock = new object();
        public static readonly Random s_random = new Random();

        public static T PopulateModel<T>()
        {
            dynamic model = Activator.CreateInstance<T>();
            return PopulateModel(model);
        }

        public static T PopulateModel<T>(T model)
        {
            foreach (PropertyInfo propInfo in model.GetType().GetProperties())
            {
                var type = propInfo.PropertyType;

                if (type.GetTypeInfo().IsEnum) continue;
                if (!propInfo.CanWrite) continue;

                if (type.GetTypeInfo().IsValueType)
                    propInfo.SetValue(model, TryGetRandom(type));
                else if (type == typeof(string))
                {
                    string randString = RandomString.NextAlphabet(s_random.Next(5, 32));
                    propInfo.SetValue(model, randString);
                }
            }

            return model;
        }

        private static object TryGetRandom(Type type)
        {
            try
            {
                lock (s_syncLock)
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