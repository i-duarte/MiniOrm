namespace MiniOrm.EntityFramework.Attributes
{
    public class FieldAttribute
        : OrmAttribute
    {
		public bool IsPrimaryKey { get; set; }
		public bool IsIdentity { get; set; }		
	}
}
