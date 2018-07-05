using System;

namespace Git.Framework.MsSql.DataAccess
{
    internal static class DataAccessSetting
    {
        private static string s_DatabaseConfigFile = ConfigurationHelper.GetConfigurationFile("DatabaseListFile");
        private static string s_DataCommandFileListConfigFile = ConfigurationHelper.GetConfigurationFile("DataCommandFile");

        public static string DatabaseConfigFile
        {
            get
            {
                return s_DatabaseConfigFile;
            }
        }

        public static string DataCommandFileListConfigFile
        {
            get
            {
                return s_DataCommandFileListConfigFile;
            }
        }
    }
}

