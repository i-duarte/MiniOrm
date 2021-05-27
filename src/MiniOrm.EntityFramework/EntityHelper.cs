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
				.Select(GetParameter)
				.Pipe(l => new ListParameter(l))
				;
		}

        private static Parameter GetParameter<T>(
			PropertyInfo p
			, T entity
		) => 
			new Parameter
			{
				Name = GetName(p.GetType()),
				Value = p.GetValue(entity, null)
			}
			;

        private static bool IsPrimaryKey(
			PropertyInfo p
		) => 
			GetFieldAttribute(p.GetType())
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
				.Select(GetParameter);

			return new ListParameter(parameters);
		}

		private static bool IsNotIdentity(
			PropertyInfo p
		) =>
			!IsIdentity(p);

		private static bool IsIdentity(
			PropertyInfo p
		) =>
			GetFieldAttribute(p.GetType())
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
				.Select(GetParameter);

			return new ListParameter(parameters);
		}

		public static string GetName(
			Type type
		)
		=> GetOrmAttribute(type)
			?.Name
			?? type.Name
			;

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
		) =>
			GetTableAttribute(type)
			?.Name
			?? type.Name;

		public static T GetEntity<T>(
			DbDataReader dr
			, List<string> columnNames
			, List<PropertyInfo> properties
		) where T : new ()
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
	}
}
