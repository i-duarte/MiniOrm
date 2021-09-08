using MiniOrm.Common;
using System.Linq;

namespace MiniOrm.Sql
{
    public class NpgEntityAdapter<T>
        : NpgDataAdapter
            , IEntityAdapter
        where T : new()
    {
        public NpgEntityAdapter(
            NpgObjectFactory npgObjectFactory
        ) : base(npgObjectFactory)
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
