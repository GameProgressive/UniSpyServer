using System.ServiceModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Models;
using SoapCore;

namespace RetroSpyWeb
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
            services.TryAddSingleton<SampleService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.UseSoapEndpoint<SampleService>("/Service.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
            });
            //app.UseSoapEndpoint<SampleService>("/Service.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);

        }
    }
}
