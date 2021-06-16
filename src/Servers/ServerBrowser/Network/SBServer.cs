using NetCoreServer;
using System;
using System.Net;
using UniSpyLib.Network;

namespace ServerBrowser.Network
{
    /// <summary>
    /// This class emulates the master.gamespy.com TCP server on port 28910.
    /// This server is responisible for sending server lists to the online server browser in the game.
    /// </summary>
    internal sealed class SBServer : UniSpyTCPServerBase
    {
        public SBServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new SBSessionManager();
        }

        protected override TcpSession CreateSession() => new SBSession(this);
    }
}
