using static MyBlog.Common.Enums.BlogEnum;

namespace MyBlog.Models.Common
{
    public class ResponseHeader
    {
        public string Message { get; set; }
        public string StateCode { get; set; }
    }
}
