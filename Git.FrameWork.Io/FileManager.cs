using Git.Framework.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Git.Framework.Io
{
    public class FileManager
    {
        private static string strRootFolder = AppDomain.CurrentDomain.BaseDirectory;

        static FileManager()
        {
            strRootFolder = strRootFolder.Substring(0, strRootFolder.LastIndexOf(@"\"));
        }

        public static bool CopyFolder(string source, string destination)
        {
            try
            {
                if (destination[destination.Length - 1] != Path.DirectorySeparatorChar)
                {
                    destination = destination + Path.DirectorySeparatorChar;
                }
                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);
                }
                string[] fileSystemEntries = Directory.GetFileSystemEntries(source);
                foreach (string str in fileSystemEntries)
                {
                    if (Directory.Exists(str))
                    {
                        CopyFolder(str, destination + Path.GetFileName(str));
                    }
                    else
                    {
                        File.Copy(str, destination + Path.GetFileName(str), true);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static bool CreateFile(string filename, string path)
        {
            try
            {
                File.Create(path + @"\" + filename).Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CreateFile(string filename, string path, byte[] contents)
        {
            try
            {
                FileStream stream = File.Create(path + @"\" + filename);
                stream.Write(contents, 0, contents.Length);
                stream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void CreateFolder(string name, string parentName)
        {
            new DirectoryInfo(parentName).CreateSubdirectory(name);
        }

        public static bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DeleteFolder(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    foreach (string str in Directory.GetFileSystemEntries(path))
                    {
                        if (File.Exists(str))
                        {
                            File.Delete(str);
                        }
                        else
                        {
                            DeleteFolder(str);
                        }
                    }
                    Directory.Delete(path);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static List<FileItem> GetDirectoryItems()
        {
            return GetDirectoryItems(strRootFolder);
        }

        public static List<FileItem> GetDirectoryItems(string path)
        {
            List<FileItem> list = new List<FileItem>();
            string[] directories = Directory.GetDirectories(path);
            foreach (string str in directories)
            {
                FileItem item = new FileItem();
                DirectoryInfo info = new DirectoryInfo(str);
                item.Name = info.Name;
                item.FullName = info.FullName;
                item.CreationDate = info.CreationTime;
                item.IsFolder = false;
                list.Add(item);
            }
            return list;
        }

        public static string GetDomainRoot()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static string GetFileDirectory(string fullPath)
        {
            return fullPath.SubStr(0, (fullPath.LastIndex(@"\") + 1));
        }

        public static List<FileItem> GetFileItems()
        {
            return GetFileItems(strRootFolder);
        }

        public static List<FileItem> GetFileItems(string path)
        {
            List<FileItem> list = new List<FileItem>();
            string[] files = Directory.GetFiles(path);
            foreach (string str in files)
            {
                FileItem item = new FileItem();
                FileInfo info = new FileInfo(str);
                item.Name = info.Name;
                item.FullName = info.FullName;
                item.CreationDate = info.CreationTime;
                item.IsFolder = true;
                item.Size = info.Length;
                list.Add(item);
            }
            return list;
        }

        public static FileItem GetItemInfo(string path)
        {
            FileItem item = new FileItem();
            if (Directory.Exists(path))
            {
                DirectoryInfo info = new DirectoryInfo(path);
                item.Name = info.Name;
                item.FullName = info.FullName;
                item.CreationDate = info.CreationTime;
                item.IsFolder = true;
                item.LastAccessDate = info.LastAccessTime;
                item.LastWriteDate = info.LastWriteTime;
                item.FileCount = info.GetFiles().Length;
                item.SubFolderCount = info.GetDirectories().Length;
                return item;
            }
            FileInfo info2 = new FileInfo(path);
            item.Name = info2.Name;
            item.FullName = info2.FullName;
            item.CreationDate = info2.CreationTime;
            item.LastAccessDate = info2.LastAccessTime;
            item.LastWriteDate = info2.LastWriteTime;
            item.IsFolder = false;
            item.Size = info2.Length;
            return item;
        }

        public static string GetRootPath()
        {
            return strRootFolder;
        }

        public static bool IsCanEdit(string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] strArray = new string[] { ".htm", ".html", ".txt", ".js", ".css", ".xml", ".sitemap" };
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strExtension.Equals(strArray[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsSafeName(string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] strArray = new string[] { ".htm", ".html", ".txt", ".js", ".css", ".xml", ".sitemap", ".jpg", ".gif", ".png", ".rar", ".zip" };
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strExtension.Equals(strArray[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsUnsafeName(string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] strArray = new string[] { 
                ".", ".asp", ".aspx", ".cs", ".net", ".dll", ".config", ".ascx", ".master", ".asmx", ".asax", ".cd", ".browser", ".rpt", ".ashx", ".xsd", 
                ".mdf", ".resx", ".xsd"
             };
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strExtension.Equals(strArray[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool MoveFile(string oldPath, string newPath)
        {
            try
            {
                File.Move(oldPath, newPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool MoveFolder(string oldPath, string newPath)
        {
            try
            {
                Directory.Move(oldPath, newPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string OpenText(string parentName)
        {
            string str;
            StreamReader reader = File.OpenText(parentName);
            StringBuilder builder = new StringBuilder();
            while ((str = reader.ReadLine()) != null)
            {
                builder.Append(str);
            }
            reader.Close();
            return builder.ToString();
        }

        public static void SetRootPath(string path)
        {
            strRootFolder = path;
        }

        public static bool WriteAllText(string parentName, string contents)
        {
            try
            {
                File.WriteAllText(parentName, contents, Encoding.Unicode);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

