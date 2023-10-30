using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models.Auth;
using MyBlog.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MyBlog.Services
{
    public class SystemService : ISystemService
    {
        private readonly BloggingContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SystemService(BloggingContext context,
                             IHttpContextAccessor httpContextAccessor) 
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public User AddUser(User user)
        {
            user.Creator = _httpContextAccessor.HttpContext.User.Claims.ToList().SingleOrDefault(m => m.Type.Equals(ClaimTypes.Name))?.Value;
            user.CreateDate = DateTime.Now;
            user.LastModifyDate = DateTime.Now;
            user.LastModifior = "";

            _context.Users.Add(user);
            _context.SaveChanges();

            return _context.Users.SingleOrDefault(m => m.Account.Equals(user.Account));
        }

        public User GetLoginUser()
        {
            //_context.Users.AsNoTracking()
            var Account = _httpContextAccessor.HttpContext.User.Claims.ToList().SingleOrDefault(u => u.Type == ClaimTypes.Name)?.Value;

            var user = _context.Users.SingleOrDefault(m => m.Account.Equals(Account));
            var name = _httpContextAccessor.HttpContext.User.Claims.ToList().SingleOrDefault(u => u.Type == "FullName")?.Value;

            return user;
        }

        public List<User> GetUsers(string Account = "")
        {
            var users = _context.Users.ToList();

            if (!string.IsNullOrEmpty(Account))
                users = users.Where(u => u.Account == Account).ToList();

            return users;
        }
    }
}
