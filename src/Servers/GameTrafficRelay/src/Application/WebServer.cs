using System.Threading.Tasks;
using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Diagnostics;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using UniSpyServer.UniSpyLib.Logging;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.GameTrafficRelay.Application
{
    public class WebServer : UniSpyLib.Abstraction.Interface.IServer
    {
        public Guid ServerID { get; private set; }
        public string ServerName { get; private set; }
        public IPEndPoint ListeningIPEndPoint { get; private set; }
        public IPEndPoint PublicIPEndPoint { get; private set; }
        public WebServer(UniSpyServerConfig config)
        {
            ServerID = config.ServerID;
            ServerName = config.ServerName;
            ListeningIPEndPoint = config.ListeningIPEndPoint;
            PublicIPEndPoint = config.PublicIPEndPoint;
        }

        public void Start()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Host.ConfigureLogging((ctx, loggerConfiguration) =>
            {
                loggerConfiguration.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Warning);
            });
            //             builder.Host.UseSerilog((ctx, loggerConfiguration) =>
            //                 {
            //                     loggerConfiguration = LogWriter.LogConfig;
            //                     loggerConfiguration.Enrich.FromLogContext()
            //                     .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
            //                     .Enrich.WithProperty("Environment", ctx.HostingEnvironment)
            //                     .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            //                     .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information);
            //                     // .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning);
            // #if DEBUG
            //                     // Used to filter out potentially bad data due debugging.
            //                     // Very useful when doing Seq dashboards and want to remove logs under debugging session.
            //                     loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
            // #endif
            //                 });

            // Add services to the container.
            builder.Services.AddControllers()
                            .AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            // asp.net will block here, we do not want to block our codes, so we let them run in background
            Task.Run(() => app.Run($"http://{ListeningIPEndPoint}"));
        }
    }
}