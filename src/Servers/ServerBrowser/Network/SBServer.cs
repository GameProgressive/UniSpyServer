using NetCoreServer;
using System;
using System.Net;
using System.Text;
using UniSpyLib.Network;

namespace ServerBrowser.Network
{
    /// <summary>
    /// This class emulates the master.gamespy.com TCP server on port 28910.
    /// This server is responisible for sending server lists to the online server browser in the game.
    /// </summary>
    public class SBServer : UniSpyTCPServerBase
    {
        /// <summary>
        /// we are not gamespy,
        /// for simplicity we use hard coded challenge
        /// to reduce computation cost of our program
        /// </summary>
        public static byte[] BytesServerChallenge => Encoding.ASCII.GetBytes(ServerChallenge);
        public static readonly string ServerChallenge = "0000000000";

        public SBServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
        }

        protected override TcpSession CreateSession() { return new SBSession(this); }
    }
}
