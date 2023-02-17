using System.Linq;
using UniSpy.Server.WebServer.Module.Auth.Entity.Structure.Request;
using UniSpy.Server.WebServer.Module.Auth.Entity.Structure.Response;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Database.DatabaseModel;

namespace UniSpy.Server.WebServer.Module.Auth.Handler
{
    
    public sealed class LoginUniqueNickWithGameIdHandler : LoginUniqueNickHandler
    {
        private new LoginUniqueNickRequest _request => (LoginUniqueNickRequest)base._request;
        public LoginUniqueNickWithGameIdHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            using (var db = new UniSpyContext())
            {
                var result = from p in db.Profiles
                             join u in db.Users on p.Userid equals u.UserId
                             join sp in db.Subprofiles on p.ProfileId equals sp.ProfileId
                             where sp.Uniquenick == _request.Uniquenick &&
                                sp.NamespaceId == _request.NamespaceId
                             select new { u, p, sp };
                if (result.Count() != 1)
                {
                    throw new System.Exception("No account exists with the provided email address.");
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
            // base.ResponseConstruct();
            _response = new LoginUniqueNickWithGameIdResponse(_request, _result);
        }
    }
}