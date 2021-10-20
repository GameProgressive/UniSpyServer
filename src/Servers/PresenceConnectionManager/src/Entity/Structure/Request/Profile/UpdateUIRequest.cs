using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Contract;

namespace UniSpyServer.PresenceConnectionManager.Entity.Structure.Request.Profile
{
    [RequestContract("updateui")]
    public sealed class UpdateUIRequest : RequestBase
    {
        public UpdateUIRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (RequestKeyValues.ContainsKey(""))
            {

            }
            //cpubrandid
            //cpuspeed
            //memory
            //videocard1ram
            //videocard2ram
            //connectionid
            //connectionspeed
            //hasnetwork
            //pic
        }
    }
}
