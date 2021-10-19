using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Contract;
using PresenceSearchPlayer.Entity.Exception.General;
using PresenceSearchPlayer.Entity.Structure.Request;
using PresenceSearchPlayer.Entity.Structure.Response;
using PresenceSearchPlayer.Entity.Structure.Result;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceSearchPlayer.Handler.CmdHandler
{
    [HandlerContract("valid")]
    public sealed class ValidHandler : CmdHandlerBase
    {
        private new ValidRequest _request => (ValidRequest)base._request;
        private new ValidResult _result
        {
            get => (ValidResult)base._result;
            set => base._result = value;
        }

        public ValidHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ValidResult();
        }
        protected override void DataOperation()
        {
            try
            {
                using (var db = new unispyContext())
                {
                    var result = from u in db.Users
                                     //According to FSW partnerid is not nessesary
                                 where u.Email == _request.Email
                                 select u.Userid;

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
