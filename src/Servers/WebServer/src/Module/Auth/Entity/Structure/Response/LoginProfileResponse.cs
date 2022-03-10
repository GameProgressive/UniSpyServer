using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure;
using UniSpyServer.Servers.WebServer.Module.Auth.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Response
{
    public class LoginProfileResponse : LoginResponseBase
    {
        protected new LoginProfileRequest _request => (LoginProfileRequest)base._request;
        protected new LoginResultBase _result => (LoginResultBase)base._result;
        public LoginProfileResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            _soapElement.Add(new XElement(SoapXElement.SoapNamespace + "LoginProfileWithGameIdResult"));
            BuildContext();
        }
    }
}