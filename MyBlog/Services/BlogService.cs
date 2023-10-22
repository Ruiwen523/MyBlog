using Dapper;
using MyBlog.Models;
using MyBlog.Models.Common;
using MyBlog.Services.Interface;
using System.Collections.Generic;
using System.Data;

namespace MyBlog.Services
{
    public class BlogService : ServicesBase, IBlogService
    {
        public BlogService(string ConnectionString) : base(ConnectionString) { }

        /// <summary>
        /// 取得資料表清單
        /// </summary>
        public List<Blog> GetBlogs()
        {
            var sql = @"select * from Blogs";
            
            return Query<Blog>(sql);
        }

        /// <summary>
        /// 取得指定單筆資料
        /// </summary>
        public Blog GetBlog(int BlogId)
        {
            var sql = @"select * from Blogs where BlogId = @BlogId";
            var parameters = new DynamicParameters();
            parameters.Add("BlogId", BlogId, DbType.Int32);

            return QueryFirstOrDefault<Blog>(sql, parameters);
        }

        /// <summary>
        /// 新增單筆資料
        /// </summary>
        public int InsertBlog(Blog blog)
        {
            var sql = @"insert into
                            Blogs (Url, Rating)
                        values
                            (@url, @rating)";

            var parameters = new DynamicParameters();
            parameters.Add("Url", blog.Url, DbType.String);
            parameters.Add("Rating", blog.Rating, DbType.Int32);

            return Execute(sql, parameters);
        }

        /// <summary>
        /// 更新單筆資料
        /// </summary>        
        public int UpdateBlog(Blog blog)
        {
            var sql = @"update Blogs
                        set
                            Url = @url,
                            Rating = @rating
                        where
                            BlogId = @BlogId";

            var parameters = new DynamicParameters();
            parameters.Add("BlogId", blog.BlogId, DbType.Int32);
            parameters.Add("Url", blog.Url, DbType.String);
            parameters.Add("Rating", blog.Rating, DbType.Int32);

            return Execute(sql, parameters);
        }

        /// <summary>
        /// 刪除單筆資料
        /// </summary>
        public int DeleteBlog(int index) 
        {
            var sql = @"Delete Blogs
                        where
                            BlogId = @BlogId";

            var parameters = new DynamicParameters();
            parameters.Add("BlogId", index, DbType.Int32);

            return Execute(sql, parameters);
        }
    }
}
