using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Module.Direct2Game.Entity.Structure.Request
{
    [RequestContract("GetPurchaseHistory")]
    public class GetPurchaseHistoryRequest : RequestBase
    {
        public GetPurchaseHistoryRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
