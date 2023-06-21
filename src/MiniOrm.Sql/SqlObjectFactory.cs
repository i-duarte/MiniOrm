using MiniOrm.Common;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace MiniOrm.Sql
{
    public class SqlObjectFactory :
        IObjectFactory
    {

        private string DataSource { get; set; }
        private string DbName { get; set; }
        private string User { get; set; }
        private string Password { get; set; }

        public SqlObjectFactory(
            string dataSource
            , string dataBase
            , string user
            , string password
        )
        {
            DataSource = dataSource;
            DbName = dataBase;
            User = user;
            Password = password;
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
                var arr = strCnn.Split('|');
                DataSource = arr[0];
                DbName = arr[1];
                switch (arr.Length)
                {
                    case 4:
                        User = arr[2];
                        Password = arr[3];
                        break;
                    default:
                        throw new Exception("Formato incorrecto de pipeCnn");
                }
            }
            else
            {
                throw new Exception("Error en el formato de la cadena de conexion, no se encontraron pipes");
            }
        }

        public DbConnection CreateConnection(int timeOut = 30)
        {
            var cnn =
                new SqlConnection(
                    GetStrConexion(
                        DataSource
                        , DbName
                        , User
                        , Password
                        , timeOut
                    )
                );
            cnn.Open();
            return cnn;
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
