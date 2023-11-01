using System.Collections.Generic;

namespace MyBlog.Models.Auth
{
    public class LoginUserDTO
    {
        public string Account { get; set; }

        public string Name { get; set; }

        public List<string> Role { get; set; }

        public List<Menu> Menus { get; set; }
    }
}
