using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm
{
	public class ListParameter : List<Parameter>
	{
		public void Add(
			string name
			, object value
		) => 
			Add(new Parameter { Name = name, Value = value });
	}
}
