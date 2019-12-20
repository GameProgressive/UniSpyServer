using GameSpyLib.Network;

namespace ServerBrowser
{
    public class SBSession : TemplateTcpSession
    {
        public SBSession(TemplateTcpServer server) : base(server)
        {
        }
    }
}
