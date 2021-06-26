using MiniOrm.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace MiniOrm.EntityFramework
{
    public class DataSource
    {
        protected IObjectFactory ObjectFactory { get; set; }
        public IDataAdapter DataAdapter { get; }

        public DataSource(IObjectFactory objectFactory)
        {
            ObjectFactory = objectFactory;
            DataAdapter = objectFactory.CreateDataAdapter();
        }

        protected T GetEntity<T>(
            DbDataReader dr
            , List<string> columnNames
            , List<PropertyInfo> properties
        ) where T : new()
        {
            var t = new T();

            foreach (var cn in columnNames)
            {
                var prop =
                    properties
                    .FirstOrDefault(
                        p =>
                        p.Name.ToUpper() == cn.ToUpper()
                    );

                prop?.SetValue(
                    t
                    , DBNull.Value == dr[cn]
                        ? null
                        : DataAdapter.ConvertTo(dr[cn], prop)
                    , null
                );
            }
            return t;
        }

        public T GetEntity<T>(
            string sql
            , ListParameter parameters = null
            , DbTransaction tran = null
        ) where T : new() =>
            GetEntityWithRead<T>(sql, parameters, tran);

        public T GetEntity<T>(
            string sql
            , ListParameter parameters = null
            , DbConnection cnn = null
        ) where T : new() => 
            GetEntityWithRead<T>(sql, parameters, cnn);

        private T GetEntityWithRead<T>(
            DbDataReader dr
        )
            where T : new()
        {
            if (dr.Read())
            {
                return
                    GetEntity<T>(
                        dr
                        , dr.GetColumnNames()
                            .ToList()
                        , EntityHelper
                            .GetPublicProperties(typeof(T))
                    );
            }
            return default;
        }

        private T GetEntityWithRead<T>(
            string sql
            , ListParameter parameters = null
            , DbConnection cnn = null
        )
            where T : new()
        {
            using (
                var dr =
                    DataAdapter.GetDataReader(
                        sql
                        , parameters
                        , cnn
                    )
            )
            {
                return GetEntityWithRead<T>(dr);
            }
        }

        private T GetEntityWithRead<T>(
            string sql
            , ListParameter parameters = null
            , DbTransaction tran = null
        )
            where T : new()
        {
            using (
                var dr =
                    DataAdapter.GetDataReader(
                        sql
                        , parameters
                        , tran
                    )
            )
            {
                return GetEntityWithRead<T>(dr);
            }
        }

        public IEnumerable<T> GetEnumerable<T>(
            string sql
            , ListParameter parameters = null
            , DbConnection cnn = null
        ) where T : new()
        {
            using (
                var dr =
                    DataAdapter.GetDataReader(
                        sql
                        , parameters
                        , cnn
                    )
            )
            {
                var columnNames =
                    dr.GetColumnNames()
                    .ToList()
                    ;

                var properties =
                    EntityHelper
                    .GetPublicProperties(typeof(T))
                    ;

                while (dr.Read())
                {

                    yield return
                        GetEntity<T>(
                            dr
                            , columnNames
                            , properties
                        );
                }
                dr.Close();
            }
        }

        public T Get<T>(
            string sql
            , ListParameter parameters            
        ) =>
            Get<T>(
                DataAdapter
                .GetDataReader(
                    sql
                    , parameters
                    , ObjectFactory.CreateConnection()
                )
            );

        public T Get<T>(
            string sql
            , ListParameter parameters
            , DbConnection cnn
        ) =>
            Get<T>(
                DataAdapter
                .GetDataReader(sql, parameters, cnn)
            );

        public T Get<T>(
            string sql
            , DbConnection cnn
        ) =>
            Get<T>(
                DataAdapter
                .GetDataReader(sql, null, cnn)
            );

        public T Get<T>(
            string sql
            , DbTransaction tran
        ) =>
            Get<T>(
                DataAdapter
                .GetDataReader(sql, null, tran)
            );

        public T Get<T>(
            string sql
            , ListParameter parameters
            , DbTransaction tran
        ) =>
            Get<T>(
                DataAdapter
                .GetDataReader(sql, parameters, tran)
            );

        private T Get<T>(DbDataReader dr)
        {
            if (dr.Read())
            {
                return (T)(dr[0]);
            }
            return default;
        }       
    }
}
