using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using System;
using System.Net;
using PresenceSearchPlayer.Network;

namespace PresenceSearchPlayer
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    public class PSPServerManager : UniSpyServerManagerBase
    {
        public new static PSPServer Server { get; protected set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverName">Server name in config file</param>
        public PSPServerManager(string serverName) : base(serverName)
        {
        }

        /// <summary>
        /// Starts a specific server
        /// </summary>
        /// <param name="cfg">The configuration of the specific server to run</param>
        protected override void StartServer(UniSpyLib.UniSpyConfig.ServerConfig cfg)
        {
            if (cfg.Name == ServerName)
            {
                Server = new PSPServer(IPAddress.Parse(cfg.ListeningAddress), cfg.ListeningPort);
                Server.Start();
                Console.WriteLine(
                    StringExtensions.FormatTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));
            }
        }
    }
}
