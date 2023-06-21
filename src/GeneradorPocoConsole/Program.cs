using MiniOrm.Common;
using MiniOrm.MsAccessNf;
using MiniOrm.Sql;
using PocoClassGen.Sql;
using System;
using System.IO;

namespace GenerardorPocoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var lineas = File.ReadAllLines("f:\\tmp\\poco\\conexion.txt");

            var tipo = lineas[0];
            var cnn = lineas[1];
            var dir = lineas[2];
            var nameSpace = lineas[3];

            MiniOrm.Common.IObjectFactory factory = null;

            if(tipo == "access")
            {
                factory = new MsaObjectFactory(cnn);
            }
            else
            {
                factory = new SqlObjectFactory(cnn);
            }

            Generar(new SqlObjectFactory(cnn), dir, nameSpace);

            Console.WriteLine("proceso terminado");
            Console.ReadLine();
        }

        private static void Generar(
            MiniOrm.Common.IObjectFactory objectFactory
            , string dir
            , string nameSpace
        )
        {
            var gen =
                new Generator(
                    objectFactory
                );

            gen.GenerateClasses(
                dir
                , nameSpace
            );
        }

        private static object GetUserAndPassword(
            string user
            , string pass
        ) => 
            string.IsNullOrEmpty(user)
            ? "" 
            : $"|{user}|\"{pass.Replace("\"", "\"\"")}\"";
    }
}
