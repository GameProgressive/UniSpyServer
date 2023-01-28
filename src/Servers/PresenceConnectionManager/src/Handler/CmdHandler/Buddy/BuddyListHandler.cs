using System.Threading.Tasks;
using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Application;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler.Buddy
{
    public sealed class BuddyListHandler : LoggedInCmdHandlerBase
    {
        private new BuddyListResult _result { get => (BuddyListResult)base._result; set => base._result = value; }
        public BuddyListHandler(IClient client) : base(client, null)
        {
            _result = new BuddyListResult();
        }
        protected override void RequestCheck() { }
        protected override void DataOperation()
        {
            var friendsId = StorageOperation.Persistance.GetFriendProfileIds(_client.Info.ProfileInfo.ProfileId,
                                                                          _client.Info.SubProfileInfo.NamespaceId);
            _result.ProfileIDList = friendsId;

        }
        protected override void ResponseConstruct()
        {
            _response = new BuddyListResponse(_request, _result);
        }
        protected override void Response()
        {
            base.Response();
            if (!_client.Info.SdkRevision.IsSupportGPINewStatusNotification)
            {
                return;
            }

            Parallel.ForEach(_result.ProfileIDList, (profileID) =>
            {
                var request = new StatusInfoRequest
                {
                    ProfileId = profileID,
                    NamespaceID = (int)_client.Info.SubProfileInfo.NamespaceId,
                    IsGetStatusInfo = true
                };
                new StatusInfoHandler(_client, request).Handle();
            });
        }
    }
}
