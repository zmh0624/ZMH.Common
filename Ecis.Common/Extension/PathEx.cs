using System;
using System.IO;

namespace Ecis.Common.Extension
{
    /// <summary>
    /// Path Extension
    /// Author: Minghua
    /// </summary>
    public static class PathEx
    {
        /// <summary>
        /// 递归删除文件夹及子文件
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteFolder(this string dir)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                    {
                        File.Delete(d); //直接删除其中的文件
                    }
                    else
                    {
                        DeleteFolder(d); //递归删除子文件夹
                    }
                }
                Directory.Delete(dir, true); //删除已空文件夹
            }
        }

        public static string AppendPath(this string path, string str)
        {
            return Path.Combine(path, str);
        }

        public static string ChangeExt(this string path, string newExt)
        {
            return Path.ChangeExtension(path, newExt);
        }

        public static string ChangeFileName(this string path, Func<string, string> nameFactory)
        {
            if (nameFactory != null)
            {
                string fileName = path.GetFileName();
                return path.ChangeFileName(nameFactory(fileName));
            }
            return path;
        }

        public static string ChangeFileName(this string path, string newName)
        {
            return path.GetDirectoryPath().AppendPath(newName);
        }

        public static string ChangeFileNameOnly(this string path, string newNameOnly)
        {
            return path.GetDirectoryPath().AppendPath((newNameOnly + path.GetFileExt()));
        }

        public static string ChangeFileNameOnly(this string path, Func<string, string> nameFactory)
        {
            if (nameFactory != null)
            {
                string fileNameWithoutExt = path.GetFileNameWithoutExt();
                return path.ChangeFileNameOnly(nameFactory(fileNameWithoutExt));
            }
            return path;
        }

        public static string GetDirectoryPath(this string path)
        {
            path = path.TrimEnd(new char[] { Path.DirectorySeparatorChar });
            return Path.GetDirectoryName(path);
        }

        public static string GetFileExt(this string path)
        {
            return Path.GetExtension(path);
        }

        public static string GetFileName(this string path)
        {
            return Path.GetFileName(path);
        }

        public static string GetFileNameWithoutExt(this string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public static string GetRootDirectory(this string path)
        {
            return Path.GetPathRoot(path);
        }

        public static string ToDirectory(this string path, string dirPath)
        {
            return dirPath.AppendPath(path.GetFileName());
        }

        public static string[] ToDirectory(this string[] paths, string dirPath, string srcDir)
        {
            string[] strArray = new string[paths.Length];
            int length = srcDir.Length;
            if (srcDir[length - 1] != Path.DirectorySeparatorChar)
            {
                length++;
            }
            for (int i = 0; i < strArray.Length; i++)
            {
                string str = paths[i];
                strArray[i] = dirPath.AppendPath(str.Substring(length));
            }
            return strArray;
        }
    }
}