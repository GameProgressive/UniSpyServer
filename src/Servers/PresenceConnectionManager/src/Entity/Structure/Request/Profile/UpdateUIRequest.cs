using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request.Profile
{
    [RequestContract("updateui")]
    public sealed class UpdateUiRequest : RequestBase
    {
        public UpdateUiRequest(string rawRequest) : base(rawRequest)
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
