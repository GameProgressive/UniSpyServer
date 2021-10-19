using System.Net;
using NatNegotiation.Handler.CmdSwitcher;
using UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;

namespace NatNegotiation.Network
{
    public sealed class Session : UniSpyUdpSession
    {
        public Session(UniSpyUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
        public override void OnReceived(byte[] message) => new CmdSwitcher(this, message).Switch();
    }
}
