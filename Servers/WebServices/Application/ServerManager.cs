using System;
using System.Net;
using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.RetroSpyConfig;
using WebServices.Server;

namespace WebServices.Application
{
    public class ServerManager : ServerManagerBase
    {
        public ServerManager(string serverName) : base(serverName)
        {
        }

        protected override void StartServer(ServerConfig cfg)
        {
            if (cfg.Name == ServerName)
            {
                Server = new WebServer(IPAddress.Parse(cfg.ListeningAddress), cfg.ListeningPort).Start();
                Console.WriteLine(
                     StringExtensions.FormatServerTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));
            }
        }
    }
}
