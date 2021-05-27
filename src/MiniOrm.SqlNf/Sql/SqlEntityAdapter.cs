using System.Linq;
using MiniOrm.Common;

namespace MiniOrm.Sql
{
    public class SqlEntityAdapter<T>
		: SqlDataAdapter
			, Common.IEntityAdapter 
		where T : new()
	{
        public SqlEntityAdapter(string strCnn) 
			: base(strCnn)
        {
        }

        protected string ToQueryEqual(
			Parameter p
		) =>
			$"{p.Name} = @{p.Name}";

        protected string GetWhere(
			ListParameter keys
		) => 
			keys == null
            ? ""
			: $" WHERE " +
                   $@"{
                       keys
                       .Select(ToQueryEqual)
                       .JoinWith(" AND ")
                   } ";

        public string CreateQuerySelect(
			string tableName
			, ListParameter parameters = null
		) =>
			$"SELECT * FROM { tableName }" +
			$" {GetWhere(parameters)}"			 
			;

	}
}
