using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Tools.Attributes
{
    using Extensions;

    public class Directory : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string dir = value as string;
            string root = Path.GetPathRoot(dir);
            if (root.IsNullOrWhiteSpace()) return false;
            return ! Path.GetInvalidPathChars().Any(c => dir.Contains(c));
        }
    }
}
