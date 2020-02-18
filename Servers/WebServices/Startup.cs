using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SoapCore;
using System.ServiceModel;

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
            services.AddRouting();
            services.AddSoapCore();

            // PublicServices
            services.TryAddSingleton<PublicServices.Authentication.AuthService>();
            services.TryAddSingleton<PublicServices.Competitive.CompetitiveService>();
            services.TryAddSingleton<PublicServices.Direct2Game.Direct2GameService>();

            // Non-PublicServices
            services.TryAddSingleton<Motd.MotdService>();
            services.TryAddSingleton<Sake.StorageServer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // PublicServices
                endpoints.UseSoapEndpoint<PublicServices.Authentication.AuthService>("/AuthService/AuthService.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
                //endpoints.UseSoapEndpoint<PublicServices.Competitive.CompetitiveService>("/", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
                //endpoints.UseSoapEndpoint<PublicServices.Direct2Game.Direct2GameService>("/", new BasicHttpBinding(), SoapSerializer.XmlSerializer);

                // Non-PublicServices
                //endpoints.UseSoapEndpoint<Motd.MotdService>("/motd/motd.asp", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
                //endpoints.UseSoapEndpoint<Sake.StorageServer>("/SakeStorageServer/StorageServer.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
            });
        }
    }
}
