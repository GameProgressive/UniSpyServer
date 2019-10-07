using GameSpyLib.Network;

namespace ServerBrowser
{
    public class SBSession : TcpSession
    {
        public SBSession(TcpServer server) : base(server)
        {
        }
    }
}
