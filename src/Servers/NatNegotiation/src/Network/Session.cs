using System.Net;
using UniSpyServer.Servers.NatNegotiation.Handler.CmdSwitcher;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;

namespace UniSpyServer.Servers.NatNegotiation.Network
{
    public sealed class Session : UniSpyUdpSession
    {
        public Session(UniSpyUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
        public override void OnReceived(byte[] message) => new CmdSwitcher(this, message).Switch();
    }
}
