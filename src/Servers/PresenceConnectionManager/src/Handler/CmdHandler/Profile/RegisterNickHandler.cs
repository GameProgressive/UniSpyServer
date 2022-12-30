using System;
using System.Linq;
using UniSpyServer.Servers.PresenceConnectionManager.Application;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler.Profile
{

    public sealed class RegisterNickHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        private new RegisterNickRequest _request => (RegisterNickRequest)base._request;
        public RegisterNickHandler(IClient client, IRequest request) : base(client, request)
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
                StorageOperation.Persistance.UpdateUniqueNick(_client.Info.SubProfileInfo.SubProfileId, _request.UniqueNick);
            }
            catch (Exception e)
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
