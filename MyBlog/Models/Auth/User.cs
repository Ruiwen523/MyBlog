using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.Auth
{
    public class User
    {
        [Key]
        public string Account { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }

        public string Password { get; set; }
    }
}
