using System.ComponentModel;

namespace MyBlog.Common.Enums
{
    public class BlogEnum
    {
        public enum StateCode 
        {
            [Description("成功拿到資料拉")]
            OK = 200,

            [Description("發送失敗拉")]
            Fail = 404
        }

        public enum DbSource 
        {
            SQLServer,
            DB2
        }
    }
}
