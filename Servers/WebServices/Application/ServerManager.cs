using System;
using System.IO;
using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.RetroSpyConfig;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace WebServices.Application
{
    public class ServerManager : ServerManagerBase
    {
        public ServerManager(string serverName) : base(serverName)
        {
        }

        public override void Start()
        {
            ShowRetroSpyLogo();
            //currently we do not need database connection
            //LoadDatabaseConfig();
            LoadServerConfig();
        }

        protected override void StartServer(ServerConfig cfg)
        {
            if (cfg.Name == ServerName)
            {
                var host = new WebHostBuilder()
                       .UseKestrel(x => x.AllowSynchronousIO = true)
                       .UseUrls($"{cfg.ListeningAddress}:{cfg.ListeningPort}")
                       .UseContentRoot(Directory.GetCurrentDirectory())
                       .UseSerilog()
                       .UseStartup<Startup>()
                       .Build();

                host.Run();
                Console.WriteLine(
                    StringExtensions.FormatServerTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));
            }
        }
    }
}
