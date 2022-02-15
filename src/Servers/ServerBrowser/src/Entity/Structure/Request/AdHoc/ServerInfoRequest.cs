using UniSpyServer.Servers.ServerBrowser.Abstraction;
using UniSpyServer.Servers.ServerBrowser.Entity.Contract;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request
{
    [RequestContract(RequestType.ServerInfoRequest)]
    public class ServerInfoRequest : AdHocRequest
    {
        public ServerInfoRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}