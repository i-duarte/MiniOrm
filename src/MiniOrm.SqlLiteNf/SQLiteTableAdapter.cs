using MiniOrm.Common;
using System.Linq;

namespace MiniOrm.SQLite
{
    public class SQliteTableAdapter<T>
        : SQLiteEntityAdapter<T>
            , ITableAdapter
        where T : new()
    {
        public SQliteTableAdapter(
            SQLiteObjectFactory sqlObjectFactory
        ) : base(sqlObjectFactory)
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
            $"INSERT INTO {tableName}" +
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
