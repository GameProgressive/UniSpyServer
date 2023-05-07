using System.Linq;
using UniSpy.Server.WebServer.Module.Auth.Contract.Request;
using UniSpy.Server.WebServer.Module.Auth.Contract.Response;
using UniSpy.Server.Core.Database.DatabaseModel;
using UniSpy.Server.WebServer.Application;

namespace UniSpy.Server.WebServer.Module.Auth.Handler
{

    public sealed class LoginRemoteAuthWithGameIdHandler : LoginRemoteAuthHandler
    {
        private new LoginRemoteAuthWithGameIdRequest _request => (LoginRemoteAuthWithGameIdRequest)base._request;
        public LoginRemoteAuthWithGameIdHandler(Client client, LoginRemoteAuthWithGameIdRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            using (var db = new UniSpyContext())
            {
                var result = from p in db.Profiles
                             join u in db.Users on p.Userid equals u.UserId
                             join sp in db.Subprofiles on p.ProfileId equals sp.ProfileId
                             where sp.Authtoken == _request.AuthToken &&
                                    sp.PartnerId == _request.GameId
                             select new { u, p, sp };
                if (result.Count() != 1)
                {
                    throw new System.Exception("No account exists with the provided email address.");
                }

                var data = result.First();
                _result.UserId = data.u.UserId;
                _result.ProfileId = data.p.ProfileId;
                _result.CdKeyHash = data.sp.Cdkeyenc;
                // currently we set this to uniquenick
                _result.ProfileNick = data.sp.Uniquenick;
                _result.UniqueNick = data.sp.Uniquenick;
            }
        }
        protected override void ResponseConstruct()
        {
            // base.ResponseConstruct();
            _response = new LoginRemoteAuthWithGameIdResponse(_request, _result);
        }
    }
}