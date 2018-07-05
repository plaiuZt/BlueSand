using Git.Framework.Io;
using System;
using System.IO;
using System.Threading;
using System.Xml.Linq;

namespace Git.Framework.ORM
{
    public static class ListenDbName
    {
        private static Timer m_timer;
        private static int TimeoutMillis = 0x7d0;

        public static void ListenDbConfig(string filePath)
        {
            TimerCallback callback = null;
            string fileDirectory = FileManager.GetRootPath() + filePath;
            if (FileManager.FileExists(fileDirectory))
            {
                fileDirectory = FileManager.GetFileDirectory(fileDirectory);
            }
            FileSystemWatcher watcher = new FileSystemWatcher {
                Path = fileDirectory,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "*.config"
            };
            watcher.Changed += (source, e) => m_timer.Change(TimeoutMillis, -1);
            watcher.EnableRaisingEvents = true;
            if (m_timer == null)
            {
                if (callback == null)
                {
                    callback = delegate (object item) {
                        Type[] allType = EntityTypeCache.GetAllType();
                        if (allType != null)
                        {
                            string[] strArray = XElement.Load(filePath).Element("database").Element("connectionString").Value.Split(new char[] { ';' });
                            string str2 = string.Empty;
                            foreach (string str3 in strArray)
                            {
                                string[] strArray2 = str3.Split(new char[] { '=' });
                                if (strArray2[0] == "database")
                                {
                                    str2 = strArray2[1];
                                }
                            }
                            TableInfo tableInfo = null;
                            foreach (Type type in allType)
                            {
                                tableInfo = EntityTypeCache.Get(type);
                                if (tableInfo != null)
                                {
                                    tableInfo.Table.DbName = str2;
                                    EntityTypeCache.InsertTableInfo(type, tableInfo);
                                }
                            }
                        }
                    };
                }
                m_timer = new Timer(callback, null, -1, -1);
            }
        }
    }
}

