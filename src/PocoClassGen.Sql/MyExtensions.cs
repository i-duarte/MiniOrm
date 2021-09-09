using System;
using System.Collections.Generic;

namespace PocoClassGen.Sql
{
    public static class MyExtensions
    {

        public static string Repeat(
            this char source
            , int n
        )
        {
            return new string(source, n);
        }

        public static string MenosUltimoEnter(
            this string source
        ) =>
            source[^2..] == Environment.NewLine 
            ? source[..^2] 
            : source;

        public static string MenosPrimerEnter(
            this string source
        ) => 
            source[(source.IndexOf(Environment.NewLine) + 2)..];

        public static TR Pipe<T, TR>(
            this T source
            , Func<T, TR> f
        ) =>
            f(source);

        public static string JoinWith(
            this IEnumerable<string> source
            , string separator
        ) =>
            string.Join(separator, source);
    }
}
