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

        // ���UDI Container
        public void ConfigureServices(IServiceCollection services)
        {
            // DI �������U�s�ղ����X�R��k
            services.AddConfig(Configuration)
                    .AddBatchRegisterDIConfig();

            services.AddDbContext<BloggingContext>(
                options => options.UseSqlServer("name=ConnectionStrings:BloggingContext"));

            //services.AddMemoryCache();

            // ���ӤɤWNET5�H�W�n�N AddNewtonsoftJson() �令 AddJsonOptions()  https://medium.com/@mvpdw06/net-core-3-1-%E5%BE%8C%E8%BD%89%E7%A7%BB-newtonsoft-json-%E8%87%B3-system-text-json-9727d774f92d
            services.AddControllers().AddNewtonsoftJson();

            // �c�� Swagger �M��
            services.AddSwaggerAPI();

            // �c�� HttpContext DI ��Controller�h�H�~�]��z�LDI�غc�X��
            services.AddHttpContextAccessor();

            // �c�� JWT ���ҳ]�w
            services.AddJwtAuthorize(Configuration);

            // �K�[����Filter����
            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            });   
        }

        // �ϥ�Middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region �ϥ�Swagger&UI
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

            // �ϥΨ�������
            app.UseAuthentication();
            // �ϥΨ������v
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapSwagger();
            });
        }
    }
}
