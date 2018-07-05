using System;
using System.Configuration;
using System.IO;

namespace Git.Framework.MsSql
{

    public class ConfigurationHelper
    {
        public static string GetConfigurationFile(string appSection)
        {
            string str = appSection;
            str = ConfigurationManager.AppSettings[appSection];
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, str.Replace('/', '\\').TrimStart(new char[] { '\\' }));
        }
    }
}

