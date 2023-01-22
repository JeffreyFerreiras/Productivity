using Productivity.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Productivity
{
    public static class IoHelper
    {
        /// <summary>
        /// Returns directory info for each level in the directory. Will return objects if the
        /// directory does not exist.
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static Stack<DirectoryInfo> GetDirectoryStack(string path)
        {
            Guard.AssertArgs(Directory.Exists(path), "directory not valid");

            var dirInfo = new DirectoryInfo(path);
            var stack = new Stack<DirectoryInfo>();

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
        /// <param name="path">
        /// </param>
        /// <param name="pattern">
        /// </param>
        /// <returns>
        /// </returns>
        public static string[] GetFiles(string path, string pattern = "*", SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        {
            Guard.AssertArgs(Directory.Exists(path), $"Does not exist \"{path}\"");

            return Directory.GetFiles(path, pattern, searchOptions);
        }

        /// <summary>
        /// Retrieve an array of file names that meet a spefied criteria based on the FileInfo properties.
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="predicate">
        /// </param>
        /// <returns>
        /// </returns>
        public static string[] GetFiles(string path, Func<FileInfo, bool> predicate)
        {
            return Directory.EnumerateFiles(path)
                .Where(f => predicate(new FileInfo(f)))
                .ToArray();
        }

        /// <summary>
        /// Get FileInfo collection from directory with optional filter.
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="pattern">
        /// </param>
        /// <returns>
        /// </returns>
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
        /// <param name="path">
        /// </param>
        /// <param name="pattern">
        /// </param>
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
        /// <param name="path">
        /// </param>
        /// <param name="predicate">
        /// </param>
        public static void DeleteFiles(string path, Func<FileInfo, bool> predicate)
        {
            Guard.AssertArgs(predicate != null, $"{nameof(predicate)} is null");
            Guard.AssertArgs(Directory.Exists(path), $"Location does not exist: {path}");

            var fileInfos = Directory.EnumerateFiles(path).Select(f => new FileInfo(f));

            foreach (var fi in fileInfos)
            {
                Debug.Assert(predicate != null, nameof(predicate) + " != null");

                if (predicate(fi))
                {
                    fi.Delete();
                }
            }
        }

        /// <summary>
        /// Builds a File Shared safe stream writer that will not allow other stream writers from
        /// blocking the file.
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static StreamWriter BuildFileShareSafeStreamWriter(string path)
        {
            return BuildFileShareSafeStreamWriter(path, FileMode.OpenOrCreate);
        }

        /// <summary>
        /// Builds a File Shared safe stream writer that will not allow other stream writers from
        /// blocking the file.
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="mode">
        /// </param>
        /// <returns>
        /// </returns>
        public static StreamWriter BuildFileShareSafeStreamWriter(string path, FileMode mode)
        {
            Guard.AssertArgs(Directory.Exists(Path.GetDirectoryName(path)), "Directory does not exist");

            return new StreamWriter(File.Open(
                path,
                mode,
                FileAccess.ReadWrite,
                FileShare.ReadWrite));
        }

        /// <summary>
        /// Determines if the system has access rights to write to the folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static bool CanWriteFolder(string folder)
        {
            try
            {
                string tempFile = Path.Combine(folder, Path.GetTempFileName());

                using (var stream = File.Open(
                    tempFile,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.ReadWrite))
                {
                    File.Delete(path: tempFile);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static void Run(string cmd)
        {
            try
            {
                var procStartInfo =
                    new ProcessStartInfo("cmd", "/c " + cmd);
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                var proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
            }
            catch (Exception)
            {
                // Log the exception
            }
        }
    }
}