using NatNegotiation.Handler.CmdSwitcher;
using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;

namespace NatNegotiation.Network
{
    internal sealed class NNServer : UniSpyUdpServer
    {
        public NNServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new NNSessionManager();
        }

        protected override UniSpyUdpSession CreateSession(EndPoint endPoint) =>
            new NNSession(this, endPoint);

        //TODO fix the natnegotiation for this architecture

        protected override void OnReceived(UniSpyUdpSession session, byte[] message)
        => new NNCmdSwitcher(session, message).Switch();

    }
}
