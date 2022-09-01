using System.Linq;
using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Exception;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Handler.CmdHandler
{
    
    public sealed class CheckHandler : CmdHandlerBase
    {
        // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\
        //\cur\pid\<pid>\final
        //check is request recieved correct and convert password into our MD5 type
        private new CheckRequest _request => (CheckRequest)base._request;
        private new CheckResult _result { get => (CheckResult)base._result; set => base._result = value; }
        public CheckHandler(IClient client, IRequest request) : base(client, request)
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

                // Not every game uses PartnerId; optional
                var result = from p in db.Profiles
                            join u in db.Users on p.Userid equals u.UserId
                            join sp in db.Subprofiles on p.ProfileId equals sp.ProfileId
                            where u.Email.Equals(_request.Email)
                            && u.Password.Equals(_request.Password)
                            && p.Nick.Equals(_request.Nick)
                            || sp.PartnerId.Equals(_request.PartnerId)
                            select p.ProfileId;

                var results = result.ToList();
                if (result.Count() == 1)
                {
                    _result.ProfileId = result.First();
                }
                else
                {
                    _result.ProfileId = null;
                }
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new CheckResponse(_request, _result);
        }
    }
}
