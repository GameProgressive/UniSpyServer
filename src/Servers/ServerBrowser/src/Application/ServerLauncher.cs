using System;
using System.Linq;
using ConsoleTables;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.ServerBrowser.Application
{
    public class ServerLauncher : ServerLauncherBase
    {
        public static IServer ServerV1;
        public static IServer ServerV2;
        public ServerLauncher() : base("ServerBrowser")
        {
        }
        protected override void LaunchServer()
        {
            var cfgs = ConfigManager.Config.Servers.Where(s => s.ServerName.Contains(ServerName)).ToArray();
            ServerV1 = LaunchNetworkService(cfgs.Where(c => c.ServerName.Contains("V1")).First());
            ServerV2 = LaunchNetworkService(cfgs.Where(c => c.ServerName.Contains("V2")).First());

            if (ServerV1 is null && ServerV2 is null)
            {
                throw new Exception("Server created failed");
            }
            // asp.net web server does not implement a Server interface, therefore this code should not be called
            ServerV1.Start();
            ServerV2.Start();

            var table = new ConsoleTable("Server Name", "Listening Address", "Listening Port");
            table.AddRow(ServerName, ServerV1.Endpoint.Address, $"{ServerV1.Endpoint.Port}, {ServerV2.Endpoint.Port}");
            table.Write(ConsoleTables.Format.Alternative);
            Servers.Add(ServerV1.ServerName, ServerV1);
            Servers.Add(ServerV2.ServerName, ServerV2);

            Console.WriteLine("Server successfully started!");
        }
        protected override IServer LaunchNetworkService(UniSpyServerConfig config) => new TcpServer(config.ServerID, config.ServerName, config.ListeningEndPoint);
    }
}