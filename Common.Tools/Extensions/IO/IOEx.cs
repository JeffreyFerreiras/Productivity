using Common.Tools.Extensions.Validation;
using System.IO;
using System.Linq;

namespace Common.Tools.Extensions.IO
{
    public static class IoEx
    {
        /// <summary>
        /// Shorthand for creating a directory
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool CreateDirectory(this string path)
        {
            if (!path.IsValidPath()) return false;

            return null != Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Returns the directory information of the specified path string.
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetDirectoryName(this string path)
        {
            return Path.GetDirectoryName(path);
        }

        public static bool IsValidPath(this string path)
        {
            if (!path.IsValid()) return false;

            char[] invalidChars = Path.GetInvalidPathChars();

            DirectoryInfo directoryInfo = null;

            if (!path.Any(x => invalidChars.Contains(x)))
            {
                directoryInfo = new DirectoryInfo(path);
            }

            return directoryInfo != null;
        }

        /// <summary>
        /// Short-hand for validating file name.
        /// </summary>
        /// <param name="fileName">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsValidFileName(this string fileName)
        {
            fileName = Path.GetFileName(fileName);

            if (string.IsNullOrWhiteSpace(fileName) || fileName.Length > 255)
            {
                return false;
            }

            var invalidChars = Path.GetInvalidFileNameChars();

            foreach (char c in fileName)
            {
                if (invalidChars.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if file exists in the current directory.
        /// </summary>
        /// <param name="fileNamePattern">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool InCurrentDirectory(this string fileNamePattern)
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), fileNamePattern);

            return files.Length > 0;
        }
    }
}