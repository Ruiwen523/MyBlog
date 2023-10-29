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

        // ���UDI Container
        public void ConfigureServices(IServiceCollection services)
        {
            // DI �������U�s�ղ����X�R��k
            services.AddConfig(Configuration)
                    .AddBatchRegisterDIConfig();

            //services.AddMemoryCache();

            #region �w����IConfigService���UDI����
            //services.AddScoped<IDbConnection, SqlConnection>(serviceProvider =>
            //{
            //    //�����s�u�r��
            //    SqlConnection conn = new SqlConnection();
            //    conn.ConnectionString = Configuration.GetConnectionString("BloggingContext");
            //    return conn;
            //});
            #endregion

            services.AddControllers().AddNewtonsoftJson();


            #region Swagger�A��
            services.AddSwaggerGen(c =>
            {
                //c.IgnoreObsoleteProperties();
                // �v�TSwaggerUI���Y��ܤ��e
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

                // �i����api�˪O
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "API Document", Version = "v2" });
            });

            services.AddSwaggerGenNewtonsoftSupport();



            #endregion

            #region �ϥ� cookie ���� Identity
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(option =>
                    {
                        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                        option.LoginPath = new PathString("/api/Login/NoLogin"); // ���n�J�ɾɭ�
                    });

            services.AddMvc(options =>
            {
                // �K�[����Filter����
                options.Filters.Add(new AuthorizeFilter());
            });
            #endregion


            services.AddDbContext<BloggingContext>(
                options => options.UseSqlServer("name=ConnectionStrings:BloggingContext"));
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
