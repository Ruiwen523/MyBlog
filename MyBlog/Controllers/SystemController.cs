using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBlog.Data;
using MyBlog.Models.Auth;
using MyBlog.Models.Common;
using MyBlog.Services.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MyBlog.Common.Enums.BlogEnum;

namespace MyBlog.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SystemController : BaseController
    {
        private protected BloggingContext _context;
        private protected ILogger _logger;
        private protected ISystemService _systemService;

        public SystemController(BloggingContext bloggingContext, ILogger<SystemController> logger, ISystemService systemService) 
        {
            _context = bloggingContext;
            _logger = logger;
            _systemService = systemService;
        }

        /// <summary>
        /// 新增使用者
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<ResponseBox<User>> CreateUser(User user)
        {
            var model = _systemService.AddUser(user);

            if (model != null)
                _logger.LogInformation(string.Format("寫入User成功 名稱: {0}", user.Name));

            return Done(model, StateCode.OK);
        }

        /// <summary>
        /// 新增角色類別
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<ResponseBox<Empty>> CreateRoleClass(List<RoleClass> roleClasse) 
        {
            _context.RoleClass.AddRange(roleClasse);
            var count = _context.SaveChangesAsync().Result;

            if (count > 0)
                _logger.LogInformation(string.Format("寫入RoleClass成功 名稱: {0}", string.Join('、', roleClasse.Select(m => m.RoleName))));

            return Done(null, StateCode.OK);
        }

        /// <summary>
        /// 新增使用者帳號與角色類別關聯
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<ResponseBox<Empty>> AddRoleRlAccount(List<RoleRlAccount> roleRlAccounts)
        {
            _context.roleRlAccounts.AddRange(roleRlAccounts);
            var count = _context.SaveChangesAsync().Result;

            if (count > 0)
                _logger.LogInformation(string.Format("寫入RoleRlAccount成功 名稱: {0}", string.Join('、', roleRlAccounts)));

            return Done(null, StateCode.OK);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<ResponseBox<List<User>>> GetUsers(string Account = "") 
        {
            var users = _systemService.GetUsers(Account);


            if (string.IsNullOrEmpty(Account)) 
            {
                //users = users.Where(m=> HttpContext.User.Claims.SingleOrDefault(u => u.Type.Equals(m.Account)))
            }

            //where user.Account == HttpContext.User.Claims.

            return Done(users, StateCode.OK);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult<ResponseBox<LoginUserDTO>> GetLoginUser() 
        {
            var user = _systemService.GetLoginUser();

            return Done(user.Item1, user.Item2);
        }
    }
}
