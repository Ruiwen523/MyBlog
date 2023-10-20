using MyBlog.Models;
using System;

namespace MyBlog.Services.Interface
{
    public interface IPay
    {
        string Type { get; }
        PayMoney Pay();
    }
}
