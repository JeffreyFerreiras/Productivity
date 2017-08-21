using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Tools
{
    public static class IOHelper
    {
        public static Stack<DirectoryInfo> GetDirectoryStack(string dir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            Stack<DirectoryInfo> stack = new Stack<DirectoryInfo>();

            while (dirInfo != null)
            {
                stack.Push(dirInfo);
                dirInfo = dirInfo?.Parent;
            }

            return stack;
        }
    }
}
