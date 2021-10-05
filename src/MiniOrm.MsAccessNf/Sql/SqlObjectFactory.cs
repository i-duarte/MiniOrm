using MiniOrm.Common;
using System;
using System.Data.Common;
using System.Data.Odbc;
using System.IO;

namespace MiniOrm.Sql
{
    public class SqlObjectFactory :
        IObjectFactory
    {
        private string StrCnn { get; set; }

        public SqlObjectFactory(
            string pathBd
            , string password
        )
        {
            StrCnn = 
                GetStrConexion(
                    pathBd
                    , password
                );
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
                new OdbcConnection(StrCnn);
            cnn.Open();
            return cnn;
        }

        private string GetStrCnnFromPipeStr(string pipeCnn)
        {
            var arr = pipeCnn.Split('|');
            switch (arr.Length)
            {
                case 2:
                    return GetStrConexion(arr[0], arr[1]);
                default:
                    throw new Exception("Formato incorrecto de pipeCnn");
            }
        }

        private string GetStrConexion(
            string pathBd
            , string password            
        ) =>
            "Provider=MSDataShape" +
            ";data provider=MSDASQL" +
            ";DRIVER=Microsoft Access Driver (*.mdb)" +
            ";UID=admin" +
            $";pwd={password}" +
            ";UserCommitSync=Yes" +
            ";Threads=3" +
            ";SafeTransactions=0" +
            ";PageTimeout=5" +
            ";MaxScanRows=8" +
            ";MaxBufferSize=2048" +
            ";FIL=MS Access" +
            ";DriverId=25" +
            $";DefaultDir={Path.GetDirectoryName(pathBd)}" +
            $";DBQ={pathBd}" +
            ";";

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
