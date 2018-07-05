namespace Git.Framework.ORM
{
    using System;
    using System.ComponentModel;

    public enum JointType
    {
        [Description("And")]
        And = 2,
        [Description("AndBegin")]
        AndBegin = 6,
        [Description("Begin")]
        Begin = 4,
        [Description("End")]
        End = 8,
        [Description("Or")]
        Or = 3,
        [Description("OrBegin")]
        OrBegin = 7,
        [Description("Where")]
        Where = 1,
        [Description("WhereBegin")]
        WhereBegin = 5
    }
}

