using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Tools.Extensions;

namespace Tools.Extensions.IO
{
    public static class IOEx
    {
        public static bool CreateDirectory(this string path)
        {
            if(!path.IsValid()) return false;
            
            return null != Directory.CreateDirectory(path);
        }
    }
}