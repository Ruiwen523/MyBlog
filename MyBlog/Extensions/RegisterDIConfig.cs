using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyBlog.Models.Common;
using MyBlog.Services;
using MyBlog.Services.Interface;
using System;
using System.Linq;
using System.Reflection;

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
            /* 這裡要注意 當商業邏輯相依於服務底層時，生命週期長的不可相依短的，
             * 儘管真的發生實際在執行時，短的也會變為跟長的相同的生命週期。*/
            services.AddScoped<IServicesBase, ServicesBase>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IPay, PayMoney1Service>();
            services.AddScoped<IPay, PayMoney2Service>();

            // 嘗試註冊至DI Container如未存在則Create，反之忽略。
            // services.TryAddSingleton<IBlogService, BlogService>();

            return services;
        }
        
        public static IServiceCollection AddBatchRegisterDIConfig(this IServiceCollection services)
        {
            var Types = Assembly.GetExecutingAssembly().GetTypes().ToList();
            var Services = Types.Where(m => m.IsPublic && m.IsClass && !m.IsAbstract && m.Name.Contains("Service"));
            
            foreach (var Service in Services) 
            {
                Type IService = Service.GetInterfaces().FirstOrDefault();
                services.AddScoped(IService, Service);
            }

            return services;
        }

    }
}
