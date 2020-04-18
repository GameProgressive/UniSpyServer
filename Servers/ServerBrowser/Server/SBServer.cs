using GameSpyLib.Network;
using NatNegotiation.Entity.Structure;
using NetCoreServer;
using System.Collections.Concurrent;
using System.Net;

namespace ServerBrowser
{
    /// <summary>
    /// This class emulates the master.gamespy.com TCP server on port 28910.
    /// This server is responisible for sending server lists to the online server browser in the game.
    /// </summary>
    public class SBServer : TemplateTcpServer
    {
        /// <summary>
        /// we are not gamespy,
        /// for simplicity we use hard coded challenge
        /// to reduce computation cost of our program
        /// </summary>
        public static readonly string ServerChallenge = "0000000000";

        public static ConcurrentDictionary<EndPoint, NatNegClientInfo> ClientList = new ConcurrentDictionary<EndPoint, NatNegClientInfo>();

        public SBServer(IPAddress address, int port) : base(address, port)
        {
        }

        protected override TcpSession CreateSession() { return new SBSession(this); }
    }
}
