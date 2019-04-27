using System.IO;
using System.Reflection;
using Tools.Exceptions;
using Tools.Extensions.IO;
using Tools.Extensions.Validation;

namespace Tools
{
    public static class AssemblyHelper
    {
        /// <summary>
        /// Loads a .NET component from assembly
        /// </summary>
        /// <param name="componentPath"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public static dynamic LoadNETAssembly(string componentPath, string module)
        {
            Guard.AssertArgs(module.IsValid(), nameof(module));
            Guard.AssertArgs(componentPath.IsValidPath(), nameof(componentPath));

            var assemblyRef = new AssemblyName
            {
                Name = Path.GetFileNameWithoutExtension(componentPath),
                ContentType = AssemblyContentType.Default,
            };

            Assembly assembly = Assembly.Load(assemblyRef);

            return assembly.CreateInstance(module, true);
        }
    }
}