using MiniOrm.Common;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace MiniOrm.Sql
{
    public class SqlDataAdapter
        : Common.IDataAdapter
    {
        private SqlObjectFactory SqlObjectFactory { get; set; }

        public SqlDataAdapter(
            SqlObjectFactory sqlObjectFactory
        )
        {
            SqlObjectFactory = sqlObjectFactory;
        }

        private SqlConnection GetConnection(
        ) =>
            (SqlConnection)
            SqlObjectFactory.CreateConnection();

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

        private SqlCommand GetCommand(
            string sql
        ) => 
            new SqlCommand(sql, GetConnection());

        private SqlCommand GetCommand(
            string sql
            , ListParameter parameters
        ) => 
            new SqlCommand(sql)
            .Pipe(c => AddParams(c, parameters));


        private SqlCommand GetCommand(
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
            cmd.Transaction = (SqlTransaction) tran;
            return cmd;
        }

        private SqlCommand GetCommand(
            string sql
            , ListParameter parameters
            , DbConnection cnn 
        )
        {
            var cmd = GetCommand(sql, parameters);
            cmd.Connection = 
                cnn as SqlConnection
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

        private SqlParameter GetParametro(Parameter p)
        {
            switch (p.Value.GetType().ToString())
            {
                case "System.Byte":
                    return new SqlParameter(p.Name, SqlDbType.TinyInt)
                    {
                        Value = (byte)p.Value
                    };
                case "System.Int16":
                    return new SqlParameter(p.Name, SqlDbType.SmallInt)
                    {
                        Value = (short)p.Value
                    };
                case "System.Int32":
                    return new SqlParameter(p.Name, SqlDbType.Int)
                    {
                        Value = (int)p.Value
                    };
                case "System.Int64":
                    return new SqlParameter(p.Name, SqlDbType.BigInt)
                    {
                        Value = (long)p.Value
                    };
                case "System.DateTime":
                    return new SqlParameter(p.Name, SqlDbType.DateTime)
                    {
                        Value = (DateTime)p.Value
                    };
                case "System.Char":
                    return new SqlParameter(p.Name, SqlDbType.VarChar)
                    {
                        Value = (string)p.Value
                    };
                case "System.String":
                    return new SqlParameter(p.Name, SqlDbType.VarChar)
                    {
                        Value = (string)p.Value,
                        Size = ((string)p.Value).Length
                    };
                case "System.Decimal":
                    return new SqlParameter(p.Name, SqlDbType.Decimal)
                    {
                        Value = (decimal)p.Value
                    };
                case "System.Single":
                    return new SqlParameter(p.Name, SqlDbType.Real)
                    {
                        Value = (float)p.Value
                    };
                case "System.Double":
                    return new SqlParameter(p.Name, SqlDbType.Float)
                    {
                        Value = (string)p.Value
                    };
                case "System.Boolean":
                    return new SqlParameter(p.Name, SqlDbType.Bit)
                    {
                        Value = (bool)p.Value
                    };
                default:
                    throw new ArgumentException(
                        $"Tipo de dato inesperado {p.Value.GetType()}"
                    );
            }
        }

        private SqlCommand AddParams(
            SqlCommand cmd
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
