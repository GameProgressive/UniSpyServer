using UniSpyServer.Servers.WebServer.Abstraction;


namespace UniSpyServer.Servers.WebServer.Module.Direct2Game.Entity.Structure.Request
{
    
    public class GetPurchaseHistoryRequest : RequestBase
    {
        public GetPurchaseHistoryRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
