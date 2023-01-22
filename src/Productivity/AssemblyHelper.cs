using Productivity.Exceptions;
using Productivity.Extensions.IO;
using Productivity.Extensions.Validation;
using System.IO;
using System.Reflection;

namespace Productivity
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

            var assembly = Assembly.Load(assemblyRef);

            dynamic instance = assembly.CreateInstance(module, true);

            return instance;
        }
    }
}