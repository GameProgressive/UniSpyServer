using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Auth.Abstraction;
using UniSpy.Server.WebServer.Module.Auth.Contract.Request;

namespace UniSpy.Server.WebServer.Module.Auth.Contract.Response
{
    public class LoginUniqueNickResponse : LoginResponseBase
    {
        protected new LoginRemoteAuthRequest _request => (LoginRemoteAuthRequest)base._request;
        protected new LoginResultBase _result => (LoginResultBase)base._result;
        public LoginUniqueNickResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            _content.Add("LoginUniqueNickResult");
            BuildContext();
            base.Build();
        }
    }
}