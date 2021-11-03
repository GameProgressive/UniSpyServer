using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request
{
    [RequestContract("addblock")]
    public sealed class AddBlockRequest : RequestBase
    {
        public uint ProfileID;
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

            uint profileID;
            if (!uint.TryParse(RequestKeyValues["profileid"], out profileID))
            {
                throw new GPParseException("profileid format is incorrect");
            }

            ProfileID = profileID;

        }
    }
}
