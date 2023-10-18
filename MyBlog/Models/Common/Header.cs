using static MyBlog.Common.Enums.BlogEnum;

namespace MyBlog.Models.Common
{
    public class Header
    {
        public string Message { get; set; }
        public StateCode StateCode { get; set; }
    }
}
