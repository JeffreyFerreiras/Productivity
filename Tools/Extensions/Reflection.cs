using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Tools.Extensions
{
    using Exceptions;

    public static class Reflection
    {
        public static dynamic GetPropertyVal(this object o, string property)
        {
            Guard.ThrowIfInvalidArgs(o.IsValid(), "object");
            Guard.ThrowIfInvalidArgs(property.IsValid(), nameof(property));

            PropertyInfo p = o.GetType().GetTypeInfo().GetProperty(property);

            if (p != null && p.CanRead)
            {
                dynamic val = p.GetValue(o);
                return val;
            }

            return Activator.CreateInstance(p.PropertyType); //something went wrong.
        }
    }
}
