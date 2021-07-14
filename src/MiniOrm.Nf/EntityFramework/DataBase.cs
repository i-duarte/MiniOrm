using MiniOrm.Common;

namespace MiniOrm.EntityFramework
{
    public class DataBase
    {
        public IObjectFactory ObjectFactory { get; set; }
        public DataBase(IObjectFactory objectFactory)
        {
            ObjectFactory = objectFactory;
        }

        public System.Data.Common.DbConnection CreateConnection(
        ) => 
            ObjectFactory.CreateConnection();
    }
}
