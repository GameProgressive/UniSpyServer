using GameSpyLib.Common.Entity.Interface;
using PresenceConnectionManager.Enumerator;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Buddy.AddBuddy
{
    //\addbuddy\\sesskey\<>\newprofileid\<>\reason\<>\final\
    public class AddBuddyHandler : PCMCommandHandlerBase
    {

        private uint _friendPid;

        public AddBuddyHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (!_recv.ContainsKey("sesskey") || !_recv.ContainsKey("newprofileid") || !_recv.ContainsKey("reason"))
            {
                _errorCode = GPErrorCode.Parse;
            }

            if (!uint.TryParse(_recv["newprofileid"], out _friendPid))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataOperation()
        {
            //Check if the friend is online
            //if(online)
            //else
            //store add request to database
        }
    }
}
