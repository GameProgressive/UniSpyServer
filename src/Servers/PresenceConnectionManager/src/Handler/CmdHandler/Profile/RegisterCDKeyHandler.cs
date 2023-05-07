using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Application;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Profile
{

    public sealed class RegisterCDKeyHandler : LoggedInCmdHandlerBase
    {
        private new RegisterCDKeyRequest _request => (RegisterCDKeyRequest)base._request;
        public RegisterCDKeyHandler(Client client, RegisterCDKeyRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            _client.Info.SubProfileInfo.Cdkeyenc = _request.CDKeyEnc;
            StorageOperation.Persistance.UpdateSubProfileInfo(_client.Info.SubProfileInfo);
        }
    }
}
