
using UniSpyServer.Servers.WebServer.Module.Sake.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Sake.Structure.Request
{
    
    public class GetRecordLimitRequest : RequestBase
    {
        public GetRecordLimitRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}