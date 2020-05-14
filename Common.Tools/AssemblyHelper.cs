using Common.Tools.Exceptions;
using Common.Tools.Extensions.IO;
using Common.Tools.Extensions.Validation;
using System.IO;
using System.Reflection;

namespace Common.Tools
{
    public static class AssemblyHelper
    {
        /// <summary>
        /// Loads a .NET component from assembly
        /// </summary>
        /// <param name="componentPath">
        /// </param>
        /// <param name="module">
        /// </param>
        /// <returns>
        /// </returns>
        public static dynamic LoadNetAssembly(string componentPath, string module)
        {
            Guard.AssertArgs(module.IsValid(), nameof(module));
            Guard.AssertArgs(componentPath.IsValidPath(), nameof(componentPath));

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