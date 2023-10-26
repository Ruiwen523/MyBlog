using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models.Auth;
using MyBlog.Models.Common;
using MyBlog.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static MyBlog.Common.Enums.BlogEnum;

namespace MyBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        private protected IConfigService _configService;
        private protected BloggingContext _dbContext;
        public LoginController(IConfigService configService,
                               BloggingContext dbContext) 
        {
            _configService = configService;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseBox<Empty>>> LoginAsync(RequestBox<LoginUser> userData)
        {
            var user = _dbContext.Users.AsNoTracking()
                                       .ToListAsync().Result
                                       .SingleOrDefault(m => m.Account.Equals(userData.Body.UserId) && m.Password.Equals(userData.Body.Mima));

            if (user == null)
            {
                return Content("帳號密碼錯誤");
            }
            else 
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Account),
                    new Claim("FullName", user.Name),
                    new Claim(ClaimTypes.Role, "Administrator"),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // 允許刷新身份驗證會話(Session)。
                    AllowRefresh = true,
                    // 身份驗證票證過期的時間。 此處設定的值會覆寫使用 AddCookie 設定的 CookieAuthenticationOptions 的 ExpireTimeSpan 選項。
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // 身份驗證會話是否在多個請求中持續存在。 與 cookie 一起使用時，控制 cookie 的生命週期是絕對的（與身分驗證票證的生命週期相符）還是基於會話。
                    IsPersistent = true,
                    // 核發身分驗證票證的時間
                    IssuedUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // 用作HTTP的完整路徑重定向轉址
                    RedirectUri = "http://www.google.com.tw/"
                };

                // SignInAsync 會建立加密的 cookie ，並將它新增至目前的回應。
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
            }

            return Done<Empty>(null, StateCode.OK);
        }
    }
}
