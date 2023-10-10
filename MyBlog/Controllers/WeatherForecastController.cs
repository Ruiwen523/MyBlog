using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBlog.Data;
using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")] // 宣告控制器的動作支援回應內容類型為 application/json
    public class WeatherForecastController : ControllerBase
    {
        private readonly BloggingContext myBlogContext;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, BloggingContext _myBlogContext)
        {
            _logger = logger;
            myBlogContext = _myBlogContext;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> GetContextData()
        //{
        //    var rng = new Random();
        //    return myBlogContext
        //}
    }
}
