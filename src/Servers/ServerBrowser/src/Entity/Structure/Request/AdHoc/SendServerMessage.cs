using UniSpyServer.Servers.ServerBrowser.Abstraction;
using UniSpyServer.Servers.ServerBrowser.Entity.Contract;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request
{
    [RequestContract(RequestType.SendMessageRequest)]

    public class SendMessageRequest : AdHocRequest
    {
        public SendMessageRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}