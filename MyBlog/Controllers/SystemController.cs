using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBlog.Data;
using MyBlog.Models.Auth;
using MyBlog.Models.Common;
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
        private readonly ILogger _logger;

        public SystemController(BloggingContext bloggingContext, ILogger<SystemController> logger) 
        {
            _context = bloggingContext;
            _logger = logger;
        }

        /// <summary>
        /// 新增使用者
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<ResponseBox<Empty>> CreateUser(User user)
        {
            _context.Users.Add(user);
            var count = _context.SaveChangesAsync().Result;

            if (count > 0)
                _logger.LogInformation(string.Format("寫入User成功 名稱: {0}", user.Name));

            return Done(null, StateCode.OK);
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
    }
}
