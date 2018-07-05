using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Git.Framework.MsSql.DataAccess
{
    [Serializable, XmlType(AnonymousType=true), GeneratedCode("xsd", "2.0.50727.42")]
    public enum dataOperationsDataCommandParametersParamDbType
    {
        AnsiString,
        Binary,
        Boolean,
        Byte,
        Currency,
        Date,
        DateTime,
        Decimal,
        Double,
        Int16,
        Int32,
        Int64,
        SByte,
        Single,
        String,
        StringFixedLength,
        AnsiStringFixedLength,
        Time,
        UInt16,
        UInt32,
        UInt64,
        VarNumeric,
        Xml
    }
}

