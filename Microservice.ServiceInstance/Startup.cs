using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microservice.Common;
using Microservice.Interface;
using Microservice.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microservice.ServiceInstance
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //foreach (var item in Directory.GetFiles(Directory.GetCurrentDirectory()))
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            //if (File.Exists(Path.Combine(Directory.GetCurrentDirectory() , "appsettings.json")))
            //{
            //    Console.WriteLine("appsettings.json存在");
            //}
            //else
            //{
            //    Console.WriteLine("appsettings.json不存在");

            //}
            //Console.WriteLine(env.ContentRootPath);
            //Console.WriteLine(env.EnvironmentName);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //向consul注册服务
            Configuration.ConsulRegist();
        }
    }
}
