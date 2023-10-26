using MyBlog.Common.EnumExtenstion;
using System;
using static MyBlog.Common.Enums.BlogEnum;

namespace MyBlog.Models.Common
{
    public class ResponseBox<T>
    {
        public ResponseBox(T body, StateCode code = StateCode.OK)
        {
            Header.StateCode = Enum.GetName(typeof(StateCode), (int) code);
            Header.Message = code.GetDescription();
            Body = body;
        }

        public ResponseHeader Header { get; set; } = new ResponseHeader();

        public T Body { get; set; }
    }
}
