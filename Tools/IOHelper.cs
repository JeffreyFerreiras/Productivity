using System.Collections.Generic;
using System.IO;
using Tools.Exceptions;
using Tools.Extensions.IO;

namespace Tools
{
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

            while(dirInfo != null)
            {
                stack.Push(dirInfo);
                dirInfo = dirInfo?.Parent;
            }

            return stack;
        }

        /// <summary>
        /// Get files from directory with optional filter.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string[] GetFiles(string path, string pattern = "*")
        {
            Guard.AssertArgs(Directory.Exists(path), $"Does not exist \"{path}\"");

            return Directory.GetFiles(path, pattern);
        }

        /// <summary>
        /// Get FileInfo collection from directory with optional filter.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> GetFileInfoCollection(string path, string pattern = "*")
        {
            Guard.AssertArgs(Directory.Exists(path), $"Location does not exist: {path}");

            string[] files = Directory.GetFiles(path, pattern);

            foreach(string file in files)
            {
                yield return new FileInfo(file);
            }
        }

        /// <summary>
        /// Delete files within a directory with optional filter
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        public static void DeleteFiles(string path, string pattern = "*")
        {
            Guard.AssertArgs(Directory.Exists(path), $"Location does not exist: {path}");

            string[] files = GetFiles(path, pattern);

            foreach(var f in files)
            {
                File.Delete(f);
            }
        }

        public static StreamWriter BuildFileShareSafeStreamWriter(string path)
        {
            Guard.AssertArgs(path.IsValidFileName(), "Invalid path");
            Guard.AssertArgs(Directory.Exists(Path.GetDirectoryName(path)), "File does not exist");
            
            return new StreamWriter(File.Open(
                    path,
                    FileMode.OpenOrCreate,
                    FileAccess.Write,
                    FileShare.ReadWrite));
        }
    }
}