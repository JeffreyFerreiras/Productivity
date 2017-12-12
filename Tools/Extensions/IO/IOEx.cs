using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Tools.Extensions;
using System.Text.RegularExpressions;
using Tools.Exceptions;

namespace Tools.Extensions.IO
{
    public static class IOEx
    {
        public static bool CreateDirectory(this string path)
        {
            if(!path.IsValidPath()) return false;
            
            return null != Directory.CreateDirectory(path);
        }
        
        public static bool IsValidPath(this string path)
        {
            if (!path.IsValid()) return false;
            
            char[] invalidChars = Path.GetInvalidPathChars();

            DirectoryInfo directoryInfo = null;

            if(!path.Any(x => invalidChars.Contains(x)))
            {
                directoryInfo = new DirectoryInfo(path);
            }

            return directoryInfo != null;
        }

        public static bool IsValidFileName(this string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return false;

            try
            {
                fileName = Path.GetFileName(fileName);
                HashSet<char> invalidChars = new HashSet<char>(Path.GetInvalidFileNameChars());

                return !fileName.Any(x => invalidChars.Contains(x));
            }
            catch
            {
                return false;
            }
        }
    }
}