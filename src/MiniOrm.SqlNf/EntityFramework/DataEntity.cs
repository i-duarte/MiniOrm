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
            ListParameter parameters
            , DbConnection cnn = null
        ) =>
            GetEntity(
                EntityAdapter
                .CreateQuerySelect(
                    EntityHelper
                    .GetTableName(typeof(T))
                    , parameters
                )
                , parameters
                , cnn
            );

        public T GetEntity(
            string sql
            , ListParameter parameters = null
            , DbConnection cnn = null
        ) =>
            GetEntity<T>(
                sql
                , parameters
                , cnn
            );

        public IEnumerable<T> Select(
            ListParameter parameters = null
            , DbConnection cnn = null
        ) =>
            GetEnumerable<T>(
                EntityAdapter
                    .CreateQuerySelect(
                        EntityHelper
                            .GetTableName(typeof(T))
                        , parameters
                    )
                , parameters
                , cnn
            );

        public IEnumerable<T> Select(
            DbConnection cnn = null
        ) =>
            GetEnumerable<T>(
                EntityAdapter
                .CreateQuerySelect(
                        EntityHelper
                            .GetTableName(typeof(T))
                    )
                , null
                , cnn
            );
    }
}
