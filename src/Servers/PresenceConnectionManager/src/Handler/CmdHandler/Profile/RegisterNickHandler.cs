using System;
using System.Linq;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("registernick")]
    public sealed class RegisterNickHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        private new RegisterNickRequest _request => (RegisterNickRequest)base._request;
        public RegisterNickHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            try
            {
                using (var db = new UniSpyContext())
                {
                    db.Subprofiles.Where(s => s.SubProfileId == _client.Info.SubProfileInfo.SubProfileId)
                        .FirstOrDefault().Uniquenick = _request.UniqueNick;
                    db.SaveChanges();
                }
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
