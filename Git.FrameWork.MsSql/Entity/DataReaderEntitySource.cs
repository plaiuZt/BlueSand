using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Git.Framework.MsSql.Entity
{
    internal class DataReaderEntitySource : IEntityDataSource, IEnumerable<string>, IEnumerable, IDisposable
    {
        private IDataReader m_DataReader;

        public DataReaderEntitySource(IDataReader dr)
        {
            this.m_DataReader = dr;
        }

        public bool ContainsColumn(string columnName)
        {
            DataTable schemaTable = this.m_DataReader.GetSchemaTable();
            foreach (DataRow row in schemaTable.Rows)
            {
                if (string.Compare(row["ColumnName"].ToString().Trim(), columnName.Trim(), true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void Dispose()
        {
            if (this.m_DataReader != null)
            {
                this.m_DataReader.Dispose();
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new ReaderColumnNameEnumerator(this.m_DataReader);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ReaderColumnNameEnumerator(this.m_DataReader);
        }

        public object this[string columnName]
        {
            get
            {
                return this.m_DataReader[columnName];
            }
        }

        public object this[int index]
        {
            get
            {
                return this.m_DataReader[index];
            }
        }

        private class ReaderColumnNameEnumerator : IEnumerator<string>, IDisposable, IEnumerator
        {
            private IEnumerator m_InternalEnumerator;

            public ReaderColumnNameEnumerator(IDataReader dr)
            {
                DataTable schemaTable = dr.GetSchemaTable();
                this.m_InternalEnumerator = schemaTable.Rows.GetEnumerator();
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return this.m_InternalEnumerator.MoveNext();
            }

            public void Reset()
            {
                this.m_InternalEnumerator.Reset();
            }

            public string Current
            {
                get
                {
                    DataRow current = this.m_InternalEnumerator.Current as DataRow;
                    return (current["ColumnName"] as string);
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

