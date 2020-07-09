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
        IWebHost _host;
        public ServerManager(string serverName) : base(serverName)
        {
        }

        public override void Start()
        {
            ShowRetroSpyLogo();
            //currently we do not need database connection
            //LoadDatabaseConfig();
            LoadServerConfig();
            _host.Run();
        }

        protected override void StartServer(ServerConfig cfg)
        {
            if (cfg.Name == ServerName)
            {
                _host = new WebHostBuilder()
                       .UseKestrel(x => x.AllowSynchronousIO = true)
                       .UseUrls($"{cfg.ListeningAddress}:{cfg.ListeningPort}")
                       //.UseContentRoot(Directory.GetCurrentDirectory())
                       .UseSerilog()
                       .UseStartup<Startup>()
                       .Build();

                Console.WriteLine(StringExtensions
                  .FormatServerTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));
            }
        }
    }
}
