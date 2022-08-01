using UniSpyServer.Servers.ServerBrowser.Abstraction;

using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request
{
    

    public class SendMsgRequest : AdHocRequest
    {
        public SendMsgRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}