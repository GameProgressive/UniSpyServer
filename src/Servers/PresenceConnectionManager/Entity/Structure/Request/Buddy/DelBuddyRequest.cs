using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Buddy
{
    public class DelBuddyRequest : PCMRequestBase
    {
        //\delbuddy\\sesskey\<>\delprofileid\<>\final\
        public uint DeleteProfileID { get; protected set; }
        public DelBuddyRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }
            if (!_recv.ContainsKey("delprofileid"))
            {

                return GPErrorCode.Parse;
            }

            uint deleteProfileID;
            if (!uint.TryParse(_recv["delprofileid"], out deleteProfileID))
            {
                return GPErrorCode.Parse;
            }

            DeleteProfileID = deleteProfileID;
            return GPErrorCode.NoError;
        }
    }
}
