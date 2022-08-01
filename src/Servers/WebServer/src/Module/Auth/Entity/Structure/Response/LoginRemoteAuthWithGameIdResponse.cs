using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Auth.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request;
namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Response
{
    public class LoginRemoteAuthWithGameIdResponse : LoginResponseBase
    {
        protected new LoginRemoteAuthWithGameIdRequest _request => (LoginRemoteAuthWithGameIdRequest)base._request;
        protected new LoginResultBase _result => (LoginResultBase)base._result;
        public LoginRemoteAuthWithGameIdResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            _content.Add("LoginRemoteAuthWithGameIdResult");
            BuildContext();
            base.Build();
        }
    }
}