using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Git.Framework.MsSql.DataAccess
{
    [Serializable, DebuggerStepThrough, GeneratedCode("xsd", "2.0.50727.42"), DesignerCategory("code"), XmlType(AnonymousType=true)]
    public class dataOperationsDataCommandParametersParam
    {
        private dataOperationsDataCommandParametersParamDbType dbTypeField;
        private dataOperationsDataCommandParametersParamDirection directionField = dataOperationsDataCommandParametersParamDirection.Input;
        private string nameField;
        private int sizeField = -1;

        public DbParameter GetDbParameter()
        {
            DbType type = (DbType) Enum.Parse(typeof(DbType), this.dbType.ToString());
            SqlParameter parameter = new SqlParameter {
                ParameterName = this.name,
                DbType = type,
                Direction = (ParameterDirection) Enum.Parse(typeof(ParameterDirection), this.direction.ToString())
            };
            if (this.size != -1)
            {
                parameter.Size = this.size;
            }
            return parameter;
        }

        [XmlAttribute]
        public dataOperationsDataCommandParametersParamDbType dbType
        {
            get
            {
                return this.dbTypeField;
            }
            set
            {
                this.dbTypeField = value;
            }
        }

        [XmlAttribute, DefaultValue(0)]
        public dataOperationsDataCommandParametersParamDirection direction
        {
            get
            {
                return this.directionField;
            }
            set
            {
                this.directionField = value;
            }
        }

        [XmlAttribute]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [DefaultValue(-1), XmlAttribute]
        public int size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }
    }
}

