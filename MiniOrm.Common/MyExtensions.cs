using System.Collections.Generic;
using System.Data;

namespace MiniOrm.Common
{
	public static class MyExtensions
	{
		public static IEnumerable<string> GetColumnNames(
			this IDataReader source
		)
		{
			for (var i = 0; i < source.FieldCount; i++)
			{
				yield return source.GetName(i);
			}
		}

		
	}
}
