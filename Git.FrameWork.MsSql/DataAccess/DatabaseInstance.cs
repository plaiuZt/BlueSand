using System;
using System.Xml.Serialization;

namespace Git.Framework.MsSql.DataAccess
{
    [XmlRoot("database")]
    public class DatabaseInstance
    {
        private string m_ConnectionString;
        private string m_Name;

        [XmlElement("connectionString")]
        public string ConnectionString
        {
            get
            {
                return this.m_ConnectionString;
            }
            set
            {
                this.m_ConnectionString = value;
            }
        }

        [XmlAttribute("name")]
        public string Name
        {
            get
            {
                return this.m_Name;
            }
            set
            {
                this.m_Name = value;
            }
        }
    }
}

