using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    internal class AddBuddyRequest : PCMRequestBase
    {
        public uint FriendProfileID { get; protected set; }
        public string AddReason { get; protected set; }
        public AddBuddyRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            if (!KeyValues.ContainsKey("sesskey") || !KeyValues.ContainsKey("newprofileid") || !KeyValues.ContainsKey("reason"))
            {
                throw new GPGeneralException("addbuddy request is invalid.", GPErrorCode.Parse);
            }

            uint friendPID;
            if (!uint.TryParse(KeyValues["newprofileid"], out friendPID))
            {
                throw new GPGeneralException("newprofileid format is incorrect.", GPErrorCode.Parse);
            }

            FriendProfileID = friendPID;
            AddReason = KeyValues["reason"];
        }
    }
}
