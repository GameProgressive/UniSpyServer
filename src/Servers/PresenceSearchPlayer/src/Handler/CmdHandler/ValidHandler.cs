using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Contract;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Handler.CmdHandler
{
    [HandlerContract("valid")]
    public sealed class ValidHandler : CmdHandlerBase
    {
        private new ValidRequest _request => (ValidRequest)base._request;
        private new ValidResult _result{ get => (ValidResult)base._result; set => base._result = value; }

        public ValidHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ValidResult();
        }
        protected override void DataOperation()
        {
            try
            {
                using (var db = new UniSpyContext())
                {
                    var result = from u in db.Users
                                //According to FSW partnerid is not nessesary
                                where u.Email == _request.Email
                                select u.UserId;

                    if (result.Count() == 0)
                    {
                        _result.IsAccountValid = false;
                    }
                    else if (result.Count() == 1)
                    {
                        _result.IsAccountValid = true;
                    }
                }
            }
            catch (System.Exception e)
            {
                throw new GPDatabaseException("Unknown error occurs in database operation.", e);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new ValidResponse(_request, _result);
        }
    }
}
