using MiniOrm.Common;
using MiniOrm.EntityFramework.Attributes;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace MiniOrm.EntityFramework
{
    public static class EntityHelper
    {
        public static ListParameter GetKeyParameters<T>(
            T entity
        )
        {
            var type = entity.GetType();

            return
                GetPublicProperties(type)
                .Where(IsPrimaryKey)
                .Select(p => GetParameter(p, entity))
                .Pipe(l => new ListParameter(l))
                ;
        }        

        private static Parameter GetParameter<T>(
            PropertyInfo p
            , T entity
        ) =>
            new Parameter
            {
                Name = GetName(p),
                Value = p.GetValue(entity, null)
            }
            ;

        private static bool IsPrimaryKey(
            PropertyInfo p
        ) =>
            GetFieldAttribute(p)
                ?.IsPrimaryKey
            ?? false;

        public static ListParameter GetNonIdentityParameters<T>(
            T entity
        )
        {
            var type = entity.GetType();

            var properties =
                GetPublicProperties(type);

            var parameters =
                properties
                .Where(IsNotIdentity)
                .Select(p => GetParameter(p, entity));

            return new ListParameter(parameters);
        }

        private static bool IsNotIdentity(
            PropertyInfo p
        ) =>
            !IsIdentity(p);

        internal static long GetIdentityValue<T>(
            T entity
        ) where T : new()
        {
            var identityField = 
                GetPublicProperties(entity.GetType())
                .FirstOrDefault(IsIdentity);

            return identityField == null
                ? 0
                : Convert.ToInt64(
                    identityField
                    .GetValue(entity, null)
                );
        }

        private static bool IsIdentity(
            PropertyInfo p
        ) =>
            GetFieldAttribute(p)
                ?.IsIdentity
            ?? false;

        public static ListParameter GetParameters<T>(
            T entity
        )
        {
            var type = entity.GetType();

            var properties =
                GetPublicProperties(type);

            var parameters =
                properties
                .Select(p => GetParameter(p, entity));

            return new ListParameter(parameters);
        }

        public static string GetName(
            PropertyInfo property
        ) => 
            GetOrmAttribute(property)
            ?.Name
            ?? property.Name
            ;


        //public static string GetName(
        //    Type type
        //)
        //=> GetOrmAttribute(type)
        //    ?.Name
        //    ?? type.Name
        //    ;

        public static OrmAttribute GetOrmAttribute(
            Type type
        ) =>
            (OrmAttribute)
            Attribute
            .GetCustomAttribute(
                type
                , typeof(OrmAttribute)
            )
            ;
        public static OrmAttribute GetOrmAttribute(
            PropertyInfo property
        ) => 
            GetAttribute<OrmAttribute>(property);

        public static FieldAttribute GetFieldAttribute(
            PropertyInfo property
        ) => 
            GetAttribute<FieldAttribute>(property);

        public static T GetAttribute<T>(
           PropertyInfo property
        ) where T : Attribute =>
            property
               .GetCustomAttributes(false)
               .FirstOrDefault(
                   a => a.GetType() == typeof(T)
               ) as T;

        public static FieldAttribute GetFieldAttribute(
            Type type
        )
        {
            return ((FieldAttribute)
             Attribute
             .GetCustomAttribute(
                 type
                 , typeof(FieldAttribute)
             ))
             ;
        }

        public static TableAttribute GetTableAttribute(
            Type type
        ) =>
            (TableAttribute)
            Attribute
            .GetCustomAttribute(
                type
                , typeof(TableAttribute)
            )
            ;

        public static string GetTableName(
            Type type
        )
        {
            var att = 
                GetTableAttribute(type);

            return 
                att == null 
                ? type.Name 
                : $"{att.Schema}.{att.Name}";
        }

        public static T GetEntity<T>(
            DbDataReader dr
            , List<string> columnNames
            , List<PropertyInfo> properties
        ) where T : new()
        {
            var t = new T();

            foreach (var cn in columnNames)
            {
                properties
                .FirstOrDefault(
                    p =>
                    p.Name.ToUpper() == cn.ToUpper()
                )
                ?.SetValue(
                    t
                    , DBNull.Value == dr[cn]
                        ? null
                        : dr[cn]
                    , null
                );
            }
            return t;
        }

        public static List<PropertyInfo> GetPublicProperties(
            Type type
        )
        {
            return
                type
                .GetProperties()
                .Where(
                    p =>
                        !p.PropertyType.IsClass
                        || p.PropertyType == typeof(string)
                )
                .ToList()
                ;
        }

        public static int ToInt32(
            string i
            , int defaultValue = 0
        )
        {
            try
            {
                return int.Parse(i);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static T Transform<T, TR>(TR obj)
        {
            throw new NotImplementedException();
        }
    }
}
