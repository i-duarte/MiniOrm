using System.Collections.Generic;

namespace PocoClassGen.Sql
{
    public class Table
    {
        public string Schema { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public List<Column> Columns { get; set; } 
            = new List<Column>();
    }
}
