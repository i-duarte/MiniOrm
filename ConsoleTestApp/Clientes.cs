using MiniOrm.Npgsql;
using System.Collections.Generic;

namespace ConsoleTestApp
{
	internal class Clientes : DataSource
	{
		public Clientes(string strCnn) : base(strCnn)
		{
		}

		public IEnumerable<Cliente> SelectAll()
		{
			var sql = "SELECT * FROM clientes";
			return GetEnumerable<Cliente>(sql);
		}
	}
}
