using MyBlog.Models;
using System.Collections.Generic;

namespace MyBlog.Services.Interface
{
    public interface IBlogService
    {
        List<Blog> GetBlogs();

        int UpdateBlog(Blog blog);

        int DeleteBlog(int id);

        Blog GetBlog(int id);
    }
}
