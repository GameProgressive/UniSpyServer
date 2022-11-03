using UniSpyServer.Servers.ServerBrowser.Abstraction;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request
{

    public class ServerInfoRequest : AdHocRequest
    {
        public ServerInfoRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}