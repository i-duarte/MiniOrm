using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MiniOrm
{
	internal static class MyExtensions
	{
		public static IEnumerable<string> GetColumnNames(this SqlDataReader source)
		{
			for (var i = 0; i < source.FieldCount; i++)
			{
				yield return source.GetName(i);
			}
		}
	}
}
