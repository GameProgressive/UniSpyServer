using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Auth.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Response
{
    public class LoginProfileResponse : LoginResponseBase
    {
        protected new LoginProfileRequest _request => (LoginProfileRequest)base._request;
        protected new LoginResultBase _result => (LoginResultBase)base._result;
        protected new AuthSoapEnvelope _content { get => (AuthSoapEnvelope)base._content; set => base._content = value; }
        public LoginProfileResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            _content.Add("LoginProfileResult");
            BuildContext();
            base.Build();
        }
    }
}