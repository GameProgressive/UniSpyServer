using PresenceConnectionManager.Enumerator;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Buddy.AddBuddy
{
    //\addbuddy\\sesskey\<>\newprofileid\<>\reason\<>\final\
    public class AddBuddyHandler : GPCMHandlerBase
    {
        protected AddBuddyHandler(Dictionary<string, string> recv) : base(recv)
        {
        }
        private uint _friendPid;
        protected override void CheckRequest(GPCMSession session)
        {
            base.CheckRequest(session);
            if (!_recv.ContainsKey("sesskey") || !_recv.ContainsKey("newprofileid") || !_recv.ContainsKey("reason"))
            {
                _errorCode = GPErrorCode.Parse;
            }
            if (!uint.TryParse(_recv["newprofileid"], out _friendPid))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataBaseOperation(GPCMSession session)
        {
            //Check if the friend is online
            //if(online)
            //else
            //store add request to database
        }





    }
}
