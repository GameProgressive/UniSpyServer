using Chat.Network;
using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using UniSpyLib.UniSpyConfig;

namespace Chat.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    public class ChatServerFactory : UniSpyServerFactoryBase
    {
        public new static ChatServer Server { get; protected set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverName">Server name in config file</param>
        public ChatServerFactory(string serverName) : base(serverName)
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
                Server = new ChatServer(IPAddress.Parse(cfg.ListeningAddress), cfg.ListeningPort);
                Server.Start();
                Console.WriteLine(
                    StringExtensions.FormatTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));
            }
        }
    }
}
