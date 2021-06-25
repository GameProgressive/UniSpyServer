using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using RetroSpyServices.Sake.Handler.Service;
using SoapCore;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

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

            services.AddSoapCore();

            //services.AddRouting();

            //Public Services (SOAP)

            services.TryAddSingleton<SakeStorageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRouting();
            //app.UseAuthorization();

            var soapEncoder = new SoapEncoderOptions();
            soapEncoder.MessageVersion = MessageVersion.Soap11; // Only Soap11 is supported by the client
            soapEncoder.WriteEncoding = new UTF8Encoding(false); // We need without BOM or the client won't parse it (it does not detect the UTF8 BOM)

            app.UseSoapEndpoint<SakeStorageService>(
                "/SakeStorageServer/StorageServer.asmx",
                soapEncoder, SoapSerializer.XmlSerializer);
        }
    }
}
