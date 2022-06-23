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

        public DataBase(DataBase db) 
            : this(db.ObjectFactory)
        {            
        }

        public System.Data.Common.DbConnection CreateConnection(
            int timeOut = 30
        ) => 
            ObjectFactory.CreateConnection(timeOut);
    }
}
