using MyBlog.Models;
using MyBlog.Services.Interface;

namespace MyBlog.Services
{
    public class PayMoney1Service : IPay
    {
        public string Type => "I Am Money One.";

        public PayMoney Pay()
        {
            var obj = new PayMoney() 
            {
                Cost = 123,
                PayOwner = "One"
            };            

            return obj;
        }
    }
}
