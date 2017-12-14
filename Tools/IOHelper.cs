using System.Collections.Generic;
using System.IO;

namespace Tools
{
    using Extensions;
    using System.Diagnostics;
    using Tools.Exceptions;
    using Tools.Extensions.IO;

    public static class IOHelper
    {
        /// <summary>
        /// Returns directory info for each level in the directory.
        /// Will return objects if the directory does not exist.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Stack<DirectoryInfo> GetDirectoryStack(string path)
        {
            Guard.AssertArgs(Directory.Exists(path), "directory not valid");

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            Stack<DirectoryInfo> stack = new Stack<DirectoryInfo>();

            while (dirInfo != null)
            {
                stack.Push(dirInfo);
                dirInfo = dirInfo?.Parent;
            }

            return stack;
        }

        public static string[] GetFiles(string path, string pattern = "*")
        {
            Guard.AssertArgs(Directory.Exists(path), $"Does not exist \"{path}\"");

            return Directory.GetFiles(path, pattern);
        }

        public static IEnumerable<FileInfo> GetFilesInfo(string path, string pattern = "*")
        {
            Guard.AssertArgs(Directory.Exists(path), $"Location does not exist: {path}");

            if(Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path, pattern);

                foreach(string file in files)
                {
                    yield return new FileInfo(file);
                }
            }
        }

        public static void DeleteFiles(string path, string pattern = "*")
        {
            Guard.AssertArgs(Directory.Exists(path), $"Location does not exist: {path}");

            string[] files = GetFiles(path, pattern);

            foreach(var f in files)
            {
                File.Delete(f);
            }
        }
    }
}