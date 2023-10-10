using Microsoft.EntityFrameworkCore;
using MyBlog.Models;
using System.Collections.Generic;

namespace MyBlog.Data
{
    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
