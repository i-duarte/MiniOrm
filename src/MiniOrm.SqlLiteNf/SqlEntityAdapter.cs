using MiniOrm.Common;
using System.Linq;

namespace MiniOrm.Sql
{
    public class SqlEntityAdapter<T>
        : SqlDataAdapter
            , IEntityAdapter
        where T : new()
    {
        public SqlEntityAdapter(
            SqlObjectFactory sqlObjectFactory
        ) : base(sqlObjectFactory)
        {
        }

        protected string ToQueryEqual(
            Parameter p
        ) =>
            $"{p.Name} = @{p.Name}";

        public string GetWhere(
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
