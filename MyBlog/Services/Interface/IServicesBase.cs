using Dapper;
using System.Collections.Generic;

namespace MyBlog.Services.Interface
{
    public interface IServicesBase
    {
        List<T> Query<T>(string sql, DynamicParameters dynamic = null);

        T QueryFirstOrDefault<T>(string sql, DynamicParameters parameters);

        int Execute(string sql, DynamicParameters parameters);
    }
}
