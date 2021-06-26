using MiniOrm.Common;
using System.Collections.Generic;
using System.Data.Common;

namespace MiniOrm.EntityFramework
{
    public class DataEntity<T> 
        : DataSource
            where T : new()
    {
        protected IEntityAdapter EntityAdapter { get; set; }

        public DataEntity(
            IEntityAdapter entityAdapter
        ) : base(entityAdapter)
        {
            EntityAdapter = entityAdapter;
        }

        public T GetEntity(
        ) =>
            GetEntity(null, null, null);

        public T GetEntity(
            string sql
        ) =>
            GetEntity(sql, null, null);

        public T GetEntity(
            string sql
            , ListParameter parameters
        ) =>
            GetEntity(sql, parameters, null);

        public T GetEntity(
            ListParameter parameters
            , DbConnection cnn
        ) =>
            GetEntity(null, parameters, cnn);

        public T GetEntity(
            ListParameter parameters
        ) =>
            GetEntity(null, parameters, null);

        public T GetEntity(
            DbConnection cnn
        ) =>
            GetEntity(null, null, cnn);

        public T GetEntity(
            string sql
            , ListParameter parameters 
            , DbConnection cnn 
        ) =>
            GetEntity<T>(
                string.IsNullOrEmpty(sql)
                ? GetSelect(parameters)
                : sql
                , parameters
                , cnn
            );

        public IEnumerable<T> Select(
       ) =>
           Select(null, null, null);

        public IEnumerable<T> Select(
            string sql
        ) =>
            Select(sql, null, null);

        public IEnumerable<T> Select(
            string sql
            , ListParameter parameters
        ) =>
            Select(sql, parameters, null);

        public IEnumerable<T> Select(
            ListParameter parameters
            , DbConnection cnn
        ) =>
            Select(null, parameters, cnn);

        public IEnumerable<T> Select(
            ListParameter parameters
        ) =>
            Select(null, parameters, null);

        public IEnumerable<T> Select(
            DbConnection cnn
        ) =>
            Select(null, null, cnn);

        public IEnumerable<T> Select(
            string sql
            , ListParameter parameters
            , DbConnection cnn
        ) =>
            GetEnumerable<T>(
                string.IsNullOrEmpty(sql)
                ? GetSelect(parameters)
                : sql
                , parameters
                , cnn
            );

        private string GetSelect(
            ListParameter parameters
        ) => 
            EntityAdapter
            .CreateQuerySelect(
                EntityHelper
                    .GetTableName(typeof(T))
                , parameters
            );
    }
}
