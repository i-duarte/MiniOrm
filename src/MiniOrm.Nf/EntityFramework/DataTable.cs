using MiniOrm.Common;
using System.Collections.Generic;
using System.Data.Common;

namespace MiniOrm.EntityFramework
{
    public class DataTable<T>
        : DataEntity<T>
        where T : new()
    {
        protected ITableAdapter TableAdapter { get; set; }
        
        public DataTable(DataBase db)
            : base(db)
        {
            TableAdapter = ObjectFactory.CreateTableAdapter<T>();
        }

        public int Delete(
            params (string nombre, object valor)[] parameters
        ) => 
            Delete(new ListParameter(parameters));

        public int Delete(
            ListParameter parameters
        ) => 
            TableAdapter
                .Execute(
                    TableAdapter.CreateQueryDelete(
                        EntityHelper.GetTableName(
                            typeof(T))
                            , parameters
                        )
                    , parameters);

        private T Insert(
            T entity
            , DbConnection cnn 
            , DbTransaction tran 
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

            var i =
                tran == null
                ? TableAdapter.Execute(
                        sql
                        , parameters
                        , cnn
                    )
                : TableAdapter.Execute(
                        sql
                        , parameters
                        , tran
                    )
                ;

            if (i == 0)
            {
                return default;
            }

            parameters =
                EntityHelper
                .GetNonIdentityParameters(entity);

            return GetEntity(parameters, tran);
        }

        public T Insert(
            T entity
            , DbTransaction tran
        ) =>
            Insert(entity, null, tran);

        public T Insert(
            T entity
            , DbConnection cnn = null
        ) =>
            Insert(entity, cnn, null);

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
