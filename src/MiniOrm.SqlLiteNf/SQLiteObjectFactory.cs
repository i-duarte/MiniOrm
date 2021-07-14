using MiniOrm.Common;
using System;
using System.Data.Common;
using System.Data.SQLite;

namespace MiniOrm.SQLite
{
    public class SQLiteObjectFactory :
        IObjectFactory
    {
        private string StrCnn { get; set; }

        public SQLiteObjectFactory(string strCnn)
        {
            StrCnn = strCnn;
        }

        public DbConnection CreateConnection()
        {
            var cnn =
                new SQLiteConnection(
                    StrCnn.Contains("|")
                    ? GetStrCnnFromPipeStr(StrCnn)
                    : StrCnn
                );
            cnn.Open();
            return cnn;
        }

        private string GetStrCnnFromPipeStr(string pipeCnn)
        {
            var arr = pipeCnn.Split('|');
            switch (arr.Length)
            {
                case 1:
                    return GetStrConexion(arr[0], "");
                case 2:
                    return GetStrConexion(arr[0], arr[1]);
                default:
                    throw new Exception("Formato incorrecto de pipeCnn");
            }
        }

        private string GetStrConexion(
            string path
            , string password
        ) =>
            $"Data Source={path}" +
            $";Version=3" +
            (
                string.IsNullOrEmpty(password) 
                ? "" 
                : $";Password={password}"
            ) +
            $";Pooling=False;"
            ;

        public IDataAdapter CreateDataAdapter()
        {
            return new SQLiteDataAdapter(this);
        }

        public IEntityAdapter CreateEntityAdapter<T>() where T : new()
        {
            return new SQLiteEntityAdapter<T>(this);
        }

        public ITableAdapter CreateTableAdapter<T>() where T : new()
        {
            return new SQliteTableAdapter<T>(this);
        }
    }

}
