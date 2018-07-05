using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Git.Framework.MsSql.DataAccess
{

    [Serializable, XmlType(AnonymousType=true), XmlRoot(IsNullable=false), DebuggerStepThrough, GeneratedCode("xsd", "2.0.50727.42"), DesignerCategory("code")]
    public class dataOperations
    {
        private dataOperationsDataCommand[] dataCommandField;

        public IList<string> GetCommandNames()
        {
            if ((this.dataCommandField == null) || (this.dataCommandField.Length == 0))
            {
                return new string[0];
            }
            List<string> list = new List<string>(this.dataCommandField.Length);
            for (int i = 0; i < this.dataCommandField.Length; i++)
            {
                list.Add(this.dataCommandField[i].name);
            }
            return list;
        }

        [XmlElement("dataCommand")]
        public dataOperationsDataCommand[] dataCommand
        {
            get
            {
                return this.dataCommandField;
            }
            set
            {
                this.dataCommandField = value;
            }
        }
    }
}

