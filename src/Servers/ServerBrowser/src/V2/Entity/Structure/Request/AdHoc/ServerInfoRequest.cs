using UniSpy.Server.ServerBrowser.V2.Abstraction;

namespace UniSpy.Server.ServerBrowser.V2.Entity.Structure.Request
{

    public class ServerInfoRequest : AdHocRequest
    {
        public ServerInfoRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}