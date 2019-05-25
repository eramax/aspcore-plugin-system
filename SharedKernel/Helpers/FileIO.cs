using System.IO;

namespace SharedKernel.Helpers
{
    public class FileIO
    {
        /// <summary>
        /// Creates all directories and subdirectories in the specified path unless they already exist
        /// </summary>
        /// <param name="path">The directory to create</param>
        public static void CreateDirectory(string path)
        {
            if (!DirectoryExists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Creates or overwrites a file in the specified path
        /// </summary>
        /// <param name="path">The path and name of the file to create</param>
        public static void CreateFile(string path)
        {
            if (FileExists(path))
                return;

            var fileInfo = new FileInfo(path);
            CreateDirectory(fileInfo.DirectoryName);

            //we use 'using' to close the file after it's created
            using (File.Create(path))
            {
            }
        }
        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk
        /// </summary>
        /// <param name="path">The path to test</param>
        /// <returns>
        /// true if path refers to an existing directory; false if the directory does not exist or an error occurs when
        /// trying to determine if the specified file exists
        /// </returns>
        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }
        /// <summary>
        /// Determines whether the specified file exists
        /// </summary>
        /// <param name="filePath">The file to check</param>
        /// <returns>
        /// True if the caller has the required permissions and path contains the name of an existing file; otherwise,
        /// false.
        /// </returns>
        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
