using MiniOrm.Sql;
using PocoClassGen.Sql;

namespace GenerardorPocoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var gen = 
                new Generator(
                    new SqlDataAdapter(args[0])
                );

            gen.GenerateClasses(
                args[1]
                , args[2]
            );
        }
    }
}
