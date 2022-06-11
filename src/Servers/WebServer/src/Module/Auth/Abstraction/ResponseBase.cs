using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Abstraction
{
    public abstract class ResponseBase : WebServer.Abstraction.ResponseBase
    {
        protected ResponseBase(RequestBase request, ResultBase result) : base(request, result)
        {
            _content = new AuthSoapEnvelope();
        }
    }
}