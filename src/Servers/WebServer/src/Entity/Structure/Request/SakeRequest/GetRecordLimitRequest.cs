using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.WebServer.Abstraction;
using UniSpyServer.WebServer.Entity.Contract;

namespace UniSpyServer.WebServer.Entity.Structure.Request.SakeRequest
{
    [RequestContract("GetRecordLimit")]
    public class GetRecordLimitRequest : SakeRequestBase
    {
        public GetRecordLimitRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}