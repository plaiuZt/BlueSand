using Git.Framework.Io;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;

namespace Git.Framework.Log
{
    public class Log : IDisposable
    {
        private static Type classType;
        private static Queue<LogMessage> logMessages;
        private static LogType logtype;
        private static string path = ConfigurationManager.AppSettings["logpath"];
        private static bool state;
        private static DateTime time;
        private static StreamWriter writer;

        public Log()
        {
            if (string.IsNullOrEmpty(path))
            {
                new Log(@".\", LogType.Daily, null);
            }
            else
            {
                new Log(path, LogType.Daily, null);
            }
        }

        public Log(LogType t)
        {
            if (string.IsNullOrEmpty(path))
            {
                new Log(@".\", t, null);
            }
            else
            {
                new Log(path, t, null);
            }
        }

        public Log(LogType t, Type type)
        {
            if (string.IsNullOrEmpty(path))
            {
                new Log(@".\", t, type);
            }
            else
            {
                new Log(path, t, type);
            }
        }

        private Log(string filepath, LogType t, Type type)
        {
            if (logMessages == null)
            {
                state = true;
                path = filepath;
                logtype = t;
                classType = type;
                this.FileOpen();
                logMessages = new Queue<LogMessage>();
                new Thread(new ThreadStart(this.Work)).Start();
            }
        }

        public void Dispose()
        {
            state = false;
            GC.SuppressFinalize(this);
        }

        public void Error(string content)
        {
            this.Error(DateTime.Now, content, MessageType.error);
        }

        public void Error(DateTime time, string content)
        {
            this.Error(time, content, MessageType.error);
        }

        public void Error(DateTime time, string content, MessageType type)
        {
            if (type >= LogAppconfig.Type)
            {
                LogMessage message = new LogMessage(time, content, type);
                this.Write(message);
            }
        }

        private void FileClose()
        {
            if (writer != null)
            {
                writer.Flush();
                writer.Close();
                writer.Dispose();
                writer = null;
            }
        }

        private void FileOpen()
        {
            lock (this)
            {
                if (!FileManager.DirectoryExists(path))
                {
                    FileManager.CreateDirectory(path);
                }
                writer = new StreamWriter(path + this.GetFileName(), true, Encoding.Default);
            }
        }

        private string GetFileName()
        {
            DateTime now = DateTime.Now;
            string format = "";
            switch (logtype)
            {
                case LogType.Daily:
                    time = new DateTime(now.Year, now.Month, now.Day);
                    time = time.AddDays(1.0);
                    format = "yyyyMMdd'.log'";
                    break;

                case LogType.Weekly:
                    time = new DateTime(now.Year, now.Month, now.Day);
                    time = time.AddDays(7.0);
                    format = "yyyyMMdd'.log'";
                    break;

                case LogType.Monthly:
                    time = new DateTime(now.Year, now.Month, 1);
                    time = time.AddMonths(1);
                    format = "yyyyMM'.log'";
                    break;

                case LogType.Annually:
                    time = new DateTime(now.Year, 1, 1);
                    time = time.AddYears(1);
                    format = "yyyy'.log'";
                    break;
            }
            return now.ToString(format);
        }

        public void Info(string content)
        {
            this.Info(DateTime.Now, content, MessageType.info);
        }

        public void Info(DateTime time, string content)
        {
            this.Info(time, content, MessageType.info);
        }

        public void Info(DateTime time, string content, MessageType type)
        {
            if (type >= LogAppconfig.Type)
            {
                LogMessage message = new LogMessage(time, content, type);
                this.Write(message);
            }
        }

        public static Log Instance()
        {
            return Instance(null);
        }

        public static Log Instance(Type classType)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = @".\";
            }
            else if (!Directory.Exists(path))
            {
                path = FileManager.GetRootPath() + path;
            }
            string str = ConfigurationManager.AppSettings["logtype"];
            if (string.IsNullOrEmpty(str))
            {
                logtype = LogType.Daily;
            }
            else
            {
                string str2 = str;
                if (str2 != null)
                {
                    if (!(str2 == "Daily"))
                    {
                        if (str2 == "Weekly")
                        {
                            logtype = LogType.Weekly;
                        }
                        else if (str2 == "Monthly")
                        {
                            logtype = LogType.Monthly;
                        }
                        else if (str2 == "Annually")
                        {
                            logtype = LogType.Annually;
                        }
                    }
                    else
                    {
                        logtype = LogType.Daily;
                    }
                }
            }
            return new Log(path, logtype, classType);
        }

        public void Success(string content)
        {
            this.Success(DateTime.Now, content, MessageType.success);
        }

        public void Success(DateTime time, string content)
        {
            this.Success(time, content, MessageType.success);
        }

        private void Success(DateTime time, string content, MessageType type)
        {
            if (type >= LogAppconfig.Type)
            {
                LogMessage message = new LogMessage(time, content, type);
                this.Write(message);
            }
        }

        public void Warning(string content)
        {
            this.Warning(DateTime.Now, content, MessageType.warning);
        }

        public void Warning(DateTime time, string content)
        {
            this.Warning(time, content, MessageType.warning);
        }

        private void Warning(DateTime time, string content, MessageType type)
        {
            if (type >= LogAppconfig.Type)
            {
                LogMessage message = new LogMessage(time, content, type);
                this.Write(message);
            }
        }

        private void Work()
        {
            while (true)
            {
                if (logMessages.Count > 0)
                {
                    LogMessage message = null;
                    lock (logMessages)
                    {
                        message = logMessages.Dequeue();
                    }
                    if (message != null)
                    {
                        this.WriteLogMessage(message);
                    }
                }
                else if (state)
                {
                    Thread.Sleep(1);
                }
                else
                {
                    this.FileClose();
                }
            }
        }

        private void Write(LogMessage message)
        {
            if (logMessages != null)
            {
                lock (logMessages)
                {
                    logMessages.Enqueue(message);
                }
            }
        }

        private void Write(Exception e, MessageType type)
        {
            this.Write(new LogMessage(e.Message, type));
        }

        private void Write(string text, MessageType type)
        {
            this.Write(new LogMessage(text, type));
        }

        public void Write(DateTime now, string text, MessageType type)
        {
            this.Write(new LogMessage(now, text, type));
        }

        private void WriteLogMessage(LogMessage message)
        {
            try
            {
                if (LogAppconfig.IsFile)
                {
                    if (writer == null)
                    {
                        this.FileOpen();
                    }
                    else
                    {
                        if (DateTime.Now >= time)
                        {
                            this.FileClose();
                            this.FileOpen();
                        }
                        lock (writer)
                        {
                            writer.Write("日志级别:[" + message.Type + "]\r\n");
                            if (classType != null)
                            {
                                writer.Write("日志位置:" + classType.FullName + "\r\n");
                            }
                            writer.Write("日志时间:" + message.Time + "\r\n");
                            writer.Write("日志内容:" + message.Content + "\r\n\r\n");
                            writer.Flush();
                        }
                    }
                }
                if (LogAppconfig.IsConsole)
                {
                    if (classType != null)
                    {
                        Console.WriteLine(string.Concat(new object[] { "日志级别:[", message.Type, "]\r\n日志位置:", classType.FullName, "\r\n日志时间:", message.Time, "\r\n日志内容:", message.Content, "\r\n\r\n" }));
                    }
                    else
                    {
                        Console.WriteLine(string.Concat(new object[] { "日志级别:[", message.Type, "]\r\n日志时间:", message.Time, "\r\n日志内容:", message.Content, "\r\n\r\n" }));
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}

