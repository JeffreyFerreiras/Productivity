using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Tools.Extensions
{
    public static class Reflection
    {
        public static dynamic GetPropertyVal(this object o, string property)
        {
            PropertyInfo p = o.GetType().GetTypeInfo().GetProperty(property);

            if (p != null && p.CanRead)
            {
                dynamic val = p.GetValue(o);
                return val;
            }

            return -1; //something went wrong.
        }
    }
}
