using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpy.Server.PresenceConnectionManager.Entity.Structure.Request
{

    public sealed class AddBuddyRequest : RequestBase
    {
        public int FriendProfileID { get; private set; }
        public string Reason { get; private set; }
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

            int friendPID;
            if (!int.TryParse(RequestKeyValues["newprofileid"], out friendPID))
            {
                throw new GPParseException("newprofileid format is incorrect.");
            }

            FriendProfileID = friendPID;
            Reason = RequestKeyValues["reason"];
        }
    }
}
