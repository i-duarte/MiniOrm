using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace MiniOrm
{
	public class DataSource<T> where T : new()
	{
		private string StrCnn { get; set; }

		public DataSource(string strCnn)
		{
			StrCnn = strCnn;
		}

		protected IEnumerable<Tx> GetEnumerable<Tx>(
			string sql
			, ListParameter parametros = null
		) where Tx : new()
		{
			using (var dr = GetDataReader(sql, parametros))
			{
				var columnNames = 
					dr.GetColumnNames()
					.ToList();

				while (dr.Read())
				{

					yield return 
						GetEntity<Tx>(
							dr
							, columnNames
							, GetPublicProperties(typeof(Tx))
						);
				}
				dr.Close();
			}
		}

		protected IEnumerable<T> GetEnumerable(
			string sql
		) => 
			GetEnumerable<T>(sql);


		protected SqlDataReader GetDataReader(
			string sql
			, ListParameter parametros
			, SqlConnection cnn = null
		)
		{
			var cmd =
				new SqlCommand(
				 sql
				 , cnn ?? GetConnection()
			 );

			if ((parametros?.Count??0) != 0)
			{
				var p = parametros.Select(GetParametro)
					.ToArray();
				cmd
				.Parameters
				.AddRange(
					p
				);
			}

			return 
				cmd.ExecuteReader(
					 CommandBehavior.CloseConnection
				 );
		}

		private SqlParameter GetParametro(Parameter p)
		{
			switch (p.Value.GetType().ToString())
			{
				case "System.Byte":
					return new SqlParameter(p.Name, SqlDbType.TinyInt) { Value = (byte)p.Value };
				case "System.Int16":
					return new SqlParameter(p.Name, SqlDbType.SmallInt) { Value = (short)p.Value };
				case "System.Int32":
					return new SqlParameter(p.Name, SqlDbType.Int) { Value = (int)p.Value };
				case "System.DateTime":
					return new SqlParameter(p.Name, SqlDbType.DateTime) { Value = (DateTime)p.Value };
				case "System.Char":
					return new SqlParameter(p.Name, SqlDbType.VarChar) { Value = (string)p.Value };
				case "System.String":
					return new SqlParameter(p.Name, SqlDbType.VarChar) { Value = (string)p.Value, Size = ((string)p.Value).Length };
				case "System.Decimal":
					return new SqlParameter(p.Name, SqlDbType.Decimal) { Value = (decimal)p.Value };
				case "System.Single":
					return new SqlParameter(p.Name, SqlDbType.Real) { Value = (float)p.Value };
				case "System.Double":
					return new SqlParameter(p.Name, SqlDbType.Float) { Value = (string)p.Value };
				default:
					throw new ArgumentException($"Tipo de dato inesperado {p.Value.GetType()}");
			}
		}

		protected SqlConnection GetConnection()
		{
			var cnn = new SqlConnection(StrCnn);
			cnn.Open();
			return cnn;
		}

		protected Tx GetEntity<Tx>(
			SqlDataReader dr
			, List<string> columnNames
			, List<PropertyInfo> properties			
		) where Tx : new()
		{
			var t = new Tx();

			foreach (var cn in columnNames)
			{
				if (DBNull.Value == dr[cn])
				{
					properties
						.FirstOrDefault(p=>p.Name == cn)
						?.SetValue(t, null);
				}
				else
				{
					properties
						.FirstOrDefault(p => p.Name == cn)
						?.SetValue(t, dr[cn]);
				}
			}
			return t;
		}

		protected T GetEntity(
			SqlDataReader dr
			, List<string> columnNames
			, List<PropertyInfo> properties
		) =>
			GetEntity<T>(dr, columnNames, properties);

		private List<PropertyInfo> GetPublicProperties(
			Type type 
		)
		{
			return
				(type ?? typeof(T))
				.GetProperties()
				.Where(
					p => 
						!p.PropertyType.IsClass
						|| p.PropertyType == typeof(String)
				)
				.ToList()
				;
		}
	}
}
