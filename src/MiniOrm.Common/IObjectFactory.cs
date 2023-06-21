﻿using System.Data.Common;

namespace MiniOrm.Common
{
    public interface IObjectFactory
    {
        DbConnection CreateConnection(int timeOut = 30);
        IDataAdapter CreateDataAdapter();
        IEntityAdapter CreateEntityAdapter<T>() where T : new();
        ITableAdapter CreateTableAdapter<T>() where T : new();
    }    
}
