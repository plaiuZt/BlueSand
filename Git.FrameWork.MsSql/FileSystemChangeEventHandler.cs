using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Git.Framework.MsSql
{
    public class FileSystemChangeEventHandler
    {
        private object m_SyncObject;
        private int m_Timeout;
        private Dictionary<string, Timer> m_Timers;

        public event FileSystemEventHandler ActualHandler;

        private FileSystemChangeEventHandler()
        {
            this.m_SyncObject = new object();
            this.m_Timers = new Dictionary<string, Timer>(new CaseInsensitiveStringEqualityComparer());
        }

        public FileSystemChangeEventHandler(int timeout) : this()
        {
            this.m_Timeout = timeout;
        }

        public void ChangeEventHandler(object sender, FileSystemEventArgs e)
        {
            lock (this.m_SyncObject)
            {
                Timer timer;
                if (this.m_Timers.ContainsKey(e.FullPath))
                {
                    timer = this.m_Timers[e.FullPath];
                    timer.Change(-1, -1);
                    timer.Dispose();
                }
                if (this.ActualHandler != null)
                {
                    timer = new Timer(new System.Threading.TimerCallback(this.TimerCallback), new FileChangeEventArg(sender, e), this.m_Timeout, -1);
                    this.m_Timers[e.FullPath] = timer;
                }
            }
        }

        private void TimerCallback(object state)
        {
            FileChangeEventArg arg = state as FileChangeEventArg;
            this.ActualHandler(arg.Sender, arg.Argument);
        }

        private class FileChangeEventArg
        {
            private FileSystemEventArgs m_Argument;
            private object m_Sender;

            public FileChangeEventArg(object sender, FileSystemEventArgs arg)
            {
                this.m_Sender = sender;
                this.m_Argument = arg;
            }

            public FileSystemEventArgs Argument
            {
                get
                {
                    return this.m_Argument;
                }
            }

            public object Sender
            {
                get
                {
                    return this.m_Sender;
                }
            }
        }
    }
}

