using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Tools.Extensions.IO
{
    public static class FileExt
    {
        public static bool CreateDirectory(string path)
        {
            FileAttributes attr = File.GetAttributes(path);

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                Directory.CreateDirectory(path);
            else
                Directory.CreateDirectory(Path.GetDirectoryName(path));

            return Directory.Exists(path);
        }
    }
}