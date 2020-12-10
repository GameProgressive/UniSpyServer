using UniSpyLib.Abstraction.Interface;
using PresenceConnectionManager.Entity.Structure.Request;
using System.Collections.Generic;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    //\addbuddy\\sesskey\<>\newprofileid\<>\reason\<>\final\
    public class AddBuddyHandler : PCMCommandHandlerBase
    {
        protected new AddBuddyRequest _request { get { return (AddBuddyRequest)base._request; } }
        public AddBuddyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
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
