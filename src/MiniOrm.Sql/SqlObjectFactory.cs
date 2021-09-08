using MiniOrm.Common;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace MiniOrm.Sql
{
    public class SqlObjectFactory :
        IObjectFactory
    {
        private string StrCnn { get; set; }

        public SqlObjectFactory(
            string dataSource
            , string dataBase
            , string user
            , string password
        )
        {
            StrCnn = 
                GetStrConexion(
                    dataSource
                    , dataBase
                    , user
                    , password
                );
        }

        public SqlObjectFactory(
            string dataSource
            , string dataBase            
        ) : this(dataSource, dataBase, "", "")
        {            
        }

        public SqlObjectFactory(string strCnn)
        {   
            if(strCnn.Contains("|"))
            {
                StrCnn = GetStrCnnFromPipeStr(strCnn);
            }
            else
            { 
                StrCnn = strCnn; 
            }
        }

        public DbConnection CreateConnection()
        {
            var cnn =
                new SqlConnection(StrCnn);
            cnn.Open();
            return cnn;
        }

        private string GetStrCnnFromPipeStr(string pipeCnn)
        {
            var arr = pipeCnn.Split('|');
            switch (arr.Length)
            {
                case 2:
                    return GetStrConexion(arr[0], arr[1], "", "");
                case 4:
                    return GetStrConexion(arr[0], arr[1], arr[2], arr[3]);
                default:
                    throw new Exception("Formato incorrecto de pipeCnn");
            }
        }

        private string GetStrConexion(
            string dataSource
            , string dbName
            , string user
            , string password
            , int timeOut = 30
        ) =>
            $"Server={dataSource};"
                + $"Database={dbName};"
                + GetLoginCnnStr(user, password)
                + "Pooling=false;"
                + $"connection timeout={timeOut};"
                ;

        private string GetLoginCnnStr(
            string user
            , string password
        ) =>
            string.IsNullOrEmpty(user)
                && string.IsNullOrEmpty(password)
            ? "Trusted_Connection=True;"
            : $"User ID={user};"
                + $"Password={password};";

        public IDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter(this);
        }

        public IEntityAdapter CreateEntityAdapter<T>() where T : new()
        {
            return new SqlEntityAdapter<T>(this);
        }

        public ITableAdapter CreateTableAdapter<T>() where T : new()
        {
            return new SqlTableAdapter<T>(this);
        }
    }

}
