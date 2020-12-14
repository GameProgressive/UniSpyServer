using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    public class DelBuddyRequest : PCMRequestBase
    {
        //\delbuddy\\sesskey\<>\delprofileid\<>\final\
        public uint DeleteProfileID { get; protected set; }
        public DelBuddyRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if( ErrorCode != GPErrorCode.NoError)
            {
                return;
            }
            if (!KeyValues.ContainsKey("delprofileid"))
            {

                ErrorCode = GPErrorCode.Parse; return;
            }

            uint deleteProfileID;
            if (!uint.TryParse(KeyValues["delprofileid"], out deleteProfileID))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }

            DeleteProfileID = deleteProfileID;
            ErrorCode = GPErrorCode.NoError;
        }
    }
}
