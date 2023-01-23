using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler.Buddy
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
