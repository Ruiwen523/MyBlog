using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyBlog.Data;
using MyBlog.Models;
using MyBlog.Services;

namespace MyBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly BloggingContext _context;
        private readonly IDbConnection _conn;

        public BlogsController(BloggingContext context, IDbConnection dbConnection)
        {
            _context = context;
            _conn = dbConnection;
        }

        // GET: api/Blogs
        [HttpGet]
        public ActionResult<List<Blog>> GetBlogs()
        {
            var service = new BlogService(_conn.ConnectionString);
            var blogs = service.GetBlogs();

            return blogs; //await _context.Blogs.AsNoTracking().ToListAsync();
        }

        // GET: api/Blogs/5
        [HttpGet("{id}", Name = nameof(GetBlog))]
        public ActionResult<Blog> GetBlog(int id)
        {
            var service = new BlogService(_conn.ConnectionString);
            var blog = service.GetBlog(id);
            //var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
            {
                return NotFound();
            }

            return blog;
        }

        // PUT: api/Blogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlog(int id, Blog blog)
        {
            var service = new BlogService(_conn.ConnectionString);
            var resCount = service.UpdateBlog(blog);


            if (id != blog.BlogId)
            {
                return BadRequest();
            }

            _context.Entry(blog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Blogs
        [HttpPost(Name =nameof(PostBlog))]
        public async Task<ActionResult<Blog>> PostBlog(Blog blog)
        {
            var service = new BlogService(_conn.ConnectionString);
            var resCount = service.InsertBlog(blog);

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            return CreatedAtRoute(nameof(GetBlog), new { id = blog.BlogId }, blog);
        }

        // DELETE: api/Blogs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Blog>> DeleteBlog(int id)
        {
            var service = new BlogService(_conn.ConnectionString);
            var resCount = service.DeleteBlog(id);


            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return blog;
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.BlogId == id);
        }
    }
}
