using System.Linq;
using MiniOrm.Common;

namespace MiniOrm.Sql
{
    public class SqlTableAdapter<T>
		: SqlEntityAdapter<T>
			, Common.ITableAdapter
		where T : new()
	{
		public SqlTableAdapter(string strCnn)
			: base(strCnn)
		{
		}

		
		public string CreateQueryDelete(
			string tableName
			, ListParameter keys
		) =>
			$"DELETE FROM {tableName} " +
			GetWhere(keys)
			;

		public string CreateQueryInsert(
			string tableName
			, ListParameter parameters = null
		) =>
			$"INSERTO INTO {tableName}" +
			$@" ({
					parameters
					.Select(p => p.Name)
					.JoinWith(", ")
				})" +
			$@" VALUES({
					parameters
					.Select(p => $"@{p.Name}")
					.JoinWith(", ")
				})";

		public string CreateQueryUpdate(
			string tableName
			, ListParameter parameters = null
			, ListParameter keys = null
		) =>
			$"UPDATE {tableName} " +
			$"SET " +
				$@"{
					parameters
					.Select(ToQueryEqual)
					.JoinWith(",")
				} " +
			GetWhere(keys)
			;
	}
}
