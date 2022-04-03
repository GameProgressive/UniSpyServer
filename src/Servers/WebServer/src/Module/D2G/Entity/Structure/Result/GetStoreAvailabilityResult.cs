using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.D2G.Entity.Structure.Result
{
    public class GetStoreAvailabilityResult : ResultBase
    {
        public int Status { get; set; }
        public int StoreResult { get; set; }

        public GetStoreAvailabilityResult()
        {

        }
    }
}
