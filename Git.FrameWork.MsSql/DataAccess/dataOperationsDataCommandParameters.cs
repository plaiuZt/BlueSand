using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Git.Framework.MsSql.DataAccess
{
    [Serializable, DesignerCategory("code"), DebuggerStepThrough, GeneratedCode("xsd", "2.0.50727.42"), XmlType(AnonymousType=true)]
    public class dataOperationsDataCommandParameters
    {
        private dataOperationsDataCommandParametersParam[] paramField;

        [XmlElement("param")]
        public dataOperationsDataCommandParametersParam[] param
        {
            get
            {
                return this.paramField;
            }
            set
            {
                this.paramField = value;
            }
        }
    }
}

