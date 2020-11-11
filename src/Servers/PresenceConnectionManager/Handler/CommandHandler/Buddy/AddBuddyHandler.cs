using UniSpyLib.Abstraction.Interface;
using PresenceConnectionManager.Entity.Structure.Request.Buddy;
using System.Collections.Generic;

namespace PresenceConnectionManager.Abstraction.BaseClass.Buddy
{
    //\addbuddy\\sesskey\<>\newprofileid\<>\reason\<>\final\
    public class AddBuddyHandler : PCMCommandHandlerBase
    {
        protected new AddBuddyRequest _request;
        public AddBuddyHandler(ISession session, IRequest request) : base(session, request)
        {
            _request = (AddBuddyRequest)request;
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
