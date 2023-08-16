using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Application;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;
using UniSpy.Server.PresenceConnectionManager.Contract.Response;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Profile
{

    public sealed class RegisterNickHandler : LoggedInCmdHandlerBase
    {
        private new RegisterNickRequest _request => (RegisterNickRequest)base._request;
        public RegisterNickHandler(Client client, RegisterNickRequest request) : base(client, request)
        {
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();
            if (_request.UniqueNick == _client.Info.SubProfileInfo.Uniquenick)
            {
                throw new GPException("new uniquenick is identical to old uniquenick, no update needed");
            }
        }
        protected override void DataOperation()
        {
            try
            {
                StorageOperation.Persistance.UpdateUniqueNick(_client.Info.SubProfileInfo.Subprofileid,
                                                              _request.UniqueNick);
            }
            catch (System.Exception e)
            {
                throw new GPDatabaseException(e.Message);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new RegisterNickResponse(_request, _result);
        }
    }
}
