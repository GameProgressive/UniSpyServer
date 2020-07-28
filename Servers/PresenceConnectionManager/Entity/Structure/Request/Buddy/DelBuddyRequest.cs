using System;
using System.Collections.Generic;
using PresenceConnectionManager.Entity.BaseClass;
using PresenceSearchPlayer.Entity.Enumerator;

namespace PresenceConnectionManager.Entity.Structure.Request.Buddy
{
    public class DelBuddyRequest : PCMRequestBase
    {
        //\delbuddy\\sesskey\<>\delprofileid\<>\final\
        public uint DeleteProfileID { get; protected set; }
        public DelBuddyRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
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
