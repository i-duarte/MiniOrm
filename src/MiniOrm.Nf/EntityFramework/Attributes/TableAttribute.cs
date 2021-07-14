namespace MiniOrm.EntityFramework.Attributes
{
    public class TableAttribute
        : OrmAttribute
    {
        public string Schema { get; set; } = "dbo";
    }
}
