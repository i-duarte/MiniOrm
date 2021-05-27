namespace PocoClassGen.Sql
{
    public class Column
    {
        public string Schema { get; set; }
        public string Table { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int MaxLen { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsPrimaryKey { get; set; }
    }
}
