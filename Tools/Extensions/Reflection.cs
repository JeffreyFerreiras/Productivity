using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Tools.Extensions
{
    using Exceptions;

    public static class Reflection
    {
        public static dynamic GetPropertyVal<T>(this T value, string property)
        {
            Guard.ThrowIfInvalidArgs(value.IsValid(), nameof(value));
            Guard.ThrowIfInvalidArgs(property.IsValid(), nameof(property));

            PropertyInfo p = value.GetType().GetTypeInfo().GetProperty(property);

            if (p != null && p.CanRead)
            {
                dynamic val = p.GetValue(value);
                return val;
            }

            return Activator.CreateInstance(p.PropertyType); //something went wrong.
        }
    }
}
