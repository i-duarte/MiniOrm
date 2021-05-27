using System.Collections.Generic;

namespace PocoClassGen.Sql
{
    public class Schema
    {
        public string Name { get; set; }
        public List<Table> Tables { get; set; }
            = new List<Table>();
    }
}
