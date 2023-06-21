using MiniOrm.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Odbc;
using System.IO;
using System.Linq;

namespace MiniOrm.MsAccessNf
{
    public class MsaObjectFactory :
        IObjectFactory
    {
        private string StrCnn { get; set; }

        public MsaObjectFactory(
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

        public MsaObjectFactory(string strCnn)
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

        public static List<string> GetSystemDriverList()
        {
            var nombres = new List<string>();

            var reg = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software");

            if (reg == null)
            {
                return nombres;
            }
            reg = reg.OpenSubKey("ODBC");

            if (reg == null)
            {
                return nombres;
            }
            reg = reg.OpenSubKey("ODBCINST.INI");

            if (reg == null)
            {
                return nombres;
            }

            reg = reg.OpenSubKey("ODBC Drivers");

            if (reg == null)
            {
                return nombres;
            }

            foreach (string sName in reg.GetValueNames())
            {
                nombres.Add(sName);
            }

            try
            {
                reg?.Close();
            }
            catch { /* ignore this exception if we couldn't close */ }

            return nombres;
        }

        private static string GetAccessDriverName()
        {
            var dn = GetSystemDriverList().FirstOrDefault(d => d.StartsWith("Microsoft Access Driver ("));
            if (string.IsNullOrEmpty(dn))
            {
                throw new Exception("No se encontro el driver ODBC para Ms Access");
            }
            return dn;
        }

        private string GetStrConexion(
            string pathBd
            , string password            
        ) =>
            "Provider=MSDataShape" +
            ";data provider=MSDASQL" +
            $";DRIVER={{{GetAccessDriverName()}}}" +
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
            return new MsaDataAdapter(this);
        }

        public IEntityAdapter CreateEntityAdapter<T>() where T : new()
        {
            return new MsaEntityAdapter<T>(this);
        }

        public ITableAdapter CreateTableAdapter<T>() where T : new()
        {
            return new MsaTableAdapter<T>(this);
        }
    }

}
