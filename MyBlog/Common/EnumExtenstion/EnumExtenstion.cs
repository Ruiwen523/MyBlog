using System;
using System.ComponentModel;
using System.Linq;

namespace MyBlog.Common.EnumExtenstion
{
    /// <summary>
    /// 列舉的擴充方法
    /// </summary>
    public static class EnumExtenstion
    {
        /// <summary>
        /// 回傳 Enum 的 Description 屬性，如果沒有 Description 屬性就回傳列舉成員名稱
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
