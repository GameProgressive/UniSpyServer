
using UniSpy.Server.WebServer.Module.Sake.Abstraction;

namespace UniSpy.Server.WebServer.Module.Sake.Contract.Request
{
    
    public class GetRecordLimitRequest : RequestBase
    {
        public GetRecordLimitRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}