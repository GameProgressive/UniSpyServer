using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.Auth.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;
using System.Linq;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Result;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Handler
{
    [HandlerContract("LoginUniqueNick")]
    public class LoginUniqueNickHandler : CmdHandlerBase
    {
        protected new LoginUniqueNickRequest _request => (LoginUniqueNickRequest)base._request;
        protected new LoginResultBase _result { get => (LoginResultBase)base._result; set => base._result = value; }
        public LoginUniqueNickHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new LoginUniqueNickResult();
        }
        protected override void DataOperation()
        {
            using (var db = new UniSpyContext())
            {
                var result = from p in db.Profiles
                             join u in db.Users on p.Userid equals u.UserId
                             join sp in db.Subprofiles on p.ProfileId equals sp.ProfileId
                             where sp.Uniquenick == _request.Uniquenick &&
                                sp.NamespaceId == _request.NamespaceId &&
                                u.Password == _request.Password
                             select new { u, p, sp };
                if (result.Count() != 1)
                {
                    throw new System.Exception("No account exists with the provided email address.");
                }
                var data = result.First();
                _result.Certificate.UserId = data.u.UserId;
                _result.Certificate.ProfileId = data.p.ProfileId;
                _result.Certificate.CdKeyHash = data.sp.Cdkeyenc;
                // currently we set this to uniquenick
                _result.Certificate.ProfileNick = data.sp.Uniquenick;
            }
        }
    }
}