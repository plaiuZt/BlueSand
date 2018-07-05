using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Git.Framework.MsSql.DataAccess
{
    [Serializable, XmlType(AnonymousType=true), GeneratedCode("xsd", "2.0.50727.42")]
    public enum dataOperationsDataCommandDatabase
    {
        Common,
        ControlPanel,
        ContentManagement,
        CustomerMasterProfile,
        InventoryManagement,
        InvoiceManagement,
        OrderManagement,
        POASNManagement,
        ServiceManagement,
        OZZO,
        OverseaLocalArchitecture,
        Portal,
        ServiceManagementSendSSB,
        ServiceManagementReceiveSSB,
        NEWSQL,
        D2WHP01,
        D2HIS01,
        EHISSQL,
        SSB,
        EIMSManagement
    }
}

