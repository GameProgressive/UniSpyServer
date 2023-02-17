using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpy.Server.PresenceConnectionManager.Entity.Structure.Request
{

    public sealed class AddBlockRequest : RequestBase
    {
        public int TargetId;
        public AddBlockRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (!RequestKeyValues.ContainsKey("profileid"))
            {
                throw new GPParseException("profileid is missing");

            }

            int profileID;
            if (!int.TryParse(RequestKeyValues["profileid"], out profileID))
            {
                throw new GPParseException("profileid format is incorrect");
            }

            TargetId = profileID;

        }
    }
}
