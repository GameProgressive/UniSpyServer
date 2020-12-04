using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Buddy
{
    public class AddBuddyRequest : PCMRequestBase
    {
        public uint FriendProfileID { get; protected set; }
        public string AddReason { get; protected set; }
        public AddBuddyRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!KeyValues.ContainsKey("sesskey") || !KeyValues.ContainsKey("newprofileid") || !KeyValues.ContainsKey("reason"))
            {
                return GPErrorCode.Parse;
            }

            uint friendPID;
            if (!uint.TryParse(KeyValues["newprofileid"], out friendPID))
            {
                return GPErrorCode.Parse;
            }

            FriendProfileID = friendPID;
            AddReason = KeyValues["reason"];

            return GPErrorCode.NoError;
        }
    }
}
