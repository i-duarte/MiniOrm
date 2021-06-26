using MiniOrm.Sql;
using PocoClassGen.Sql;
using System;

namespace GenerardorPocoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Server:");
            var server = Console.ReadLine();
            Console.Write("Instance\\Database:");
            var dataBase = Console.ReadLine();
            Console.Write("User:");
            var user = Console.ReadLine();
            Console.Write("Password:");
            var pass = Console.ReadLine();

            Console.Write("Directory:");
            var dir = Console.ReadLine();
            Console.Write("Namespace:");
            var nameSpace = Console.ReadLine();

            var gen = 
                new Generator(
                    new SqlObjectFactory(
                        $"{server}" +
                        $"|{dataBase}" +
                        $"{GetUserAndPassword(user,pass)}"
                    )
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
