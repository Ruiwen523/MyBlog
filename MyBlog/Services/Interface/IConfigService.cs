using MyBlog.Models.Common;

namespace MyBlog.Services.Interface
{
    public interface IConfigService
    {
        public string SqlServerBlog { get; }

        public string DB2Blog { get; }

        public AppSettings appSettings { get; }
    }
}
