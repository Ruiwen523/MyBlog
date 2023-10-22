using Dapper;
using Microsoft.Data.SqlClient;
using MyBlog.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyBlog.Services
{
    public class ServicesBase : IServicesBase
    {
        private string _dbConnectionString;

        public ServicesBase(IDbConnection _conn)
        {
            _dbConnectionString = _conn.ConnectionString;
        }

        /// <summary>
        /// 查詢清單資料
        /// </summary>
        public List<T> Query<T>(string sql, DynamicParameters dynamic = null)
        {
            using (SqlConnection conn = new SqlConnection(_dbConnectionString))
            {
                return conn.Query<T>(sql, dynamic, null, true, 30, CommandType.Text).ToList();
            }
        }

        /// <summary>
        /// 查詢單筆資料時用
        /// </summary>
        public T QueryFirstOrDefault<T>(string sql, DynamicParameters parameters)
        {
            using (var conn = new SqlConnection(_dbConnectionString)) 
            {
                return conn.QueryFirstOrDefault<T>(sql, parameters, null, 30, CommandType.Text);
            }
        }

        /// <summary>
        /// 執行Insert、Update、Delete、Stored Procedure時用。 
        /// </summary>
        public int Execute(string sql, DynamicParameters parameters)
        {
            using (var conn = new SqlConnection(_dbConnectionString))
            {
                return conn.Execute(sql, parameters);
            }
        }
    }
}
