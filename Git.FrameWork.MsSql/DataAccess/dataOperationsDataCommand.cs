using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Git.Framework.MsSql.DataAccess
{

    [Serializable, DebuggerStepThrough, DesignerCategory("code"), XmlType(AnonymousType=true), GeneratedCode("xsd", "2.0.50727.42")]
    public class dataOperationsDataCommand
    {
        private string commandTextField;
        private dataOperationsDataCommandCommandType commandTypeField = dataOperationsDataCommandCommandType.Text;
        private string databaseField;
        private bool databaseFieldSpecified;
        private string nameField;
        private dataOperationsDataCommandParameters parametersField;
        private int timeOutField = 300;

        public DataCommand GetDataCommand()
        {
            return new DataCommand(this.database.ToString(), this.GetDbCommand());
        }

        private DbCommand GetDbCommand()
        {
            DbCommand command = DbCommandFactory.CreateDbCommand();
            command.CommandText = this.commandText.Trim();
            command.CommandTimeout = this.timeOut;
            command.CommandType = (CommandType) Enum.Parse(typeof(CommandType), this.commandType.ToString());
            if (((this.parameters != null) && (this.parameters.param != null)) && (this.parameters.param.Length > 0))
            {
                foreach (dataOperationsDataCommandParametersParam param in this.parameters.param)
                {
                    command.Parameters.Add(param.GetDbParameter());
                }
            }
            return command;
        }

        public string commandText
        {
            get
            {
                return this.commandTextField;
            }
            set
            {
                this.commandTextField = value;
            }
        }

        [XmlAttribute, DefaultValue(2)]
        public dataOperationsDataCommandCommandType commandType
        {
            get
            {
                return this.commandTypeField;
            }
            set
            {
                this.commandTypeField = value;
            }
        }

        [XmlAttribute]
        public string database
        {
            get
            {
                return this.databaseField;
            }
            set
            {
                this.databaseField = value;
            }
        }

        [XmlIgnore]
        public bool databaseSpecified
        {
            get
            {
                return this.databaseFieldSpecified;
            }
            set
            {
                this.databaseFieldSpecified = value;
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

        public dataOperationsDataCommandParameters parameters
        {
            get
            {
                return this.parametersField;
            }
            set
            {
                this.parametersField = value;
            }
        }

        [DefaultValue(300), XmlAttribute]
        public int timeOut
        {
            get
            {
                return this.timeOutField;
            }
            set
            {
                this.timeOutField = value;
            }
        }
    }
}

