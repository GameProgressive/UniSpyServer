using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

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
            if( ErrorCode != GPErrorCode.NoError)
            {
                return;
            }

            if (!KeyValues.ContainsKey("sesskey") || !KeyValues.ContainsKey("newprofileid") || !KeyValues.ContainsKey("reason"))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }

            uint friendPID;
            if (!uint.TryParse(KeyValues["newprofileid"], out friendPID))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }

            FriendProfileID = friendPID;
            AddReason = KeyValues["reason"];
        }
    }
}
