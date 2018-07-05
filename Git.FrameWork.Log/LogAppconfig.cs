using System;
using System.Configuration;

namespace Git.Framework.Log
{
    public static class LogAppconfig
    {
        private static bool isConsole = bool.Parse(ConfigurationManager.AppSettings["console"].ToLower().Trim());
        private static bool isFile = bool.Parse(ConfigurationManager.AppSettings["file"].ToLower().Trim());
        private static MessageType type = MessageType.success;

        public static bool IsConsole
        {
            get
            {
                return isConsole;
            }
            set
            {
                isConsole = value;
            }
        }

        public static bool IsFile
        {
            get
            {
                return isFile;
            }
            set
            {
                isFile = value;
            }
        }

        public static MessageType Type
        {
            get
            {
                if ((ConfigurationManager.AppSettings["level"] == null) || (ConfigurationManager.AppSettings["level"] == ""))
                {
                    type = MessageType.success;
                }
                else
                {
                    switch (ConfigurationManager.AppSettings["level"])
                    {
                        case "success":
                            return MessageType.success;

                        case "warning":
                            return MessageType.warning;

                        case "info":
                            return MessageType.info;

                        case "error":
                            return MessageType.error;
                    }
                }
                return type;
            }
            set
            {
                type = value;
            }
        }
    }
}

