using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;
using UniSpy.Server.PresenceConnectionManager.Contract.Result;
using UniSpy.Server.PresenceConnectionManager.Application;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Buddy
{
    //\addbuddy\\sesskey\<>\newprofileid\<>\reason\<>\final\

    public sealed class AddBuddyHandler : LoggedInCmdHandlerBase
    {
        private new AddBuddyRequest _request => (AddBuddyRequest)base._request;

        public AddBuddyHandler(Client client, AddBuddyRequest request) : base(client, request)
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
