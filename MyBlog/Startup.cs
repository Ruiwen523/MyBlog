using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace MyBlog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // 註冊DI Container
        public void ConfigureServices(IServiceCollection services)
        {
            // DI 相關註冊群組移至擴充方法
            services.AddConfig(Configuration)
                    .AddBatchRegisterDIConfig();

            //services.AddMemoryCache();

            #region 已移至IConfigService註冊DI取用
            //services.AddScoped<IDbConnection, SqlConnection>(serviceProvider =>
            //{
            //    //指派連線字串
            //    SqlConnection conn = new SqlConnection();
            //    conn.ConnectionString = Configuration.GetConnectionString("BloggingContext");
            //    return conn;
            //});
            #endregion

            services.AddControllers().AddNewtonsoftJson();


            #region Swagger服務
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
            });

            services.AddSwaggerGenNewtonsoftSupport();



            #endregion

            #region 使用 cookie 驗證 Identity
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(option =>
                    {
                        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                        option.LoginPath = new PathString("/api/Login/NoLogin"); // 未登入時導頁
                    });

            services.AddMvc(options =>
            {
                // 添加全域Filter驗證
                options.Filters.Add(new AuthorizeFilter());
            });
            #endregion


            services.AddDbContext<BloggingContext>(
                options => options.UseSqlServer("name=ConnectionStrings:BloggingContext"));
        }

        // 使用Middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region 使用Swagger&UI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "My API V1");
            });


            //app.UseSwagger(c =>
            //{
            //    c.RouteTemplate = "/api/swagger/{documentName}/swagger.json";
            //});

            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("v1/swagger.json", "My API V1");
            //    c.SwaggerEndpoint("v2/swagger.json", "My API V2");
            //    c.RoutePrefix = "api/swagger";
            //});
            #endregion

            app.UseHttpsRedirection();

            app.UseRouting();

            // 使用身分驗證
            app.UseAuthentication();
            // 使用身分授權
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapSwagger();
            });
        }
    }
}
