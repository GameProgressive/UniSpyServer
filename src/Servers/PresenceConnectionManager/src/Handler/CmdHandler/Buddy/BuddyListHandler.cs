using System.Threading.Tasks;
using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Application;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;
using UniSpy.Server.PresenceConnectionManager.Contract.Response;
using UniSpy.Server.PresenceConnectionManager.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Buddy
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
