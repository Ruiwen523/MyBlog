using Dapper;
using System.Collections.Generic;
using static MyBlog.Common.Enums.BlogEnum;

namespace MyBlog.Services.Interface
{
    public interface IServicesBase
    {
        public List<T> Query<T>(string sql, DynamicParameters dynamic = null, DbSource dbSource = DbSource.SQLServer);

        public T QueryFirstOrDefault<T>(string sql, DynamicParameters parameters, DbSource dbSource = DbSource.SQLServer);

        public int Execute(string sql, DynamicParameters parameters, DbSource dbSource = DbSource.SQLServer);
    }
}
