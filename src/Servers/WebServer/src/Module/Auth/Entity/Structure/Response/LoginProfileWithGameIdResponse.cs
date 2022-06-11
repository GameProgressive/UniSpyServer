using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Auth.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Response
{
    public class LoginProfileWithGameIdResponse : LoginResponseBase
    {
        protected new LoginProfileWithGameIdRequest _request => (LoginProfileWithGameIdRequest)base._request;
        protected new LoginResultBase _result => (LoginResultBase)base._result;
        public LoginProfileWithGameIdResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            _content.Add("LoginProfileWithGameIdResult");
            BuildContext();
            base.Build();
        }
    }
}