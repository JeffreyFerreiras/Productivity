using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Tools.Attributes
{
    using Extensions.Validation;
    using System.Collections.Generic;

    public class DirectoryAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string dir = value as string;

            if(dir.IsNullOrWhiteSpace())
            {
                return false;
            }

            string root = Path.GetPathRoot(dir);
            var set = new HashSet<char>(dir);

            return !Path.GetInvalidPathChars().Any(c => set.Contains(c));
        }
    }
}