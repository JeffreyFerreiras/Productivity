using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Tools.Extensions;

namespace Tools.Extensions.IO
{
    public static class FileExt
    {
        public static bool CreateDirectory(this string path)
        {
            if(!path.IsValid()) return false;
            
            if(path != Path.GetPathRoot(path))
            {
                DirectoryInfo parent = Directory.GetParent(path);
                parent?.FullName.CreateDirectory();
            }
            
            Directory.CreateDirectory(path);
            return Directory.Exists(path);
        }
    }
}