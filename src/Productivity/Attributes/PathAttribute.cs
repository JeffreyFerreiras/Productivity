using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using Productivity.Extensions.Validation;

namespace Productivity.Attributes
{
    /// <summary>
    /// Directory Attribute to be used for validating if a property is a valid directory name. Can only be used with string types.
    /// </summary>
    /// <exception cref="ArgumentException">Can only be used with string types</exception>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PathAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is not null && value is not string directoryName)
            {
                throw new ArgumentException($"{nameof(PathAttribute)} can only be used on string types");
            }

            string dir = value as string;

            if (dir.IsNullOrWhiteSpace())
            {
                return false;
            }

            string root = Path.GetPathRoot(dir);
            var set = new HashSet<char>(dir);

            return !Path.GetInvalidPathChars().Any(c => set.Contains(c));
        }
    }
}