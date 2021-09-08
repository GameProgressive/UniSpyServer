using System.Net;
using QueryReport.Handler.CmdSwitcher;
using UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;

namespace QueryReport.Network
{
    internal sealed class Session : UniSpyUdpSession
    {
        public uint InstantKey { get; set; }
        public Session(UniSpyUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
        public override void OnReceived(byte[] message) => new CmdSwitcher(this, message).Switch();
    }
}
