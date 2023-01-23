using UniSpyServer.Servers.PresenceConnectionManager.Application;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler.Profile
{

    public sealed class NewProfileHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        private new NewProfileRequest _request => (NewProfileRequest)base._request;
        private new NewProfileResult _result { get => (NewProfileResult)base._result; set => base._result = value; }
        public NewProfileHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new NewProfileResult();
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();
            if (_client.Info.ProfileInfo.Nick != _request.OldNick)
            {
                throw new GPException("The old nickname is not identical to current nickname.");
            }
        }
        protected override void DataOperation()
        {
            if (_request.IsReplaceNickName)
            {
                StorageOperation.Persistance.UpdateNickName(_client.Info.ProfileInfo.ProfileId,
                                                            _request.OldNick,
                                                            _request.NewNick);
            }
            else
            {
                StorageOperation.Persistance.AddNickName(_client.Info.UserInfo.UserId,
                                                         _client.Info.ProfileInfo.ProfileId,
                                                         _request.NewNick);
            }
            _result.ProfileId = _client.Info.ProfileInfo.ProfileId;
        }

        protected override void ResponseConstruct()
        {
            _response = new NewProfileResponse(_request, _result);
        }
    }
}
