using MyBlog.Models;
using MyBlog.Services.Interface;

namespace MyBlog.Services
{
    public class PayMoney2Service : IPay
    {
        public string Type => "I Am Money Two.";

        public PayMoney Pay()
        {
            var obj = new PayMoney()
            {
                Cost = 321,
                PayOwner = "Two"
            };
            return obj;
        }
    }
}
