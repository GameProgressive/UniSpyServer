using NATNegotiation.Handler.CmdSwitcher;
using System;
using System.Net;
using UniSpyLib.Network;

namespace NATNegotiation.Network
{
    public class NNServer : UniSpyUDPServerBase
    {
        public NNServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new NNSessionManager();
        }

        protected override UniSpyUDPSessionBase CreateSession(EndPoint endPoint) =>
            new NNSession(this, endPoint);

        //TODO fix the natnegotiation for this architecture

        protected override void OnReceived(UniSpyUDPSessionBase session, byte[] message)
        => new NNCmdSwitcher(session, message).Switch();

    }
}
