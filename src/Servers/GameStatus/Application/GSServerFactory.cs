using GameStatus.Network;
using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using UniSpyLib.UniSpyConfig;

namespace GameStatus.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    internal sealed class GSServerFactory : UniSpyServerFactoryBase
    {
        public static new GSServer Server
        {
            get => (GSServer)UniSpyServerFactoryBase.Server;
            private set => UniSpyServerFactoryBase.Server = value;
        }        /// <summary>
                 /// Constructor
                 /// </summary>
                 /// <param name="serverName">Server name in config file</param>
        public GSServerFactory(string serverName) : base(serverName)
        {
        }

        /// <summary>
        /// Starts a specific server
        /// </summary>
        /// <param name="cfg">The configuration of the specific server to run</param>
        protected override void StartServer(UniSpyServerConfig cfg)
        {
            if (cfg.Name == ServerName)
            {
                Server = new GSServer(cfg.ServerID, cfg.ListeningEndPoint);
                Server.Start();
                Console.WriteLine(
                     StringExtensions.FormatTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));

            }
        }
    }
}
