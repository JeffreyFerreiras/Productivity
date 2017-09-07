using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Tools.Extensions;
using Tools.Exceptions;
using System.Text.RegularExpressions;

namespace Tools.Extensions.IO
{
    public static class IOEx
    {
        public static bool CreateDirectory(this string path)
        {
            if(!path.IsValidPath()) return false;
            
            return null != Directory.CreateDirectory(path);
        }
        
        //TODO: Create unit tests
        public static bool IsValidPath(this string path)
        {
            char[] invalidChars = Path.GetInvalidPathChars();
            return path.Any(x => invalidChars.Contains(x));
        }
    }
}