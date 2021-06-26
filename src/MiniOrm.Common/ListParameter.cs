using System.Collections.Generic;

namespace MiniOrm.Common
{
    public class ListParameter : List<Parameter>
	{
		public ListParameter() { }
		
		public ListParameter(string name, object value) 
		{
			Add(name, value);
		}

		public ListParameter(Parameter parameter)
        {
			Add(parameter);
        }
		
		public ListParameter(IEnumerable<Parameter> list)
        {
			AddRange(list);
        }

		public void Add(
			string name
			, object value
		) => 
			Add(
				new Parameter { 
					Name = name
					, Value = value 
				}
			);
	}
}
