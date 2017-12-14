using System.IO;
using System.Reflection;

namespace Tools
{
    using Exceptions;
    using Extensions.Validation;
    using Extensions.IO;

    public static class AssemblyHelper
    {
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