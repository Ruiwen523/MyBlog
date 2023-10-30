using System;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.Auth
{
    public class User
    {
        [Key]
        public string Account { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Creator { get; set; }

        public DateTime CreateDate { get; set; }

        public string LastModifior { get; set; } = string.Empty;

        public DateTime? LastModifyDate { get; set; } = null;
    }
}
