using System;
using System.Xml.Serialization;

namespace Git.Framework.MsSql.DataAccess
{
    [XmlRoot("databaseList")]
    public class DatabaseList
    {
        private DatabaseInstance[] m_DatabaseInstances;

        [XmlElement("database")]
        public DatabaseInstance[] DatabaseInstances
        {
            get
            {
                return this.m_DatabaseInstances;
            }
            set
            {
                this.m_DatabaseInstances = value;
            }
        }
    }
}

