namespace MiniOrm.Common
{
    public interface IEntityAdapter
        : IDataAdapter
    {
        string CreateQuerySelect(
            string tableName
            , ListParameter parameters = null
        );

        string GetWhere(
            ListParameter keys
        );
        
    }
}
