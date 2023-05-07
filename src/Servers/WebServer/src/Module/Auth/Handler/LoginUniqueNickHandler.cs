using System.Linq;
using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Auth.Abstraction;
using UniSpy.Server.WebServer.Module.Auth.Contract.Request;
using UniSpy.Server.WebServer.Module.Auth.Contract.Response;
using UniSpy.Server.WebServer.Module.Auth.Contract.Result;
using UniSpy.Server.Core.Database.DatabaseModel;
using UniSpy.Server.WebServer.Application;

namespace UniSpy.Server.WebServer.Module.Auth.Handler
{

    public class LoginUniqueNickHandler : CmdHandlerBase
    {
        protected new LoginUniqueNickRequest _request => (LoginUniqueNickRequest)base._request;
        protected new LoginResultBase _result { get => (LoginResultBase)base._result; set => base._result = value; }
        public LoginUniqueNickHandler(Client client, LoginUniqueNickRequest request) : base(client, request)
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
                             where sp.Uniquenick == _request.Uniquenick
                             && sp.NamespaceId == _request.NamespaceId
                             //  && u.Password == _request.Password
                             select new { u, p, sp };
                if (result.Count() != 1)
                {
                    throw new Auth.Exception("No account exists with the provided email address.");
                }
                var data = result.First();
                _result.UserId = data.u.UserId;
                _result.ProfileId = data.p.ProfileId;
                // _result.CdKeyHash = data.sp.Cdkeyenc;
                _result.CdKeyHash = "xxxxxxxxxxx";
                // currently we set this to uniquenick
                _result.ProfileNick = data.p.Nick;
                _result.UniqueNick = data.sp.Uniquenick;
            }
        }
        protected override void ResponseConstruct()
        {
            _response = new LoginUniqueNickResponse(_request, _result);
        }
    }
}