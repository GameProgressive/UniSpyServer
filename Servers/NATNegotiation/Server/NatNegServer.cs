using GameSpyLib.Database.Entity;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using NATNegotiation.Entity.Enumerator;
using NATNegotiation.Handler.CommandHandler;
using NATNegotiation.Handler.CommandHandler.CommandSwitcher;
using NATNegotiation.Entity.Structure.Packet;
using System;
using System.Net;

namespace NATNegotiation
{
    public class NatNegServer : TemplateUdpServer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbdriver">If choose sqlite for database you should pass the connection to server
        /// ,maybe NatNeg server dose not need connected to database.</param>
        /// <param name="bindTo"></param>
        /// <param name="MaxConnections"></param>
        public NatNegServer(string serverName, DatabaseEngine engine, IPAddress address, int port) : base(serverName, address, port)
        {

        }

        protected override void OnReceived(EndPoint endpoint, byte[] message)
        {

            CommandSwitcher.Switch(this,message);
            
        }

    }

}
