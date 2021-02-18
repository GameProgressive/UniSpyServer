﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;
using UniSpyLib.UniSpyConfig;

namespace WebServices.Application
{
    public class WebServerFactory : UniSpyServerFactoryBase
    {
        private IHostBuilder _hostBuilder;
        public WebServerFactory(string serverName) : base(serverName)
        {
        }

        public override void Start()
        {
            ShowRetroSpyLogo();
            //currently we do not need database connection
            //LoadDatabaseConfig();
            LogWriter.SettngUpLogger();
            LoadServerConfig();
            _hostBuilder.Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(UniSpyServerConfig cfg)
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
        protected override void StartServer(UniSpyServerConfig cfg)
        {
            if (cfg.Name == ServerName)
            {
                _hostBuilder = CreateHostBuilder(cfg);

                Console.WriteLine(StringExtensions
                  .FormatTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));
            }
        }
    }
}