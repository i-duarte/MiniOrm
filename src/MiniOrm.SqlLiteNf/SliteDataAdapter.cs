using MiniOrm.Common;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;

namespace MiniOrm.Sql
{
    public class SliteDataAdapter
        : Common.IDataAdapter
    {
        private SliteObjectFactory SliteObjectFactory { get; set; }

        public SliteDataAdapter(
            SliteObjectFactory sliteObjectFactory
        )
        {
            SliteObjectFactory = sliteObjectFactory;
        }

        private SQLiteConnection GetConnection(
        ) =>
            (SQLiteConnection)
            SliteObjectFactory.CreateConnection();

        #region Execute

        public int Execute(
            string sql
            , ListParameter parameters 
            , DbTransaction tran 
        ) =>
            GetCommand(sql, parameters, tran)
            .ExecuteNonQuery();

        public int Execute(
           string sql
           , ListParameter parameters 
           , DbConnection cnn 
        ) =>
           GetCommand(sql, parameters, cnn)
           .ExecuteNonQuery();

        public int Execute(
           string sql
           , DbTransaction tran
       ) =>
           GetCommand(sql, null, tran)
           .ExecuteNonQuery();

        public int Execute(
           string sql
           , DbConnection cnn
       ) =>
           GetCommand(sql, null, cnn)
           .ExecuteNonQuery();

        public int Execute(
           string sql
           , ListParameter parameters
        ) =>
           GetCommand(sql, parameters)
           .ExecuteNonQuery();

        public int Execute(
            string sql
            , params (string nombre, object valor)[] parameters
        ) =>
            Execute(sql, new ListParameter(parameters));

        #endregion

        #region GetDataReader

        public DbDataReader GetDataReader(
            string sql
            , ListParameter parameters
            , DbConnection cnn
        ) =>
            GetCommand(sql, parameters, cnn)
            .ExecuteReader(
                cnn == null
                ? CommandBehavior.CloseConnection
                : CommandBehavior.Default
            );

        public DbDataReader GetDataReader(
            string sql
            , ListParameter parameters
            , DbTransaction tran
        ) =>
            GetCommand(sql, parameters, tran)
            .ExecuteReader(
                tran == null
                ? CommandBehavior.CloseConnection
                : CommandBehavior.Default
            );

        public DbDataReader GetDataReader(
            string sql
            , DbTransaction tran
        ) =>
            GetDataReader(sql, null, tran);

        public DbDataReader GetDataReader(
            string sql
            , DbConnection cnn
        ) =>
            GetDataReader(sql, null, cnn);

        public DbDataReader GetDataReader(
            string sql
            , ListParameter parameters
        ) =>
            GetCommand(sql, parameters)
            .ExecuteReader(
                CommandBehavior.CloseConnection
            );

        public DbDataReader GetDataReader(
            string sql
        ) =>
            GetCommand(sql)
            .ExecuteReader(
                CommandBehavior.CloseConnection
            );

        #endregion

        #region GetCommand

        private SQLiteCommand GetCommand(
            string sql
        ) => 
            new SQLiteCommand(sql, GetConnection());

        private SQLiteCommand GetCommand(
            string sql
            , ListParameter parameters
        ) => 
            new SQLiteCommand(sql)
            .Pipe(c => AddParams(c, parameters));


        private SQLiteCommand GetCommand(
            string sql
            , ListParameter parameters
            , DbTransaction tran
        )
        {
            if(tran == null)
            {
                return GetCommand(sql, parameters);
            }

            var cmd = GetCommand(sql, parameters, tran.Connection);
            cmd.Transaction = (SQLiteTransaction) tran;
            return cmd;
        }

        private SQLiteCommand GetCommand(
            string sql
            , ListParameter parameters
            , DbConnection cnn 
        )
        {
            var cmd = GetCommand(sql, parameters);
            cmd.Connection = 
                cnn as SQLiteConnection
                ?? GetConnection();
            return cmd;
        }

        #endregion

        public object ConvertTo(
            object v
            , PropertyInfo pi
        )
        {
            switch (pi.PropertyType.ToString())
            {
                case "System.Boolean":
                    return Convert.ToBoolean(v);
                case "System.Byte":
                    return Convert.ToByte(v);
                case "System.Int16":
                    return Convert.ToInt16(v);
                case "System.Int32":
                    return Convert.ToInt32(v);
                case "System.Int64":
                case "System.Nullable`1[System.Int64]":
                    return Convert.ToInt64(v);
                case "System.DateTime":
                    return Convert.ToDateTime(v);
                case "System.Char":
                    return Convert.ToChar(v);
                case "System.String":
                    return Convert.ToString(v);
                case "System.Decimal":
                case "System.Nullable`1[System.Decimal]":
                    return Convert.ToDecimal(v);
                case "System.Single":
                    return Convert.ToSingle(v);
                case "System.Double":
                    return Convert.ToDouble(v);
                default:
                    throw
                        new ArgumentException(
                            $"Tipo de dato inesperado {pi.PropertyType}"
                        );
            }
        }

        private SQLiteParameter GetParametro(Parameter p)
        {
            switch (p.Value.GetType().ToString())
            {
                case "System.Byte":
                    return new SQLiteParameter(p.Name, DbType.Byte)
                    {
                        Value = (byte)p.Value
                    };
                case "System.Int16":
                    return new SQLiteParameter(p.Name, DbType.Int16)
                    {
                        Value = (short)p.Value
                    };
                case "System.Int32":
                    return new SQLiteParameter(p.Name, DbType.Int32)
                    {
                        Value = (int)p.Value
                    };
                case "System.Int64":
                    return new SQLiteParameter(p.Name, DbType.Int64)
                    {
                        Value = (long)p.Value
                    };
                case "System.DateTime":
                    return new SQLiteParameter(p.Name, DbType.DateTime)
                    {
                        Value = (DateTime)p.Value
                    };
                case "System.Char":
                    return new SQLiteParameter(p.Name, DbType.String)
                    {
                        Value = (string)p.Value,
                        Size = ((string)p.Value).Length
                    };
                case "System.Decimal":
                    return new SQLiteParameter(p.Name, DbType.Decimal)
                    {
                        Value = (decimal)p.Value
                    };
                case "System.Single":
                    return new SQLiteParameter(p.Name, DbType.Single)
                    {
                        Value = (float)p.Value
                    };
                case "System.Double":
                    return new SQLiteParameter(p.Name, DbType.Double)
                    {
                        Value = (string)p.Value
                    };
                case "System.Boolean":
                    return new SQLiteParameter(p.Name, DbType.Boolean)
                    {
                        Value = (bool)p.Value
                    };
                default:
                    throw new ArgumentException(
                        $"Tipo de dato inesperado {p.Value.GetType()}"
                    );
            }
        }

        private SQLiteCommand AddParams(
            SQLiteCommand cmd
            , ListParameter parameters
        )
        {
            if ((parameters?.Count ?? 0) != 0)
            {
                parameters
                .Select(GetParametro)
                .ToArray()
                .Do(
                    cmd
                    .Parameters
                    .AddRange
                );

            }
            return cmd;
        }
    }

}
