using MiniOrm.Common;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace MiniOrm.EntityFramework
{
    public class DataTable<T>
        : DataEntity<T>
        where T : new()
    {
        private ITableAdapter TableAdapter { get; set; }

        public DataTable(ITableAdapter tableAdapter) 			
            : base(tableAdapter)
        {
        }

        protected IEnumerable<T> GetEnumerable(
            string sql
            , ListParameter parameters = null
        ) =>
            GetEnumerable(sql, parameters);

        protected T GetEntity(
            string sql
            , ListParameter parameters = null
        ) =>
            GetEntity(sql, parameters);

        public int Insert(
            T entity
            , DbConnection cnn = null
        )
        {
            var parameters = 
                EntityHelper
                .GetNonIdentityParameters(entity);

            var sql =
                TableAdapter
                .CreateQueryInsert(
                    EntityHelper.GetTableName(typeof(T))
                    , parameters
                );

            return
                TableAdapter.Execute(
                    sql
                    , parameters
                    , cnn
                );
        }

        public int Update(
            T entity
            , DbConnection cnn = null
        )
        {
            var parameters =
                EntityHelper.GetParameters(entity)
                ;
            
            var keyParameters =
                EntityHelper.GetKeyParameters(entity)
                ;

            var sql =
                TableAdapter
                .CreateQueryUpdate(
                    EntityHelper.GetTableName(typeof(T))
                    , parameters
                    , keyParameters
                );

            return TableAdapter.Execute(sql, parameters, cnn);
        }
    }
}
