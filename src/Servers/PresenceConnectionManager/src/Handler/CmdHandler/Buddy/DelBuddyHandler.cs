using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Application;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler.Buddy
{
    /// <summary>
    /// handles dell buddy request,remove friends from friends list
    /// </summary>

    public sealed class DelBuddyHandler : LoggedInCmdHandlerBase
    {
        private new DelBuddyRequest _request => (DelBuddyRequest)base._request;
        //delete friend in database then send bm_revoke message to friend
        public DelBuddyHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            StorageOperation.Persistance.DeleteFriendByProfileId(_client.Info.ProfileInfo.ProfileId,
                                                                 _request.TargetId,
                                                                 _client.Info.SubProfileInfo.NamespaceId);
        }
    }
}
