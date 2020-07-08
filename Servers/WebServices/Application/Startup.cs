using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using RetroSpyServices.Authentication.Service;
using RetroSpyServices.Competitive.Service;
using RetroSpyServices.Direct2Game.Service;
using RetroSpyServices.Motd.Service;
using RetroSpyServices.Sake.Service;
using System.ServiceModel;
using SOAPMiddleware.MiddlewareComponent;
using Serilog;

namespace WebServices
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
            //use serilog as our logger
            //services.AddSingleton(Log.Logger);

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddRouting();

            //PublicServices
            //services.TryAddSingleton<AuthService>();
            //services.TryAddSingleton<CompetitiveService>();
            //services.TryAddSingleton<Direct2GameService>();
            //services.TryAddSingleton<MotdService>();
            services.TryAddSingleton<SakeStorageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRouting();
            
            /*app.UseEndpoints(endpoints =>
            {
                // PublicServices
                /*endpoints.UseSoapEndpoint<AuthService>(
                    "/AuthService/AuthService.asmx",
                    new BasicHttpBinding(), SoapSerializer.XmlSerializer);*/
            //endpoints.UseSoapEndpoint<PublicServices.Competitive.CompetitiveService>("/", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
            //endpoints.UseSoapEndpoint<PublicServices.Direct2Game.Direct2GameService>("/", new BasicHttpBinding(), SoapSerializer.XmlSerializer);

            // Non-PublicServices
            //endpoints.UseSoapEndpoint<Motd.MotdService>("/motd/motd.asp", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
            //});

            app.UseSOAPEndpoint<SakeStorageService>(
                "/SakeStorageServer/StorageServer.asmx",
                new BasicHttpBinding());
        }
    }
}
