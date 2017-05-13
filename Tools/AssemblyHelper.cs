﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace Tools
{
    using Exceptions;

    public static class AssemblyHelper
    {
        public static dynamic LoadNETAssembly(string componentPath, string module)
        {
            Guard.ThrowIfInvalidArgs(componentPath, module);

            var assemblyRef = new AssemblyName
            {
                Name = Path.GetFileNameWithoutExtension(componentPath),
                ContentType = AssemblyContentType.Default,
            };

            Assembly assembly = Assembly.Load(assemblyRef);
            dynamic instance = assembly.CreateInstance(module, true);

            return instance;
        }
    }
}
