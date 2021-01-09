using UniSpyLib.Abstraction.Interface;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Result;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    //\addbuddy\\sesskey\<>\newprofileid\<>\reason\<>\final\
    public class AddBuddyHandler : PCMCmdHandlerBase
    {
        protected new AddBuddyRequest _request => (AddBuddyRequest)base._request;

        public AddBuddyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            _result = new AddBuddyResult();
            throw new System.NotImplementedException();
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
            //Check if the friend is online
            //if(online)
            //else
            //store add request to database
        }

        protected override void ResponseConstruct()
        {
            throw new System.NotImplementedException();
        }
    }
}
