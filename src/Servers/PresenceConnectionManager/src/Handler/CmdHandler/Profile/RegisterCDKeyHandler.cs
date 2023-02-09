using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Application;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler.Profile
{

    public sealed class RegisterCDKeyHandler : LoggedInCmdHandlerBase
    {
        private new RegisterCDKeyRequest _request => (RegisterCDKeyRequest)base._request;
        public RegisterCDKeyHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            _client.Info.SubProfileInfo.Cdkeyenc = _request.CDKeyEnc;
            StorageOperation.Persistance.UpdateSubProfileInfo(_client.Info.SubProfileInfo);
        }
    }
}
