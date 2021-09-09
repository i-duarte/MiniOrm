using MiniOrm.Common;
using MiniOrm.EntityFramework;
using System.Collections.Generic;

namespace PocoClassGen.Sql
{
    public class DataBaseData : DataSource
    {
        public DataBaseData(IObjectFactory objectFactory) 
            : base(objectFactory)
        {
        }

        public IEnumerable<DataBase> QueryDataBases()
        {
            var sql = 
                @"
                SELECT *
                FROM sys.databases
                ";
            return GetEnumerable<DataBase>(sql);
        }

        public IEnumerable<Table> QueryTables()
        {
            var sql =
                @"
                SELECT 
                    TABLE_SCHEMA Schema
                    , TABLE_NAME Name
                    , TABLE_TYPE Type
                FROM INFORMATION_SCHEMA.TABLES
                ";
            return GetEnumerable<Table>(sql);
        }

        public IEnumerable<Column> QueryColumns(string tableName)
        {
            var sql =
                @"
                SELECT 
                    TABLE_SCHEMA Schema
                    , COLUMN_NAME Name
                    , DATA_TYPE Type
                    , CHARACTER_MAXIMUM_LENGTH MaxLen
                    , NUMERIC_PRECISION Precision
                    , NUMERIC_SCALE Scale
                FROM 
                    INFORMATION_SCHEMA.COLUMNS
                WHERE 
                    TABLE_NAME = @tableName
                ORDER BY 
                    TABLE_SCHEMA
                    , TABLE_NAME
                    , ORDINAL_POSITION
                ";

            return GetEnumerable<Column>(
                sql
                , new ListParameter { 
                    new Parameter { 
                        Name = "tableName", 
                        Value = tableName 
                    }
                }
            );
        }

        public IEnumerable<Column> QueryColumns()
        {
            var sql =
                @"
                SELECT 
                    C.TABLE_SCHEMA [Schema]
                    , C.TABLE_NAME [Table]
                    , C.COLUMN_NAME Name
                    , C.DATA_TYPE Type
                    , C.CHARACTER_MAXIMUM_LENGTH MaxLen
                    , C.NUMERIC_PRECISION Precision
                    , C.NUMERIC_SCALE Scale
					, COLUMNPROPERTY(object_id(C.TABLE_SCHEMA + '.' + C.TABLE_NAME), C.COLUMN_NAME, 'IsIdentity') IsIdentity
					, CASE WHEN CCU.COLUMN_NAME IS NULL THEN 0 ELSE 1 END IsPrimaryKey
                FROM 
                    INFORMATION_SCHEMA.COLUMNS C
						LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC ON
							C.TABLE_SCHEMA = TC.TABLE_SCHEMA 
							AND C.TABLE_NAME = TC.TABLE_NAME 
							AND TC.CONSTRAINT_TYPE = 'PRIMARY KEY'
						LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CCU ON
							TC.CONSTRAINT_NAME = CCU.CONSTRAINT_NAME 
							AND TC.TABLE_SCHEMA = CCU.TABLE_SCHEMA 
							AND TC.TABLE_NAME = CCU.TABLE_NAME
							AND CCU.COLUMN_NAME = C.COLUMN_NAME 
                WHERE 
                    NOT C.TABLE_SCHEMA IS NULL
                    AND C.TABLE_NAME <> 'sysdiagrams'
                ORDER BY 
                    C.TABLE_SCHEMA
                    , C.TABLE_NAME
                    , C.ORDINAL_POSITION
                ";

            return GetEnumerable<Column>(sql);
        }
    }
}
