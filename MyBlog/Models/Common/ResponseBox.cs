using MyBlog.Common.EnumExtenstion;
using static MyBlog.Common.Enums.BlogEnum;

namespace MyBlog.Models.Common
{
    public class ResponseBox<T>
    {
        public ResponseBox(T body, StateCode code = StateCode.OK)
        {
            Header.StateCode = code;
            Header.Message = code.GetDescription();
            Body = body;
        }

        public Header Header { get; set; } = new Header();

        public T Body { get; set; }
    }
}
