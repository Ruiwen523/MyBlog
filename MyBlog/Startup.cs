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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            services.AddDbContext<BloggingContext>(
                options => options.UseSqlServer("name=ConnectionStrings:BloggingContext"));

            //services.AddMemoryCache();

            // 未來升上NET5以上要將 AddNewtonsoftJson() 改成 AddJsonOptions()  https://medium.com/@mvpdw06/net-core-3-1-%E5%BE%8C%E8%BD%89%E7%A7%BB-newtonsoft-json-%E8%87%B3-system-text-json-9727d774f92d
            services.AddControllers().AddNewtonsoftJson();

            // 構建 Swagger 套件
            services.AddSwaggerAPI();

            // 構建 HttpContext DI 使Controller層以外也能透過DI建構出來
            services.AddHttpContextAccessor();

            // 構建 JWT 驗證設定
            services.AddJwtAuthorize(Configuration);

            // 添加全域Filter驗證
            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            });   
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
