using MyBlog.Models.Auth;
using System.Collections.Generic;

namespace MyBlog.Services.Interface
{
    public interface ISystemService
    {
        public List<User> GetUsers(string Account = "");

        public User GetLoginUser();

        public User AddUser(User user);

    }
}
