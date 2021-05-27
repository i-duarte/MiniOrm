using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using MiniOrm.Common;

namespace MiniOrm.Sql
{
    public class SqlDataAdapter
		: Common.IDataAdapter 
    {
		private string StrCnn { get; set; }

		public SqlDataAdapter(string strCnn)
		{
			StrCnn = strCnn;
		}
		
        public int Execute(
            string sql
            , ListParameter parameters = null
            , DbConnection cnn = null
        ) =>
			GetCommand(sql, parameters, cnn)
			.ExecuteNonQuery();

        public DbDataReader GetDataReader(
            string sql
            , ListParameter parameters
            , DbConnection cnn
        ) => 
			GetCommand(sql, parameters, cnn)
            .ExecuteReader(
                CommandBehavior.CloseConnection
            );


		private SqlCommand GetCommand(
			string sql
			, ListParameter parameters
			, DbConnection cnn
		)
		{
			var cmd =
				new SqlCommand(
					sql
					, (SqlConnection)cnn
						?? GetConnection()
				);

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
					return new SqlParameter(p.Name, SqlDbType.TinyInt) {
						Value = (byte)p.Value 
					};
				case "System.Int16":
					return new SqlParameter(p.Name, SqlDbType.SmallInt) { 
						Value = (short)p.Value 
					};
				case "System.Int32":
					return new SqlParameter(p.Name, SqlDbType.Int) { 
						Value = (int)p.Value 
					};
				case "System.DateTime":
					return new SqlParameter(p.Name, SqlDbType.DateTime) { 
						Value = (DateTime)p.Value 
					};
				case "System.Char":
					return new SqlParameter(p.Name, SqlDbType.VarChar) { 
						Value = (string)p.Value 
					};
				case "System.String":
					return new SqlParameter(p.Name, SqlDbType.VarChar) {
						Value = (string)p.Value, Size = ((string)p.Value).Length 
					};
				case "System.Decimal":
					return new SqlParameter(p.Name, SqlDbType.Decimal) { 
						Value = (decimal)p.Value 
					};
				case "System.Single":
					return new SqlParameter(p.Name, SqlDbType.Real) { 
						Value = (float)p.Value 
					};
				case "System.Double":
					return new SqlParameter(p.Name, SqlDbType.Float) { 
						Value = (string)p.Value 
					};
				default:
					throw new ArgumentException(
						$"Tipo de dato inesperado {p.Value.GetType()}"
					);
			}
		}

		private SqlConnection GetConnection()
		{
            var cnn = 
				new SqlConnection(
					StrCnn.Contains("|") 
					? GetStrCnnFromPipeStr(StrCnn) 
					: StrCnn
				);
			cnn.Open();
			return cnn;
		}

        private string GetStrCnnFromPipeStr(string pipeCnn)
        {
			var arr = pipeCnn.Split('|');
			switch (arr.Length)
			{
				case 2:
					return GetStrConexion(arr[0], arr[1], "", "");					
				case 4:
					return GetStrConexion(arr[0], arr[1], arr[2], arr[3]);
				default:
					throw new Exception("Formato incorrecto de pipeCnn");
			}
		}

		private string GetStrConexion(
			string dataSource
			, string dbName
			, string user
			, string password
			, int timeOut = 30
		) =>
			$"Server={dataSource};"
				+ $"Database={dbName};"
				+ GetLoginCnnStr(user, password)
				+ "Pooling=false;"
				+ $"connection timeout={timeOut};"
				;

		private string GetLoginCnnStr(
			string user
			, string password
		) =>
			string.IsNullOrEmpty(user)
				&& string.IsNullOrEmpty(password)
			? "Trusted_Connection=True;"
			: $"User ID={user};"
				+ $"Password={password};";

        
    }
}
