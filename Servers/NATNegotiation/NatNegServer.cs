using GameSpyLib.Database;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using NATNegotiation.Enumerator;
using NATNegotiation.Handler;
using NATNegotiation.Handler.Switcher;
using NATNegotiation.Structure.Packet;
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
        public NatNegServer(string serverName, DatabaseDriver databaseDriver, IPAddress address, int port) : base(serverName, address, port)
        {

        }

        protected override void OnReceived(EndPoint endpoint, byte[] message)
        {

            PacketSwitcher.Switch(this,message);
            
        }

    }

}
