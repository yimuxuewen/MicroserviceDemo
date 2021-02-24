using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Cache.CacheManager;
using Ocelot.Cache;
using Ocelot.Provider.Polly;
using IdentityServer4.AccessTokenValidation;
using Microservice.Common;

namespace MicroService.GatewayDemo
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
            //string authenticationProviderKey = "UserGatewayKey";
            //services.AddAuthentication("Bearer")
            //    .AddIdentityServerAuthentication(authenticationProviderKey, options =>
            //    {
            //        options.Authority = "http://localhost:9001";
            //        options.ApiName = "UserApi";
            //        options.RequireHttpsMetadata = false;
            //        options.SupportedTokens = SupportedTokens.Both;
            //    });

            services.AddControllers();
            services.AddOcelot()
                //.AddCacheManager(x =>
                //{
                //    x.WithDictionaryHandle();//×Öµä»º´æ
                //})
                .AddConsul()
                .AddPolly();
            //services.AddSingleton<IOcelotCache<CachedResponse>, CustomCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOcelot();

            //Ïòconsul×¢²á·þÎñ
            if (!Configuration.ConsulRegist())
            {
                app.UseExceptionHandler("/Health/Error");
            }
            //Configuration.ConsulRegist();
        }
    }
}
