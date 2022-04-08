using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Direct2Game.Entity.Result
{
    public class GetStoreAvailabilityResult : ResultBase
    {
        public int StatusCode { get; set; }
        public int StoreResult { get; set; }

        public GetStoreAvailabilityResult()
        {

        }
    }
}
