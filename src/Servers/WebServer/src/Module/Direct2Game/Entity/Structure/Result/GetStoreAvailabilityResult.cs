using UniSpyServer.Servers.WebServer.Module.Direct2Game.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Direct2Game.Entity.Structure.Result
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
