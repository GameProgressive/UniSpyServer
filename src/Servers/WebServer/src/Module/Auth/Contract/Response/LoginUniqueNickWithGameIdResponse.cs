using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Auth.Abstraction;
using UniSpy.Server.WebServer.Module.Auth.Contract.Request;

namespace UniSpy.Server.WebServer.Module.Auth.Contract.Response
{
    public class LoginUniqueNickWithGameIdResponse : LoginResponseBase
    {
        protected new LoginRemoteAuthRequest _request => (LoginRemoteAuthRequest)base._request;
        protected new LoginResultBase _result => (LoginResultBase)base._result;
        public LoginUniqueNickWithGameIdResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            _content.Add("LoginUniqueNickWithGameIdResult");
            BuildContext();
            base.Build();
        }
    }
}