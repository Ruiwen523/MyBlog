using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/[controller]/[action]")]
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

        [AllowAnonymous]
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
                    //new Claim(ClaimTypes.Role, "Admin"),
                    //new Claim("LastChanged", {Database Value})
                };

                // 若今日登入帳號具有多角色權限則，則撈取後同時添加進來
                var role = from r in _dbContext.roleRlAccounts
                           where r.Account == user.Account
                           select r.RoleCode;

                foreach (var r in role) 
                {
                    claims.Add(new Claim(ClaimTypes.Role, r));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // 要實測一下該屬性是否為，瀏覽器全部關閉，而只關閉索引頁籤也不會執行瀏覽器關閉事件(登出)。
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)

                    // 允許刷新身份驗證會話(Session)。
                    //AllowRefresh = true,
                    // 身份驗證票證過期的時間。 此處設定的值會覆寫使用 AddCookie 設定的 CookieAuthenticationOptions 的 ExpireTimeSpan 選項。
                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                    // 身份驗證會話是否在多個請求中持續存在。 與 cookie 一起使用時，控制 cookie 的生命週期是絕對的（與身分驗證票證的生命週期相符）還是基於會話。
                    //IsPersistent = true,
                    // 核發身分驗證票證的時間
                    //IssuedUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // 用作HTTP的完整路徑重定向轉址
                    //RedirectUri = "http://www.google.com.tw/"
                };

                // SignInAsync 會建立加密的 cookie ，並將它新增至目前的回應。
                //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            }

            return Done<Empty>(null, StateCode.Login);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseBox<Empty>>> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Done<Empty>(null, StateCode.Logout);
        }


        [HttpGet]
        [AllowAnonymous]
        public string NoLogin()
        {
            return "未登入";
        }

        [HttpGet]
        public string NeedLogin()
        {
            return "登入成功";
        }

        [HttpGet]
        public ActionResult<ResponseBox<Empty>> NoAccess()
        {
            return Done<Empty>(null, StateCode.NoAccess);
        }

        [HttpGet]
        [Authorize(Roles = "Access")]
        public string NeedAccess() 
        {
            return "有登入且有授權";
        }

        [HttpGet]
        [Authorize(Roles = "Access2")]
        public string NeedAccess2()
        {
            return "有登入且有授權";
        }

    }
}
