namespace Git.Framework.ORM
{
    using System;
    using System.ComponentModel;

    public enum JoinType
    {
        [Description("连接子查询")]
        From = 5,
        [Description("全外连接")]
        Full = 4,
        [Description("内连接")]
        Inner = 2,
        [Description("左外连接")]
        Left = 1,
        [Description("右外连接")]
        Right = 3
    }
}

