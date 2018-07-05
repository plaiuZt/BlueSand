using System;
using System.Xml.Serialization;

namespace Git.Framework.MsSql.DataAccess
{


    [XmlRoot("dataCommandFiles")]
    public class ConfigDataCommandFileList
    {
        private DataCommandFile[] m_commandFiles;

        [XmlElement("file")]
        public DataCommandFile[] FileList
        {
            get
            {
                return this.m_commandFiles;
            }
            set
            {
                this.m_commandFiles = value;
            }
        }

        public class DataCommandFile
        {
            private string m_FileName;

            [XmlAttribute("name")]
            public string FileName
            {
                get
                {
                    return this.m_FileName;
                }
                set
                {
                    this.m_FileName = value;
                }
            }
        }
    }
}

