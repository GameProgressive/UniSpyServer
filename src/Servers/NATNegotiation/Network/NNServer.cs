using UniSpyLib.Network;
using NATNegotiation.Handler.CommandSwitcher;
using System.Net;

namespace NATNegotiation.Network
{
    public class NNServer : UDPServerBase
    {


        public NNServer(IPAddress address, int port) : base(address, port)
        {
        }

        protected override UDPSessionBase CreateSession(EndPoint endPoint)
        {
            return new NNSession(this, endPoint);
        }


        //TODO fix the natnegotiation for this architecture

        protected override void OnReceived(UDPSessionBase session, byte[] message)
        {
            base.OnReceived(session, message);
            new NNCommandSwitcher(session, message).Switch();
        }
    }
}
