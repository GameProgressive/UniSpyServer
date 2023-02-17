using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Application;
using UniSpy.Server.PresenceConnectionManager.Entity.Structure.Request;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Buddy
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
