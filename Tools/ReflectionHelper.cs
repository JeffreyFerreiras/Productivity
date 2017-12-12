using System;
using System.Collections.Generic;
using System.Text;

namespace Tools
{
    public static class ReflectionHelper
    {
        public static bool TryInstantiate<T>(out T item, params object[] args)
        {
            try
            {
                item = (T)Activator.CreateInstance(typeof(T), args);

                return true;
            }
            catch (Exception)
            {
                item = default(T);

                return false;
            }
        }

        
    }
}
