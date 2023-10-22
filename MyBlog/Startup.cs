using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using System.Data;
using Microsoft.Data.SqlClient;
using MyBlog.Services.Interface;
using MyBlog.Services;
using MyBlog.Extensions;

namespace MyBlog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // DI Container
        public void ConfigureServices(IServiceCollection services)
        {
            // DI 相關註冊群組移至擴充方法
            services.AddConfig(Configuration)
                    .AddRegisterDIConfig();

            //services.AddMemoryCache();

            services.AddScoped<IDbConnection, SqlConnection>(serviceProvider =>
            {
                //指派連線字串
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = Configuration.GetConnectionString("BloggingContext");
                return conn;
            });


            services.AddControllers();

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

            //services.AddSwaggerGenNewtonsoftSupport(); // Swashbuckle.AspNetCore.Newtonsoft

            services.AddDbContext<BloggingContext>(
                options => options.UseSqlServer("name=ConnectionStrings:BloggingContext"));
        }

        // Middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapSwagger();
            });
        }
    }
}
