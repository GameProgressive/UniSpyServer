using UniSpy.Server.WebServer.Module.Direct2Game.Abstraction;

namespace UniSpy.Server.WebServer.Module.Direct2Game.Contract.Result
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
