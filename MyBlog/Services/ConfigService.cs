using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MyBlog.Models.Common;
using MyBlog.Services.Interface;

namespace MyBlog.Services
{
    public class ConfigService : IConfigService
    {
        private readonly IConfiguration _configuration;
        private readonly AppSettings _options;

        public ConfigService(IConfiguration configuration,
                             IOptions<AppSettings> options)
        {
            _configuration = configuration;
            _options = options.Value;
        }

        public string SqlServerBlog { get => _configuration.GetConnectionString("BloggingContext"); }

        public string DB2Blog { get => _configuration.GetConnectionString("DB2"); }

        //public AppSettings appSettings { get => _configuration.GetValue<AppSettings>("AppSettings"); }

        public AppSettings appSettings { get => _options; }
    }
}
