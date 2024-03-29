using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Auth.Abstraction;
using UniSpy.Server.WebServer.Module.Auth.Contract.Request;
using UniSpy.Server.WebServer.Module.Auth.Contract.Response;
using UniSpy.Server.WebServer.Module.Auth.Contract.Result;
using UniSpy.Server.Core.Database.DatabaseModel;
using UniSpy.Server.WebServer.Application;

namespace UniSpy.Server.WebServer.Module.Auth.Handler
{

    public class LoginRemoteAuthHandler : CmdHandlerBase
    {
        protected new LoginRemoteAuthRequest _request => (LoginRemoteAuthRequest)base._request;
        protected new LoginResultBase _result { get => (LoginResultBase)base._result; set => base._result = value; }
        public LoginRemoteAuthHandler(Client client, LoginRemoteAuthRequest request) : base(client, request)
        {
            _result = new LoginRemoteAuthResult();
        }
        protected override void DataOperation()
        {
            using (var db = new UniSpyContext())
            {
                // var result = from p in db.Profiles
                //              join u in db.Users on p.Userid equals u.UserId
                //              join sp in db.Subprofiles on p.ProfileId equals sp.ProfileId
                //              where sp.Authtoken == _request.AuthToken &&
                //                     sp.PartnerId == _request.GameId
                //              select new { u, p, sp };
                // if (result.Count() != 1)
                // {
                //     throw new System.Exception("No account exists with the provided email address.");
                // }

                // var data = result.First();
                // _result.UserId = data.u.UserId;
                // _result.ProfileId = data.p.ProfileId;
                // _result.CdKeyHash = data.sp.Cdkeyenc;
                // currently we set this to uniquenick
                // _result.ProfileNick = data.sp.Uniquenick;
                _result.UserId = 1;
                _result.ProfileId = 1;
                _result.CdKeyHash = "00000000000000s";
                _result.ProfileNick = "xiaojiuwo";
                _result.UniqueNick = "xiaojiuwo";
            }
        }
        protected override void ResponseConstruct()
        {
            _response = new LoginRemoteAuthResponse(_request, _result);
        }
    }
}