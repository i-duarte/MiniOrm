using MiniOrm.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PocoClassGen.Sql
{
    public class Generator
    {
        private DataBaseData Data { get; set; }
        public static string IdentacionPropiedades 
            => "        ";

        public static string SaltoMasIdentacionPropiedades
            => $"{Environment.NewLine}{IdentacionPropiedades}";

        public Generator(IDataAdapter dataAdapter)
        {
            Data = new DataBaseData(dataAdapter);
        }

        public void GenerateClasses(
            string dirPath
            , string nameSpace
        )
        {
            if(!Directory.Exists(dirPath))
            {
                throw new DirectoryNotFoundException();
            }

            var columns =
                Data
                .QueryColumns()
                .ToList()
                ;

            var schemas =
                Data
                .QueryColumns()
                .ToLookup(c => c.Schema)
                .Pipe(GetSchemas)
                .ToList()
                ;

            schemas
            .ForEach(
                sch => 
                GenerarEntidades(
                    sch
                    , nameSpace
                    , dirPath
                )
            );

            //schemas
            //.ForEach(
            //    sch =>
            //    GenerarBdClass(
            //        sch
            //        , nameSpace
            //        , dirPath
            //    )
            //);
        }

        private static void GenerarBdClass(
            Schema sch
            , string nameSpace
            , string dirPath
        )
        {
            
        }

        private static void GenerarEntidades(
            Schema sch
            , string nameSpace
            , string dir
        )
        {

            var fulldir = 
                Path.Combine(                    
                    dir
                    , "Entities"
                    , sch.Name == "dbo" 
                        ? ""
                        : sch.Name
                )
                ;

            if (!Directory.Exists(fulldir))
            {
                Directory.CreateDirectory(fulldir);
            }           

            sch.Tables
               .ForEach(
                    tbl =>
                    GenerarEntidad(
                        tbl
                        , nameSpace
                        , fulldir
                    )
               );
        }

        private static void GenerarEntidad(
            Table tbl
            , string nameSpace
            , string fulldir
        )
        {
            File.WriteAllText(
                Path.Combine(
                    fulldir
                    , $"{tbl.Name}.cs"
                )
                , GetStrClass(tbl, nameSpace)
            );
        }

        private static string GetStrClass(
            Table tbl
            , string nameSpace
        )
        {
            var nameSpaceFull = 
                $"{nameSpace}.Entities{(tbl.Schema == "dbo" ? "" : $".{tbl.Schema}")}";

            Console.WriteLine($"Generando {tbl.Name}.cs en {nameSpaceFull}");

            return
                $@"using System;
using MiniOrm.EntityFramework.Attributes;

namespace {nameSpaceFull}
{{
    public class {tbl.Name} 
    {{
        {GetStrPropiedades(tbl)}
    }}
}}
    ";
        }

        private static string GetStrPropiedades(
            Table tbl
        ) =>
            tbl
            .Columns
            .Select(GetPropiedad)
            .JoinWith(SaltoMasIdentacionPropiedades)            
            ;

        private static string GetPropiedad(
            Column col
        ) => 
            @$"{GetAttributes(col)}public {GetTipo(col)} {col.Name} {{ get; set; }}";

        private static string GetAttributes(Column col)
        {
            var att = new List<string>();

            if(col.IsIdentity)
            {
                att.Add("IsIdentity = true");
            }
            if(col.IsPrimaryKey)
            {
                att.Add("IsPrimaryKey = true");
            }

            if(att.Count == 0)
            {
                return "";
            }

            return $"[FieldAttribute({att.JoinWith(", ")})]{SaltoMasIdentacionPropiedades}";
        }

        private static string GetTipo(Column col)
        {
            return col.Type switch
            {
                "bigint" => "long",
                "bit" => "bool",
                "char" 
                    or "nchar" 
                    or "nvarchar" 
                    or "varchar" => "string",
                "varbinary" => "string",
                "date" 
                    or "time" 
                    or "datetime" 
                    or "datetime2" 
                    or "smalldatetime" => "DateTime",
                "decimal" => "decimal",
                "float" => "flota",
                "int" => "int",
                "smallint" => "short",
                "tinyint" => "byte",
                _ => throw new Exception(
                        $"Tipo desconocido [{col.Type}]"
                    ),
            };
        }

        private static IEnumerable<Schema> GetSchemas(
            ILookup<string, Column> groupSchemas
        ) => 
            groupSchemas
            .Select(GetSchema);

        private static Schema GetSchema(
            IGrouping<string, Column> groupSchema
        )
        {
            var groupTables =
                groupSchema
                .ToLookup(c => c.Table);
            
            var fs = groupSchema.First();

            var s = new Schema
            {
                Name = fs.Schema
            };

            s.Tables
                .AddRange(
                    groupTables
                    .Select(GetTable)
                );

            return s;
        }

        private static Table GetTable(
            IGrouping<string, Column> groupTable
        )
        {
            var ft = groupTable.First();
            var t = new Table
            {
                Name = ft.Table,
                Schema = ft.Schema,
                Type = ft.Type
            };

            t.Columns.AddRange(groupTable.ToList());

            return t;
        }
    }
}
