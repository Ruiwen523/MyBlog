using MyBlog.Models.Auth;
using System.Collections.Generic;
using static MyBlog.Common.Enums.BlogEnum;

namespace MyBlog.Services.Interface
{
    public interface ISystemService
    {
        public List<User> GetUsers(string Account = "");

        public (LoginUserDTO, StateCode) GetLoginUser();

        public User AddUser(User user);

    }
}
