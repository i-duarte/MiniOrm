using System;
using System.Collections;

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
}
