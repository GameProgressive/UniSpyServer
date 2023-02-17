using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Request
{

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
