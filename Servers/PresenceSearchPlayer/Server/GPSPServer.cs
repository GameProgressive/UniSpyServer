using GameSpyLib.Database.Entity;
using GameSpyLib.Network;
using NetCoreServer;
using System.Net;

namespace PresenceSearchPlayer
{
    public class GPSPServer : TemplateTcpServer
    {
        public GPSPServer(IPAddress address, int port) : base(address, port)
        {
        }

        protected override TcpSession CreateSession()
        {
            return new GPSPSession(this);
        }
    }
}
