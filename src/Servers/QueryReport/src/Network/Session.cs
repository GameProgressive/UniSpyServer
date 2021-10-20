using System.Net;
using UniSpyServer.QueryReport.Handler.CmdSwitcher;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;

namespace UniSpyServer.QueryReport.Network
{
    public sealed class Session : UniSpyUdpSession
    {
        public uint InstantKey { get; set; }
        public Session(UniSpyUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
        public override void OnReceived(byte[] message) => new CmdSwitcher(this, message).Switch();
    }
}
