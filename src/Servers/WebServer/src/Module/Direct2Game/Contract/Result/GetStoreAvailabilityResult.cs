using UniSpy.Server.WebServer.Module.Direct2Game.Abstraction;

namespace UniSpy.Server.WebServer.Module.Direct2Game.Contract.Result
{
    public enum AvaliableCode
    {
        StoreOnline = 10,
        StoreOfflineForMaintaince = 20,
        StoreOfflineRetired = 50,
        StoreNotYetLaunched = 100
    }
    public class GetStoreAvailabilityResult : ResultBase
    {
        public int Code { get; set; } = 0;
        public AvaliableCode StoreStatusId { get; set; } = AvaliableCode.StoreOnline;

        public GetStoreAvailabilityResult()
        {
        }
    }
}
