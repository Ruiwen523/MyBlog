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
using Microsoft.Extensions.Logging;
using MyBlog.Models.Common;
using static MyBlog.Common.Enums.BlogEnum;
using MyBlog.Services.Interface;
using Microsoft.Extensions.Options;

namespace MyBlog.Controllers
{
    
    [Route("api/[controller]")]
    //[Route("api/[controller]/[action]")]
    [ApiController]
    public class BlogsController : BaseController
    {
        private readonly BloggingContext _context;
        private readonly IDbConnection _conn;
        private readonly ILogger<BlogsController> _logger;
        private readonly IBlogService _blogService;
        private readonly AppSettings _options;
        private readonly IConfigService _config;

        public BlogsController(BloggingContext context,
                               IDbConnection dbConnection,
                               ILogger<BlogsController> logger,
                               IBlogService blogService,
                               IOptions<AppSettings> options,
                               IConfigService configService)
        {
            _context = context;
            _conn = dbConnection;
            _logger = logger;
            _blogService = blogService;
            _options = options.Value;
            _config = configService;
        }
        
        //[HttpGet(Name = nameof(GetTestBlog))]
        [HttpGet("GetTestBlog")]
        //[Route("api/GetTestBlog/")]
        //[HttpPatch("GetTestBlog/")]
        public ActionResult<ResponseBox<Empty>> GetTestBlog()
        {
            return Done();
        }

        // GET: api/Blogs
        [HttpGet]
        public ActionResult<ResponseBox<List<Blog>>> GetBlogs()
        {
            // Controller 用來存放控制流程的邏輯
            // Service 用來存放商業邏輯

            _logger.LogInformation("Hello, this is the BlogList!");
            _logger.LogInformation(_options.Author);
            _logger.LogInformation(_options.WebSite);
            _logger.LogInformation(_config.DB2Blog);
            _logger.LogInformation(_config.SqlServerBlog);
            _logger.LogInformation(_config.appSettings.Author);
            _logger.LogInformation(_config.appSettings.WebSite);
            _logger.LogInformation(_options.Environment);
            _logger.LogInformation(_config.appSettings.Environment);

            var blogs = _blogService.GetBlogs();

            //return blogs; //await _context.Blogs.AsNoTracking().ToListAsync();

            return Done(blogs);
        }

        // GET: api/Blogs/5
        [HttpGet("{id}", Name = nameof(GetBlog))]
        public ActionResult<ResponseBox<Blog>> GetBlog(int id)
        {
            var blog = _blogService.GetBlog(id);
            //var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
            {
                return NotFound();
            }

            return Done(blog);
        }

        // PUT: api/Blogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlog(int id, Blog blog)
        {
            var resCount = _blogService.UpdateBlog(blog);

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
        [HttpPost(Name = nameof(PostBlog))]
        public async Task<ActionResult<Blog>> PostBlog(Blog blog)
        {
            var resCount = _blogService.InsertBlog(blog);

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            return CreatedAtRoute(nameof(GetBlog), new { id = blog.BlogId }, blog);
        }

        // DELETE: api/Blogs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Blog>> DeleteBlog(int id)
        {
            var resCount = _blogService.DeleteBlog(id);

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

