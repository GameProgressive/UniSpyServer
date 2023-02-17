using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Entity.Structure.Request;
using UniSpy.Server.PresenceConnectionManager.Entity.Structure.Result;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Buddy
{
    //\addbuddy\\sesskey\<>\newprofileid\<>\reason\<>\final\

    public sealed class AddBuddyHandler : LoggedInCmdHandlerBase
    {
        private new AddBuddyRequest _request => (AddBuddyRequest)base._request;

        public AddBuddyHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new AddBuddyResult();
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
