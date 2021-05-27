using System;
using System.Data.Common;
using System.Reflection;

namespace MiniOrm.Common
{
    public interface IDataAdapter
    {
        object ConvertTo(
            object v
            , PropertyInfo pi
        );

        DbDataReader GetDataReader(
            string sql
            , ListParameter parameters = null
            , DbConnection cnn = null
        );

        int Execute(
            string sql
            , ListParameter parameters = null
            , DbConnection cnn = null
        );

    }
}
