using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyBlog.Models.Common;
using MyBlog.Services;
using MyBlog.Services.Interface;


namespace MyBlog.Extensions
{
    public static class RegisterDIConfig
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettingsOptions>(config.GetSection("AppSettings"));

            return services;
        }

        public static IServiceCollection AddRegisterDIConfig(this IServiceCollection services) 
        {
            // 研究相依問題中
            //services.AddScoped<IServicesBase, ServicesBase>();
            //services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IPay, PayMoney1Service>();
            services.AddScoped<IPay, PayMoney2Service>();

            // 嘗試註冊至DI Container如未存在則Create，反之忽略。
            // services.TryAddSingleton<IBlogService, BlogService>();

            return services;
        }
    }
}
