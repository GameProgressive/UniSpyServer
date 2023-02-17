using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Auth.Abstraction;
using UniSpy.Server.WebServer.Module.Auth.Contract.Request;
namespace UniSpy.Server.WebServer.Module.Auth.Contract.Response
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