using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    [Command("updateui")]
    internal sealed class UpdateUIRequest : PCMRequestBase
    {
        public UpdateUIRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (KeyValues.ContainsKey(""))
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
