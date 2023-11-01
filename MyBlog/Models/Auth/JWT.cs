namespace MyBlog.Models.Auth
{
    public class JWT
    {
        /// <summary>
        /// 安全金鑰
        /// </summary>
        public string KEY { get; set; }

        /// <summary>
        /// 發行者
        /// </summary>
        public string Issur { get; set; }

        /// <summary>
        /// 發給誰
        /// </summary>
        public string Audience { get; set; }
    }
}
