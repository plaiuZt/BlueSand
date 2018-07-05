using Git.Framework.Io;
using System;
using System.IO;
using System.Threading;

namespace Git.Framework.Resource
{

    public class ListenCache
    {
        private Timer m_timer;
        private int TimeoutMillis = 0x7d0;

        public void ListenCacheFile(LoadResourceDelegate argLoadResource, string filePath)
        {
            string fileDirectory = filePath;
            if (FileManager.FileExists(filePath))
            {
                fileDirectory = FileManager.GetFileDirectory(filePath);
            }
            FileSystemWatcher watcher = new FileSystemWatcher {
                Path = fileDirectory,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "*.config"
            };
            watcher.Changed += (source, e) => this.m_timer.Change(this.TimeoutMillis, -1);
            watcher.EnableRaisingEvents = true;
            if (this.m_timer == null)
            {
                this.m_timer = new Timer(new TimerCallback(argLoadResource.Invoke), null, -1, -1);
            }
        }
    }
}

