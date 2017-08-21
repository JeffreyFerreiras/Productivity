using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace Tools
{
    using Exceptions;
    using Tools.Extensions;

    public static class AssemblyHelper
    {
        public static dynamic LoadNETAssembly(string componentPath, string module)
        {
            Guard.ThrowIfInvalidArgs(module.IsValid(), nameof(module));
            Guard.ThrowIfInvalidArgs(componentPath.IsValid(), nameof(componentPath));

            var assemblyRef = new AssemblyName
            {
                Name = Path.GetFileNameWithoutExtension(componentPath),
                ContentType = AssemblyContentType.Default,
            };

            Assembly assembly = Assembly.Load(assemblyRef);

            return assembly.CreateInstance(module, true); ;
        }
    }
}
