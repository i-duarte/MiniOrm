﻿using MiniOrm.Common;
using System;
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
            IObjectFactory objectFactory
        ) : base(objectFactory)
        {
            EntityAdapter = objectFactory.CreateEntityAdapter<T>();
        }

        public T GetEntity(
            ListParameter parameters
            , DbConnection cnn
        ) =>
            GetEntity("", parameters, cnn);

        public T GetEntity(
            string nombre
            , object valor
        ) => 
            GetEntity(new ListParameter(nombre, valor));

        public T GetEntity(
            ListParameter parameters
        ) =>
            GetEntity("", parameters);

        public T GetEntity(
            DbConnection cnn
        ) =>
            GetEntity("", null, cnn);

        public T GetEntity(
            string sql 
            , DbTransaction tran
        ) =>
            GetEntity(sql, null, tran);

        public T GetEntity(
            ListParameter parameters
            , DbTransaction tran
        ) =>
            GetEntity("", parameters, tran);

        public T GetEntity(
            string sql
            , ListParameter parameters
            , DbTransaction tran
        ) =>
            GetEntity<T>(
                string.IsNullOrEmpty(sql)
                ? GetSelect(parameters)
                : sql
                , parameters
                , tran
            );

        public T GetEntity(
            string sql
            , ListParameter parameters = null
            , DbConnection cnn = null
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

        public int[] Where(Func<object, object> p)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Select(
            ListParameter parameters
            , DbConnection cnn
        ) =>
            Select(null, parameters, cnn);

        public IEnumerable<T> Select(
            string nombre, object valor
        ) =>
            Select(
                null
                , new ListParameter(nombre, valor)
                , null
            );

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
