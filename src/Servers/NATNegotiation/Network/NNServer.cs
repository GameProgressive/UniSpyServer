using NATNegotiation.Handler.CmdSwitcher;
using System.Net;
using UniSpyLib.Network;

namespace NATNegotiation.Network
{
    public class NNServer : UniSpyUDPServerBase
    {
        public NNServer(IPAddress address, int port) : base(address, port)
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
