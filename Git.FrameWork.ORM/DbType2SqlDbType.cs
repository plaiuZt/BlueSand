namespace Git.Framework.ORM
{
    using System;
    using System.Data;

    public class DbType2SqlDbType
    {
        public static SqlDbType ToType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString:
                    return SqlDbType.VarChar;

                case DbType.Binary:
                    return SqlDbType.Binary;

                case DbType.Byte:
                    return SqlDbType.Bit;

                case DbType.Boolean:
                    return SqlDbType.Bit;

                case DbType.Currency:
                    return SqlDbType.Money;

                case DbType.Date:
                    return SqlDbType.Date;

                case DbType.DateTime:
                    return SqlDbType.DateTime;

                case DbType.Decimal:
                    return SqlDbType.Money;

                case DbType.Double:
                    return SqlDbType.Float;

                case DbType.Guid:
                    return SqlDbType.UniqueIdentifier;

                case DbType.Int16:
                    return SqlDbType.SmallInt;

                case DbType.Int32:
                    return SqlDbType.Int;

                case DbType.Int64:
                    return SqlDbType.BigInt;

                case DbType.Single:
                    return SqlDbType.Float;

                case DbType.String:
                    return SqlDbType.VarChar;

                case DbType.Time:
                    return SqlDbType.Time;

                case DbType.UInt16:
                    return SqlDbType.SmallInt;

                case DbType.UInt32:
                    return SqlDbType.Int;

                case DbType.UInt64:
                    return SqlDbType.BigInt;
            }
            return SqlDbType.VarChar;
        }
    }
}

