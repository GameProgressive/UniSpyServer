using UniSpyLib.Abstraction.Interface;
using PresenceConnectionManager.Entity.Structure.Request.Buddy;
using System.Collections.Generic;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CommandHandler
{
    //\addbuddy\\sesskey\<>\newprofileid\<>\reason\<>\final\
    public class AddBuddyHandler : PCMCommandHandlerBase
    {
        protected new AddBuddyRequest _request;
        public AddBuddyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _request = (AddBuddyRequest)request;
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
