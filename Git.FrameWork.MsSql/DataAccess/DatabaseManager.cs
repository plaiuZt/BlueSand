using Git.Framework.Log;
using Git.Framework.MsSql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;

namespace Git.Framework.MsSql.DataAccess
{
    internal static class DatabaseManager
    {
        private static Git.Framework.Log.Log log = Git.Framework.Log.Log.Instance(typeof(DatabaseManager));
        private static Dictionary<string, Database> s_DatabaseHashtable = new Dictionary<string, Database>();
        private static FileSystemChangeEventHandler s_FileChangeHandler = new FileSystemChangeEventHandler(500);
        private static FileSystemWatcher s_Watcher;

        [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
        static DatabaseManager()
        {
            s_FileChangeHandler.ActualHandler += new FileSystemEventHandler(DatabaseManager.OnFileChanged);
            string directoryName = Path.GetDirectoryName(DataAccessSetting.DatabaseConfigFile);
            string fileName = Path.GetFileName(DataAccessSetting.DatabaseConfigFile);
            log.Info("databaseFolder:" + directoryName);
            log.Info("databaseFile:" + fileName);
            s_Watcher = new FileSystemWatcher(directoryName);
            s_Watcher.Filter = fileName;
            s_Watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite;
            s_Watcher.Changed += new FileSystemEventHandler(s_FileChangeHandler.ChangeEventHandler);
            s_Watcher.EnableRaisingEvents = true;
            s_DatabaseHashtable = LoadDatabaseList();
        }

        public static Database GetDatabase(string name)
        {
            return s_DatabaseHashtable[name.ToUpper()];
        }

        private static Dictionary<string, Database> LoadDatabaseList()
        {
            DatabaseList list = ObjectXmlSerializer.LoadFromXml<DatabaseList>(DataAccessSetting.DatabaseConfigFile);
            if (((list == null) || (list.DatabaseInstances == null)) || (list.DatabaseInstances.Length == 0))
            {
                throw new DatabaseNotSpecifiedException();
            }
            Dictionary<string, Database> dictionary = new Dictionary<string, Database>(list.DatabaseInstances.Length);
            foreach (DatabaseInstance instance in list.DatabaseInstances)
            {
                log.Info("instance.ConnectionString:" + instance.ConnectionString);
                SqlDatabase database = new SqlDatabase(instance.ConnectionString);
                dictionary.Add(instance.Name.ToUpper(), database);
            }
            return dictionary;
        }

        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            s_DatabaseHashtable = LoadDatabaseList();
        }

        public static string TripleDESDecrypting(string Source)
        {
            string str;
            try
            {
                byte[] buffer = Convert.FromBase64String(Source);
                byte[] buffer2 = new byte[] { 
                    0x2a, 0x10, 0x5d, 0x9c, 0x4e, 4, 0xda, 0x20, 15, 0xa7, 0x2c, 80, 0x1a, 20, 0x9b, 0x70, 
                    2, 0x5e, 11, 0xcc, 0x77, 0x23, 0xb8, 0xc5
                 };
                byte[] buffer3 = new byte[] { 0x37, 0x67, 0xf6, 0x4f, 0x24, 0x63, 0xa7, 3 };
                ICryptoTransform transform = new TripleDESCryptoServiceProvider { IV = buffer3, Key = buffer2 }.CreateDecryptor();
                MemoryStream stream = new MemoryStream(buffer, 0, buffer.Length);
                CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
                str = new StreamReader(stream2, Encoding.Default).ReadToEnd();
            }
            catch (Exception exception)
            {
                throw new Exception("解密时候出现错误!错误提示:\n" + exception.Message);
            }
            return str;
        }
    }
}

