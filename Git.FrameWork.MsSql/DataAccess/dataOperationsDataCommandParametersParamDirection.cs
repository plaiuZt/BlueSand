using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Git.Framework.MsSql.DataAccess
{
    [Serializable, GeneratedCode("xsd", "2.0.50727.42"), XmlType(AnonymousType=true)]
    public enum dataOperationsDataCommandParametersParamDirection
    {
        Input,
        InputOutput,
        Output,
        ReturnValue
    }
}

