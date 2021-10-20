using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.PresenceConnectionManager.Entity.Structure.Request
{
    [RequestContract("addbuddy")]
    public sealed class AddBuddyRequest : RequestBase
    {
        public uint FriendProfileID { get; private set; }
        public string AddReason { get; private set; }
        public AddBuddyRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            if (!RequestKeyValues.ContainsKey("sesskey") || !RequestKeyValues.ContainsKey("newprofileid") || !RequestKeyValues.ContainsKey("reason"))
            {
                throw new GPParseException("addbuddy request is invalid.");
            }

            uint friendPID;
            if (!uint.TryParse(RequestKeyValues["newprofileid"], out friendPID))
            {
                throw new GPParseException("newprofileid format is incorrect.");
            }

            FriendProfileID = friendPID;
            AddReason = RequestKeyValues["reason"];
        }
    }
}
