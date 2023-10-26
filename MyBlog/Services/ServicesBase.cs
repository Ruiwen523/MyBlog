using Dapper;
using Microsoft.Data.SqlClient;
using MyBlog.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using static MyBlog.Common.Enums.BlogEnum;

namespace MyBlog.Services
{
    public class ServicesBase : IServicesBase
    {
        //private string _dbConnectionString;
        private protected IConfigService _config;

        public ServicesBase(//IDbConnection _conn,
                            IConfigService configService)
        {
            //_dbConnectionString = _conn.ConnectionString;
            _config = configService;
        }

        IDbConnection ChoseDbSorce(DbSource dbSource) => 
        dbSource switch
        {
            DbSource.SQLServer => new SqlConnection(_config.SqlServerBlog),
            _ => new OleDbConnection(_config.DB2Blog)
        
        };

        /// <summary>
        /// 查詢清單資料
        /// </summary>
        public List<T> Query<T>(string sql, DynamicParameters dynamic = null, DbSource dbSource = DbSource.SQLServer)
        {
            using (IDbConnection conn = ChoseDbSorce(dbSource))
            {
                return conn.Query<T>(sql, dynamic, null, true, 30, CommandType.Text).ToList();
            }
        }

        /// <summary>
        /// 查詢單筆資料時用
        /// </summary>
        public T QueryFirstOrDefault<T>(string sql, DynamicParameters parameters, DbSource dbSource = DbSource.SQLServer)
        {
            using (IDbConnection conn = ChoseDbSorce(dbSource))
            {
                return conn.QueryFirstOrDefault<T>(sql, parameters, null, 30, CommandType.Text);
            }
        }

        /// <summary>
        /// 執行Insert、Update、Delete、Stored Procedure時用。 
        /// </summary>
        public int Execute(string sql, DynamicParameters parameters, DbSource dbSource = DbSource.SQLServer)
        {
            using (IDbConnection conn = ChoseDbSorce(dbSource))
            {
                return conn.Execute(sql, parameters);
            }
        }
    }
}
