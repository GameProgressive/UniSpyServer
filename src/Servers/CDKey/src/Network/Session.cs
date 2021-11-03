using System.Net;
using UniSpyServer.Servers.CDkey.Handler.CmdSwitcher;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;

namespace UniSpyServer.Servers.CDkey.Network
{
    public sealed class Session : UniSpyUdpSession
    {
        public Session(UniSpyUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
        public override void OnReceived(string message) => new CDKeyCmdSwitcher(this, message).Switch();
    }
}
