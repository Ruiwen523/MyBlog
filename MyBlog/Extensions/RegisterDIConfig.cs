using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyBlog.Models.Auth;
using MyBlog.Models.Common;
using MyBlog.Services;
using MyBlog.Services.Interface;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyBlog.Extensions
{
    public static class RegisterDIConfig
    {
        /// <summary>
        /// 註冊配置檔至 IOptions 選項模式中
        /// </summary>
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config.GetSection("AppSettings"));
            services.Configure<Security>(config.GetSection("Security"));

            return services;
        }

        /// <summary>
        /// 這是一般註冊DI方法
        /// </summary>
        public static IServiceCollection AddRegisterDIConfig(this IServiceCollection services) 
        {
            /* 這裡要注意 當商業邏輯相依於服務底層時，生命週期長的不可相依短的，
             * 儘管真的發生實際在執行時，短的也會變為跟長的相同的生命週期。*/
            services.AddScoped<IServicesBase, ServicesBase>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IPay, PayMoney1Service>();
            services.AddScoped<IPay, PayMoney2Service>();

            #region 已移至IConfigService註冊DI取用
            //services.AddScoped<IDbConnection, SqlConnection>(serviceProvider =>
            //{
            //    //指派連線字串
            //    SqlConnection conn = new SqlConnection();
            //    conn.ConnectionString = Configuration.GetConnectionString("BloggingContext");
            //    return conn;
            //});
            #endregion

            // 嘗試註冊至DI Container如未存在則Create，反之忽略。
            // services.TryAddSingleton<IBlogService, BlogService>();

            return services;
        }
        
        /// <summary>
        /// 這是批次註冊方法
        /// </summary>        
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

        public static IServiceCollection AddSwaggerAPI(this IServiceCollection services) 
        {
            services.AddSwaggerGen(c =>
            {
                //c.IgnoreObsoleteProperties();
                // 影響SwaggerUI抬頭顯示內容
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API Document",
                    Version = "v1",
                    Description = "An ASP.NET Core Web API for managing My Blog",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });

                // 可切換api樣板
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "API Document", Version = "v2" });

                // 這行用意在幹嘛要查一下
                services.AddSwaggerGenNewtonsoftSupport();
            });

            return services;
        }

        /// <summary>
        /// 這是 Cookie 驗證方法
        /// </summary>
        /// <param name="services"></param>
        /// <param name="comfig"></param>
        /// <returns></returns>
        public static IServiceCollection AddCookieAuthorize(this IServiceCollection services, IConfiguration comfig) 
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(option =>
                    {
                        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                        option.LoginPath = new PathString("/api/Login/NoLogin"); // 未登入時導頁
                        option.AccessDeniedPath = new PathString("/api/Login/NoAccess"); // 登入但無權限時導頁
                    });

            return services;
        }

        /// <summary>
        /// 這是 JWT 驗證方法
        /// </summary>
        public static IServiceCollection AddJwtAuthorize(this IServiceCollection services, IConfiguration config)
        {
            var a = config["Security:JWT:KEY"];
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(option =>
                    {
                        option.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true, // 需要驗證發行者
                            ValidIssuer = config["Security:JWT:Issur"],
                            ValidateAudience = true, // 需要驗證發給誰
                            ValidAudience = config["Security:JWT:Audience"],
                            ValidateLifetime = true, // 驗證生命週期 (預設本身就是True)
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Security:JWT:KEY"])),
                            ClockSkew = TimeSpan.Zero,
                        };
                    });

            return services;
        }

    }
}
