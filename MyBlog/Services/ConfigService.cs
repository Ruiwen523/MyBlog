using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MyBlog.Models.Auth;
using MyBlog.Models.Common;
using MyBlog.Services.Interface;

namespace MyBlog.Services
{
    public class ConfigService : IConfigService
    {
        private readonly IConfiguration _configuration;
        private readonly AppSettings _AppOptions;
        private readonly Security _SecurityOptions;

        public ConfigService(IConfiguration configuration,
                             IOptions<AppSettings> appSettingsOptions,
                             IOptions<Security> securityOptions)
        {
            _configuration = configuration;
            _AppOptions = appSettingsOptions.Value;
            _SecurityOptions = securityOptions.Value;
        }

        public string SqlServerBlog { get => _configuration.GetConnectionString("BloggingContext"); }

        public string DB2Blog { get => _configuration.GetConnectionString("DB2"); }

        public AppSettings appSettings { get => _AppOptions; }

        public Security Security { get => _SecurityOptions; }

        //_configuration.GetSection("Security:JWT:KEY").Value;
    }
}
