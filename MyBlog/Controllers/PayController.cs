using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;
using MyBlog.Models.Common;
using MyBlog.Services.Interface;
using System.Collections.Generic;
using System.Linq;

namespace MyBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayController : BaseController
    {
        protected IEnumerable<IPay> _pay { get; set; }

        public PayController(IEnumerable<IPay> pay)
        {
            _pay = pay;
        }

        // POST: api/PayMoney
        [HttpPost]
        public ActionResult<ResponseBox<PayMoney>> PayMoney(string PayMode)
        {
            var PostNeedCost = new PayMoney();
            if (PayMode == "I Am Money One.")
            {
                PostNeedCost = _pay.Where(m => m.Type.Equals(PayMode)).Single().Pay();
            }
            else
            {
                PostNeedCost = _pay.Where(m => m.Type.Equals("I Am Money Two.")).Single().Pay();
            }

            return Done(PostNeedCost);
        }
    }
}
