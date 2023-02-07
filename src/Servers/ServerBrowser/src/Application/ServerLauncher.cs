using System;
using System.Linq;
using ConsoleTables;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.ServerBrowser.Application
{
    internal sealed class ServerLauncher : ServerLauncherBase
    {
        public static IServer ServerInstanceV1 => ServerLauncher.ServerInstance;
        public static IServer ServerInstanceV2;
        public ServerLauncher() : base("ServerBrowser")
        {
        }
        protected override void LaunchServer()
        {
            var cfgs = ConfigManager.Config.Servers.Where(s => s.ServerName.Contains(ServerName)).ToArray();
            ServerInstance = LaunchNetworkService(cfgs.Where(c => c.ServerName == $"{ServerName}V1").First());
            ServerInstanceV2 = LaunchNetworkService(cfgs.Where(c => c.ServerName == $"{ServerName}V2").First());

            if (ServerInstanceV1 is null && ServerInstanceV2 is null)
            {
                throw new Exception("Server created failed");
            }
            // asp.net web server does not implement a Server interface, therefore this code should not be called
            ServerInstanceV1.Start();
            ServerInstanceV2.Start();

            var table = new ConsoleTable("Server Name", "Listening Address", "Listening Port");
            table.AddRow($"{ServerName}V1", ServerInstanceV1.ListeningIPEndPoint.Address, ServerInstanceV1.ListeningIPEndPoint.Port);
            table.AddRow($"{ServerName}V2", ServerInstanceV2.ListeningIPEndPoint.Address, ServerInstanceV2.ListeningIPEndPoint.Port);
            table.Write(ConsoleTables.Format.Alternative);
            Console.WriteLine("Server successfully started!");
        }
        protected override IServer LaunchNetworkService(UniSpyServerConfig config) => new TcpServer(config);
    }
}