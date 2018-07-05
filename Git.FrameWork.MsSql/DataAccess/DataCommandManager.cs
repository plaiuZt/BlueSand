using Git.Framework.Log;
using Git.Framework.MsSql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;

namespace Git.Framework.MsSql.DataAccess
{
    public static class DataCommandManager
    {
        private const string EventCategory = "DataCommandManager";
        private const int FILE_CHANGE_NOTIFICATION_INTERVAL = 500;
        private static Git.Framework.Log.Log log = Git.Framework.Log.Log.Instance(typeof(DataCommandManager));
        private static object s_CommandFileListSyncObject;
        private static object s_CommandSyncObject;
        private static Dictionary<string, DataCommand> s_DataCommands;
        private static string s_DataFileFolder;
        private static FileSystemChangeEventHandler s_FileChangeHandler = new FileSystemChangeEventHandler(500);
        private static Dictionary<string, IList<string>> s_FileCommands;
        private static FileSystemWatcher s_Watcher;

        static DataCommandManager()
        {
            s_FileChangeHandler.ActualHandler += new FileSystemEventHandler(DataCommandManager.Watcher_Changed);
            s_DataFileFolder = Path.GetDirectoryName(DataAccessSetting.DataCommandFileListConfigFile);
            s_CommandSyncObject = new object();
            s_CommandFileListSyncObject = new object();
            s_Watcher = new FileSystemWatcher(s_DataFileFolder);
            s_Watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite;
            s_Watcher.Changed += new FileSystemEventHandler(s_FileChangeHandler.ChangeEventHandler);
            s_Watcher.EnableRaisingEvents = true;
            UpdateAllCommandFiles();
        }

        public static CustomDataCommand CreateCustomDataCommand(GitDatabase database)
        {
            return new CustomDataCommand(database);
        }

        public static CustomDataCommand CreateCustomDataCommand(GitDatabase database, CommandType commandType)
        {
            return new CustomDataCommand(database, commandType);
        }

        public static CustomDataCommand CreateCustomDataCommand(GitDatabase database, CommandType commandType, string commandText)
        {
            return new CustomDataCommand(database, commandType, commandText);
        }

        public static CustomDataCommand CreateCustomDataCommand(string database, CommandType commandType, string commandText)
        {
            return new CustomDataCommand(database, commandType, commandText);
        }

        public static DataCommand GetDataCommand(string name)
        {
            return (s_DataCommands[name].Clone() as DataCommand);
        }

        private static void UpdateAllCommandFiles()
        {
            object obj2;
            Monitor.Enter(obj2 = s_CommandFileListSyncObject);
            try
            {
                log.Info("FilePath:" + DataAccessSetting.DataCommandFileListConfigFile);
                ConfigDataCommandFileList list = ObjectXmlSerializer.LoadFromXml<ConfigDataCommandFileList>(DataAccessSetting.DataCommandFileListConfigFile);
                log.Info("config files length:" + list.FileList.Length);
                if (((list == null) || (list.FileList == null)) || (list.FileList.Length == 0))
                {
                    throw new DataCommandFileNotSpecifiedException();
                }
                s_FileCommands = new Dictionary<string, IList<string>>();
                s_DataCommands = new Dictionary<string, DataCommand>();
                foreach (ConfigDataCommandFileList.DataCommandFile file in list.FileList)
                {
                    string fileName = Path.Combine(s_DataFileFolder, file.FileName);
                    log.Info("fileName:" + fileName);
                    UpdateCommandFile(fileName);
                }
            }
            catch (Exception exception)
            {
                log.Info("加载配置文件异常:" + exception.Message);
                throw exception;
            }
            finally
            {
                Monitor.Exit(obj2);
            }
        }

        private static void UpdateCommandFile(string fileName)
        {
            IList<string> list;
            if (s_FileCommands.ContainsKey(fileName))
            {
                list = s_FileCommands[fileName];
            }
            else
            {
                list = null;
            }
            lock (s_CommandSyncObject)
            {
                Dictionary<string, DataCommand> dictionary = new Dictionary<string, DataCommand>(s_DataCommands);
                if (list != null)
                {
                    foreach (string str in list)
                    {
                        dictionary.Remove(str);
                    }
                }
                dataOperations operations = ObjectXmlSerializer.LoadFromXml<dataOperations>(fileName);
                if (operations == null)
                {
                    throw new DataCommandFileLoadException(fileName);
                }
                if ((operations.dataCommand != null) && (operations.dataCommand.Length > 0))
                {
                    foreach (dataOperationsDataCommand command in operations.dataCommand)
                    {
                        try
                        {
                            dictionary.Add(command.name, command.GetDataCommand());
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("Command:" + command.name + " has exists.", exception);
                        }
                    }
                    s_DataCommands = dictionary;
                }
                s_FileCommands[fileName] = operations.GetCommandNames();
            }
        }

        private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (string.Compare(e.FullPath, DataAccessSetting.DataCommandFileListConfigFile, true) == 0)
            {
                UpdateAllCommandFiles();
            }
            else
            {
                lock (s_CommandFileListSyncObject)
                {
                    foreach (string str in s_FileCommands.Keys)
                    {
                        if (string.Compare(str, e.FullPath, true) == 0)
                        {
                            UpdateCommandFile(str);
                            return;
                        }
                    }
                }
            }
        }

        private static string DataCommandListFileName
        {
            get
            {
                return Path.GetFileName(DataAccessSetting.DataCommandFileListConfigFile);
            }
        }
    }
}

