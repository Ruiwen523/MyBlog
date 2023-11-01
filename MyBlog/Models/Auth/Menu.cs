namespace MyBlog.Models.Auth
{
    public class Menu
    {
        public string  Module {get;set;}

        public string SubModule { get;set;}

        public bool IsDisplay { get; set; }

        public int Sort { get; set; }
    }
}