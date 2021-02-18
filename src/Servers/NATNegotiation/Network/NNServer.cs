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
        }

        protected override UniSpyUDPSessionBase CreateSession(EndPoint endPoint)
        {
            return new NNSession(this, endPoint);
        }

        //TODO fix the natnegotiation for this architecture

        protected override void OnReceived(UniSpyUDPSessionBase session, byte[] message)
        {
            base.OnReceived(session, message);
            new NNCmdSwitcher(session, message).Switch();
        }
    }
}
