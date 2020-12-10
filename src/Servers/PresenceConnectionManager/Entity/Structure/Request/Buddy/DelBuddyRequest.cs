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

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }
            if (!KeyValues.ContainsKey("delprofileid"))
            {

                return GPErrorCode.Parse;
            }

            uint deleteProfileID;
            if (!uint.TryParse(KeyValues["delprofileid"], out deleteProfileID))
            {
                return GPErrorCode.Parse;
            }

            DeleteProfileID = deleteProfileID;
            return GPErrorCode.NoError;
        }
    }
}
