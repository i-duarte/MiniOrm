namespace MiniOrm.Common
{
    public interface ITableAdapter
        : IEntityAdapter
    {

        string CreateQueryInsert(
            string tableName
            , ListParameter parameters = null
        );

        string CreateQueryUpdate(
            string tableName
            , ListParameter parameters = null
            , ListParameter keys = null
        );

        string CreateQueryDelete(
            string tableName
            , ListParameter keys
        );        
    }
}
