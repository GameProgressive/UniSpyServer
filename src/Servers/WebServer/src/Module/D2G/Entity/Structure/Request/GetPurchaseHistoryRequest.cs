using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Module.D2G.Entity.Structure.Request
{
    [RequestContract("GetPurchaseHistory")]
    public class GetPurchaseHistoryRequest : RequestBase
    {
        public GetPurchaseHistoryRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
