using System.ComponentModel;

namespace MyBlog.Common.Enums
{
    public class BlogEnum
    {
        public enum StateCode 
        {
            [Description("成功")]
            OK,

            [Description("失敗")]
            Fail,

            [Description("登入成功")]
            Login,

            [Description("登出成功")]
            Logout,

            [Description("操作無權限")]
            NoAccess,
        }

        public enum DbSource 
        {
            SQLServer,
            DB2
        }
    }
}
