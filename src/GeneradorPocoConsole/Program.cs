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
            var lineas = File.ReadAllLines("d:\\tmp\\poco\\conexion.txt");

            var cnn = lineas[0];
            var dir = lineas[1];
            var nameSpace = lineas[2];

            var gen = 
                new Generator(
                    new SqlObjectFactory(cnn)
                );

            gen.GenerateClasses(
                dir
                , nameSpace
            );

            Console.WriteLine("proceso terminado");
            Console.ReadLine();
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
