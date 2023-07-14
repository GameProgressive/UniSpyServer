using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay;

namespace UniSpy.Server.GameTrafficRelay.Application
{
    public class Server : ServerBase
    {
        private ServerStatusReporter _reporter;
        static Server()
        {
            _name = "GameTrafficRelay";
        }

        public Server() : base(null)
        {
            _reporter = new ServerStatusReporter(this);
        }

        public override void Start()
        {
            _reporter.Start();
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

        protected override IConnectionManager CreateConnectionManager(IPEndPoint endPoint) => null;

        protected override IClient CreateClient(IConnection connection) => null;
    }
}