using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Application;
using UniSpy.Server.PresenceConnectionManager.Entity.Structure.Request;
using UniSpy.Server.PresenceSearchPlayer.Entity.Exception.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Profile
{

    public sealed class AddBlockHandler : LoggedInCmdHandlerBase
    {
        private new AddBlockRequest _request => (AddBlockRequest)base._request;
        public AddBlockHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();
            if (_client.Info.ProfileInfo.ProfileId == _request.TargetId)
            {
                throw new GPException("You can not block your self.");
            }
        }
        protected override void DataOperation()
        {
            StorageOperation.Persistance.UpdateBlockInfo(_request.TargetId,
                                                         _client.Info.ProfileInfo.ProfileId,
                                                         _client.Info.SubProfileInfo.NamespaceId);
        }
    }
}
