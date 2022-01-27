using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Contract;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Exception;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Handler.CmdHandler
{
    [HandlerContract("check")]
    public sealed class CheckHandler : CmdHandlerBase
    {
        // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\
        //\cur\pid\<pid>\final
        //check is request recieved correct and convert password into our MD5 type
        private new CheckRequest _request => (CheckRequest)base._request;
        private new CheckResult _result{ get => (CheckResult)base._result; set => base._result = value; }
        public CheckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new CheckResult();
        }

        protected override void DataOperation()
        {
            using (var db = new UniSpyContext())
            {
                if (db.Users.Where(e => e.Email == _request.Email).Count() < 1)
                {
                    throw new CheckException("No account exists with the provided email address.", GPErrorCode.CheckBadMail);
                }

                if (db.Users.Where(u => u.Email == _request.Email && u.Password == _request.Password).Count() < 1)
                {
                    throw new CheckException("No account exists with the provided email address.", GPErrorCode.CheckBadPassword);
                }

                var result = from p in db.Profiles
                             join u in db.Users on p.Userid equals u.Userid
                             where u.Email.Equals(_request.Email)
                             && u.Password.Equals(_request.Password)
                             && p.Nick.Equals(_request.Nick)
                             select p.ProfileId;

                if (result.Count() == 1)
                {
                    _result.ProfileId = result.First();
                }
                else
                {
                    throw new CheckException();
                }
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new CheckResponse(_request, _result);
        }
    }
}
