using PresenceConnectionManager.Network;
using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using UniSpyLib.UniSpyConfig;

namespace PresenceConnectionManager.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    internal class PCMServerFactory : UniSpyServerFactoryBase
    {
        public new static PCMServer Server { get; protected set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverName">Server name in config file</param>
        public PCMServerFactory(string serverName) : base(serverName)
        {
        }

        /// <summary>
        /// Starts a specific server
        /// </summary>
        /// <param name="cfg">The configuration of the specific server to run</param>
        protected override void StartServer(ServerConfig cfg)
        {
            if (cfg.Name == ServerName)
            {
                Server = new PCMServer(IPAddress.Parse(cfg.ListeningAddress), cfg.ListeningPort);
                Server.Start();
                Console.WriteLine(
                      StringExtensions.FormatTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));
            }
        }

        /// <summary>
        /// Stop a specific server
        /// </summary>
        /// <param name="cfg">The configuration of the specific server to stop</param>

    }
}
