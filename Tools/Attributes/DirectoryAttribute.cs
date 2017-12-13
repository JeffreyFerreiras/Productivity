using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Tools.Attributes
{
    using Extensions.Validation;

    public class DirectoryAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string dir = value as string;
            string root = Path.GetPathRoot(dir);
            if (root.IsNullOrWhiteSpace()) return false;
            return !Path.GetInvalidPathChars().Any(c => dir.Contains(c));
        }
    }
}