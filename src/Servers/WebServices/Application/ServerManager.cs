using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.RetroSpyConfig;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace WebServices.Application
{
    public class ServerManager : ServerManagerBase
    {
        private IHostBuilder _hostBuilder;
        public ServerManager(string serverName) : base(serverName)
        {
        }

        public override void Start()
        {
            ShowRetroSpyLogo();
            //currently we do not need database connection
            //LoadDatabaseConfig();
            SettingUpSerilog();
            LoadServerConfig();
            _hostBuilder.Build().Run();
        }

        private void SettingUpSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console(outputTemplate: "{Timestamp:[HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}")
                        .WriteTo.File($"Logs/[{ServerName}]-.log",
                        outputTemplate: "{Timestamp:[yyyy-MM-dd HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}", rollingInterval: RollingInterval.Day)
                        .CreateLogger();
        }

        private static IHostBuilder CreateHostBuilder(ServerConfig cfg)
        {
            return Host.CreateDefaultBuilder()
            .UseSerilog() // <- Add this line
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel(x => x.AllowSynchronousIO = true)
                           .UseUrls($"{cfg.ListeningAddress}:{cfg.ListeningPort}")
                           //.UseContentRoot(Directory.GetCurrentDirectory())
                           .UseStartup<Startup>();
            });
        }



        protected override void StartServer(ServerConfig cfg)
        {
            if (cfg.Name == ServerName)
            {
                _hostBuilder = CreateHostBuilder(cfg);

                Console.WriteLine(StringExtensions
                  .FormatServerTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));
            }
        }
    }
}
