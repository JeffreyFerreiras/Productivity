using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tools.Exceptions;

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

            while (dirInfo != null)
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
        public static string[] GetFiles(string path, string pattern = "*", SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        {
            Guard.AssertArgs(Directory.Exists(path), $"Does not exist \"{path}\"");

            return Directory.GetFiles(path, pattern, searchOptions);
        }

        /// <summary>
        /// Retrieve an array of file names that meet a spefied criteria
        /// based on the FileInfo properties.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static string[] GetFiles(string path, Func<FileInfo, bool> predicate)
        {
            return Directory.EnumerateFiles(path)
                .Where(f => predicate(new FileInfo(f)))
                .ToArray();
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

            foreach (string file in files)
            {
                yield return new FileInfo(file);
            }
        }

        /// <summary>
        /// Delete files within a directory with optional pattern.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        public static void DeleteFiles(string path, string pattern = "*")
        {
            Guard.AssertArgs(Directory.Exists(path), $"Location does not exist: {path}");

            string[] files = GetFiles(path, pattern);

            foreach (var f in files)
            {
                File.Delete(f);
            }
        }

        /// <summary>
        /// Delete files in a directory that meet specified conditions.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="predicate"></param>
        public static void DeleteFiles(string path, Func<FileInfo, bool> predicate)
        {
            Guard.AssertArgs(predicate != null, $"{nameof(predicate)} is null");
            Guard.AssertArgs(Directory.Exists(path), $"Location does not exist: {path}");

            var fileInfos = Directory.EnumerateFiles(path).Select(f => new FileInfo(f));

            foreach (var fi in fileInfos)
            {
                if (predicate(fi))
                {
                    fi.Delete();
                }
            }
        }

        /// <summary>
        /// Builds a File Shared safe tream writer that will not
        /// allow other stream writers from blocking the file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static StreamWriter BuildFileShareSafeStreamWriter(string path)
        {
            Guard.AssertArgs(Directory.Exists(Path.GetDirectoryName(path)), "Directory does not exist");

            return new StreamWriter(File.Open(
                    path,
                    FileMode.OpenOrCreate,
                    FileAccess.Write,
                    FileShare.ReadWrite));
        }
    }
}