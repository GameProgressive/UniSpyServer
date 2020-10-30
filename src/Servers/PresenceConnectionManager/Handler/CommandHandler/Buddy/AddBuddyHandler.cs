using GameSpyLib.Abstraction.Interface;
using PresenceConnectionManager.Entity.Structure.Request.Buddy;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.CommandHandler.Buddy
{
    //\addbuddy\\sesskey\<>\newprofileid\<>\reason\<>\final\
    public class AddBuddyHandler : PCMCommandHandlerBase
    {
        protected AddBuddyRequest _request;
        public AddBuddyHandler(ISession session, Dictionary<string, string> recv) : base(session, recv)
        {
            _request = new AddBuddyRequest(recv);
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
            //Check if the friend is online
            //if(online)
            //else
            //store add request to database
        }
    }
}
