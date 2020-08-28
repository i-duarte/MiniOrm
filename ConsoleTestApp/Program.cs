using MiniOrmPg;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleTestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			var clientes = 
				new Clientes(
					"User ID=postgres" +
					";Password=123" +
					";Host=localhost" +
					";Port=5432" +
					";Database=OncData" +
					";Pooling=false" +					
					";"
				);

			foreach(var c in clientes.SelectAll())
			{
				Console.WriteLine(c.Id);
			}
			
		}
	}

	internal class Cliente
	{
		public long Id { get; set; }
	}

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
