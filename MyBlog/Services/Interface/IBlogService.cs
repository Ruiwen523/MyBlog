using MyBlog.Models;
using System.Collections.Generic;

namespace MyBlog.Services.Interface
{
    public interface IBlogService
    {
        List<Blog> GetBlogs();
        Blog GetBlog(int id);
        int UpdateBlog(Blog blog);
        int DeleteBlog(int id);
        int InsertBlog(Blog blog);
    }
}
