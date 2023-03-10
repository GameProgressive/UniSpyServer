using UniSpy.Server.ServerBrowser.Abstraction;

namespace UniSpy.Server.ServerBrowser.Contract.Request
{

    public class ServerInfoRequest : AdHocRequestBase
    {
        public ServerInfoRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}