using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MCGalaxy
{
    /// <summary>
    /// File extensions.
    /// 
    /// Not an extension method class because file is static.
    /// </summary>
    public static partial class FileExts
    {
        /// <summary>
        /// Recursively deletes all files and subdirectories of the directory <paramref name="RDirectoryName"/>, and then optionally deleted <paramref name="RDirectoryName"/> itself.
        /// </summary>
        /// <param name="RDirectoryName">The directory to remove all files and subdirectories of.</param>
        /// <param name="DeleteRootDirectory">Determines if the root directory itself will additionally be deleted. Default is <c>false</c>.</param>
        public static void DeleteAllFilesAndSubdirectories(string RDirectoryName, bool DeleteRootDirectory = false)
        {
            foreach (string FileName in Directory.GetFiles(RDirectoryName))
            {
                foreach (string SDirectoryName in Directory.GetDirectories(RDirectoryName))
                {
                    DeleteAllFilesAndSubdirectories_RecursiveDelete(SDirectoryName);
                }

                Logger.Log(LogType.SystemActivity, $"Deleting file {FileName}...");
                File.Delete(FileName);
            }

            if (DeleteRootDirectory) 
            {
                Logger.Log(LogType.SystemActivity, $"Removing directory {RDirectoryName}...");
                Directory.Delete(RDirectoryName);
            }
        }

        private static void DeleteAllFilesAndSubdirectories_RecursiveDelete(string RDirectoryName)
        {
            if (!Directory.Exists(RDirectoryName)) return; 

            foreach (string SubFileName in Directory.GetFiles(RDirectoryName))
            {
                Logger.Log(LogType.SystemActivity, $"Deleting file {SubFileName}...");
                File.Delete(SubFileName);
            }

            string[] Subdirs = Directory.GetDirectories(RDirectoryName);

            if (Subdirs.Length > 0)
            {
                foreach (string Subdir in Subdirs)
                {
                    DeleteAllFilesAndSubdirectories_RecursiveDelete(Subdir);
                }
            }

            Logger.Log(LogType.SystemActivity, $"Removing directory {RDirectoryName}...");
            Directory.Delete(RDirectoryName);
        }
    }
}
