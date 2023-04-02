using UniSpy.Server.ServerBrowser.V2.Abstraction;

namespace UniSpy.Server.ServerBrowser.V2.Contract.Request
{

    public class ServerInfoRequest : AdHocRequestBase
    {
        public ServerInfoRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}