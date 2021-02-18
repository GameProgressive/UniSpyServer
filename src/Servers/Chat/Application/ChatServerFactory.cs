using Chat.Network;
using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using UniSpyLib.UniSpyConfig;

namespace Chat.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    internal sealed class ChatServerFactory : UniSpyServerFactoryBase
    {
        public static new ChatServer Server
        {
            get => (ChatServer)UniSpyServerFactoryBase.Server;
            private set => UniSpyServerFactoryBase.Server = value;
        }
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
                Server = new ChatServer(cfg.ServerID,cfg.ListeningEndPoint);
                Server.Start();
                Console.WriteLine(
                    StringExtensions.FormatTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));
            }
        }
    }
}
