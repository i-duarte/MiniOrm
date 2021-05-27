using System;
using System.Collections.Generic;
using System.Data;

namespace MiniOrm.Common
{
	public static class MyExtensions
	{
		public static void Do<T>(
			this T source
			, Action<T> a
		)
        {
			a(source);
        }

		public static TR Pipe<T, TR>(
			this T source
			, Func<T, TR> f
		)
        {
			return f(source);
        }

		public static IEnumerable<string> GetColumnNames(
			this IDataReader source
		)
		{
			for (var i = 0; i < source.FieldCount; i++)
			{
				yield return source.GetName(i);
			}
		}

        public static string JoinWith(
            this IEnumerable<string> source
            , string separator
        ) =>
			string.Join(separator, source);

		public static string Prepend(
			this string source
			, string item
		)
		{
			return item + source;
		}

		public static IEnumerable<T> Prepend<T>(
			this IEnumerable<T> source
			, T item
		)
        {
			yield return item;
			foreach(var i in source)
            {
				yield return i;
            }
        }
    }
}
