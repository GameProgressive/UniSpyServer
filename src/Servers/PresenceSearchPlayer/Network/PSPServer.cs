using UniSpyLib.Network;
using NetCoreServer;
using System.Net;

namespace PresenceSearchPlayer.Network
{
    public class PSPServer : TCPServerBase
    {
        public PSPServer(IPAddress address, int port) : base(address, port)
        {
        }

        protected override TcpSession CreateSession()
        {
            return new PSPSession(this);
        }
    }
}
