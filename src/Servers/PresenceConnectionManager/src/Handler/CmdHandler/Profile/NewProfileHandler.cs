using UniSpy.Server.PresenceConnectionManager.Application;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;
using UniSpy.Server.PresenceConnectionManager.Contract.Response;
using UniSpy.Server.PresenceConnectionManager.Contract.Result;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Profile
{

    public sealed class NewProfileHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        private new NewProfileRequest _request => (NewProfileRequest)base._request;
        private new NewProfileResult _result { get => (NewProfileResult)base._result; set => base._result = value; }
        public NewProfileHandler(Client client, NewProfileRequest request) : base(client, request)
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
