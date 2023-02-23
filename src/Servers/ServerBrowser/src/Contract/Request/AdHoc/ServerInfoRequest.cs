using UniSpy.Server.ServerBrowser.Abstraction;

namespace UniSpy.Server.ServerBrowser.Contract.Request
{

    public class ServerInfoRequest : AdHocRequest
    {
        public ServerInfoRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}