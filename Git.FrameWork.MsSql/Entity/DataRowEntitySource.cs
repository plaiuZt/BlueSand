using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Git.Framework.MsSql.Entity
{
    internal class DataRowEntitySource : IEntityDataSource, IEnumerable<string>, IEnumerable, IDisposable
    {
        private DataRow m_DataRow;

        public DataRowEntitySource(DataRow dr)
        {
            this.m_DataRow = dr;
        }

        public bool ContainsColumn(string columnName)
        {
            return this.m_DataRow.Table.Columns.Contains(columnName);
        }

        public void Dispose()
        {
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new RowColumnNameEnumerator(this.m_DataRow);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public object this[string columnName]
        {
            get
            {
                return this.m_DataRow[columnName];
            }
        }

        public object this[int index]
        {
            get
            {
                return this.m_DataRow[index];
            }
        }

        private class RowColumnNameEnumerator : IEnumerator<string>, IDisposable, IEnumerator
        {
            private IEnumerator m_InternalEnumeator;

            public RowColumnNameEnumerator(DataRow dr)
            {
                this.m_InternalEnumeator = dr.Table.Columns.GetEnumerator();
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return this.m_InternalEnumeator.MoveNext();
            }

            public void Reset()
            {
                this.m_InternalEnumeator.Reset();
            }

            public string Current
            {
                get
                {
                    DataColumn current = this.m_InternalEnumeator.Current as DataColumn;
                    return current.ColumnName;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }
        }
    }
}

