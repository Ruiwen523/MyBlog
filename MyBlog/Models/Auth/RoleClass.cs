using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.Auth
{
    public class RoleClass
    {
        [Key]
        public string RoleCode { get; set; }

        public string RoleName { get; set; }
    }
}
