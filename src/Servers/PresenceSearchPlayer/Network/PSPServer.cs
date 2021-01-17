using NetCoreServer;
using System.Net;
using UniSpyLib.Network;

namespace PresenceSearchPlayer.Network
{
    public class PSPServer : UniSpyTCPServerBase
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
