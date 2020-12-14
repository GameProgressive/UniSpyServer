using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using UniSpyLib.UniSpyConfig;
using NATNegotiation.Network;
using System;
using System.Net;

namespace NATNegotiation.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    public class NNServerManager : UniSpyServerManagerBase
    {
        public new static NNServer Server { get; protected set; }
        public NNServerManager(string serverName) : base(serverName)
        {
        }
        /// <summary>
        /// Starts a specific server, you can also start all server in once if you do not check the server name.
        /// </summary>
        /// <param name="cfg">The configuration of the specific server to run</param>
        protected override void StartServer(ServerConfig cfg)
        {
            if (cfg.Name == ServerName)
            {
                Server = new NNServer(IPAddress.Parse(cfg.ListeningAddress), cfg.ListeningPort);
                Server.Start();
                Console.WriteLine(
                    StringExtensions.FormatTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));
            }
        }
    }
}
