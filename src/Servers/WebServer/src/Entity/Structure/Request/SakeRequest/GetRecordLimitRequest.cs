using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.SakeRequest
{
    [RequestContract("GetRecordLimit")]
    public class GetRecordLimitRequest : SakeRequestBase
    {
        public GetRecordLimitRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}