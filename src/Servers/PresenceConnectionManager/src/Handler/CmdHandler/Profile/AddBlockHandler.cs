using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Application;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler.Profile
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
