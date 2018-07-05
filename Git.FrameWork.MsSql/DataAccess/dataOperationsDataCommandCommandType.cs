using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Git.Framework.MsSql.DataAccess
{
    [Serializable, XmlType(AnonymousType=true), GeneratedCode("xsd", "2.0.50727.42")]
    public enum dataOperationsDataCommandCommandType
    {
        StoredProcedure,
        TableDirect,
        Text
    }
}

