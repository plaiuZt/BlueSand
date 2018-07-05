using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Git.Framework.MsSql.Entity
{
    internal interface IEntityDataSource : IEnumerable<string>, IEnumerable, IDisposable
    {
        bool ContainsColumn(string columnName);

        object this[string columnName] { get; }

        object this[int iIndex] { get; }
    }
}

