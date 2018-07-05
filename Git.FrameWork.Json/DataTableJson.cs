using System;
using System.Data;
using System.Text;

namespace Git.Framework.Json
{
    public class DataTableJson
    {
        private DataTable _jsonTable;

        public DataTableJson()
        {
        }

        public DataTableJson(DataTable table)
        {
            this._jsonTable = table;
        }

        public string ToJson()
        {
            StringBuilder builder = new StringBuilder();
            if ((this._jsonTable != null) && (this._jsonTable.Rows.Count > 0))
            {
                builder.Append("{ ");
                builder.Append("\"Head\":[ ");
                for (int i = 0; i < this._jsonTable.Rows.Count; i++)
                {
                    builder.Append("{ ");
                    for (int j = 0; j < this._jsonTable.Columns.Count; j++)
                    {
                        if (j < (this._jsonTable.Columns.Count - 1))
                        {
                            builder.Append("\"" + this._jsonTable.Columns[j].ColumnName.ToString() + "\":\"" + this._jsonTable.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == (this._jsonTable.Columns.Count - 1))
                        {
                            builder.Append("\"" + this._jsonTable.Columns[j].ColumnName.ToString() + "\":\"" + this._jsonTable.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == (this._jsonTable.Rows.Count - 1))
                    {
                        builder.Append("} ");
                    }
                    else
                    {
                        builder.Append("}, ");
                    }
                }
                builder.Append("]}");
                return builder.ToString();
            }
            return null;
        }

        public string ToJson(DataTable table)
        {
            this._jsonTable = table;
            return this.ToJson();
        }

        public DataTable JsonTable
        {
            get
            {
                return this._jsonTable;
            }
            set
            {
                this._jsonTable = value;
            }
        }
    }
}

