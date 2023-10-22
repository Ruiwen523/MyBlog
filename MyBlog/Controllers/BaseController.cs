using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models.Common;
using static MyBlog.Common.Enums.BlogEnum;

namespace MyBlog.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        [NonAction]
        public ResponseBox<T> Done<T>(T body, StateCode code = StateCode.OK) 
        {
            return new ResponseBox<T>(body, code);
        }

        [NonAction]
        public ResponseBox<Empty> Done(Empty body = null, StateCode code = StateCode.OK)
        {
            return new ResponseBox<Empty>(body, code);
        }
    }
}
