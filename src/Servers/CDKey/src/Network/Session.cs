using System.Net;
using CDKey.Handler.CmdSwitcher;
using UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;

namespace CDKey.Network
{
    internal sealed class Session : UniSpyUdpSession
    {
        public Session(UniSpyUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
        public override void OnReceived(string message) => new CDKeyCmdSwitcher(this, message).Switch();
    }
}
